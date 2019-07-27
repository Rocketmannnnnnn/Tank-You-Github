using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private TankManagerV2 tankManager;
    private FadeManager fadeManager;
    private bool running;
    private bool playing;
    private float fadeInDelay = 3.5f;
    private float idleMoment;
    private PauseManager pauseManager;
    private GameObject musicInstance;

    [SerializeField]
    private Transform playerSpawn;

    [SerializeField]
    private GameObject soloPlayer;

    [SerializeField]
    private List<GameObject> duoPlayer = new List<GameObject>();

    [SerializeField]
    private GameObject fadeObject;

    [SerializeField]
    private AudioMixer mainMixer;

    [SerializeField]
    private AudioMixer musicMixer;

    [SerializeField]
    private bool lastLevel = false;

    [SerializeField]
    private GameObject fireWorks;

    [SerializeField]
    private GameObject musicObject;

    private void Start()
    {
        idleMoment = Time.time + fadeInDelay;
        running = false;
        playing = true;
        fadeManager = fadeObject.GetComponent<FadeManager>();

        if (GameManager.soloGame)
        {
            Instantiate(soloPlayer, playerSpawn.transform.position, playerSpawn.transform.rotation);
        } else
        {
            for(int i = 0; i < duoPlayer.Count; i++)
            {
                Instantiate(duoPlayer[i], playerSpawn.transform.position - Vector3.right * 4 * i + Vector3.right * 2, playerSpawn.transform.rotation);
            }
        }

        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();

        mainMixer.SetFloat("volume", GameManager.volume);
        musicMixer.SetFloat("musicVolume", GameManager.volume);

        if(GameManager.cameraDistance != 0)
        {
            GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
            camera.GetComponent<Camera>().orthographicSize = GameManager.cameraDistance;
        }
        GameManager.run(running);

        if(GameObject.FindWithTag("Music") == null)
        {
            musicInstance = Instantiate(musicObject);
            DontDestroyOnLoad(musicInstance);
        }

        pauseManager = GetComponent<PauseManager>();
        pauseManager.enabled = false;
    }

    private void Update()
    {
        if (running && playing)
        {
            //One team is left :(
            if (!tankManager.multipleTeams())
            {
                //Player(s) won :)
                if (tankManager.isPlayerAlive())
                {
                    if (lastLevel)
                    {
                        fireWorks.SetActive(true);
                        
                        if (PlayerPrefs.HasKey("savedLevelNumber") && PlayerPrefs.HasKey("lives"))
                        {
                            PlayerPrefs.DeleteKey("savedLevelNumber");
                            PlayerPrefs.DeleteKey("lives");
                        }
                        fadeManager.win();
                        fadeManager.sceneToLoad(0);
                    } else
                    {
                        //Bonus tank :)
                        if (SceneManager.GetActiveScene().buildIndex % 5 == 0)
                        {
                            GameManager.lives++;
                            fadeManager.bonusFadeOut();

                            if (SceneManager.GetActiveScene().buildIndex % 10 == 0)
                            {
                                if (PlayerPrefs.HasKey("maxPassedLevelIndex"))
                                {
                                    if (PlayerPrefs.GetInt("maxPassedLevelIndex") < SceneManager.GetActiveScene().buildIndex + 1)
                                    {
                                        PlayerPrefs.SetInt("maxPassedLevelIndex", SceneManager.GetActiveScene().buildIndex + 1);
                                    }
                                }
                                else
                                {
                                    PlayerPrefs.SetInt("maxPassedLevelIndex", SceneManager.GetActiveScene().buildIndex + 1);
                                }
                            }
                        }
                        else
                        {
                            fadeManager.fadeOut();
                        }
                    }
                }
                else
                {
                    //Player lost this round :(
                    GameManager.lives--;

                    //Player is out of lives :(
                    if (GameManager.lives <= 0)
                    {
                        if (PlayerPrefs.HasKey("savedLevelNumber") && PlayerPrefs.HasKey("lives"))
                        {
                            PlayerPrefs.DeleteKey("savedLevelNumber");
                            PlayerPrefs.DeleteKey("lives");
                        }
                        fadeManager.sceneToLoad(0);
                        fadeManager.lose();
                    }
                    else
                    {
                        fadeManager.sceneToLoad(SceneManager.GetActiveScene().buildIndex);
                        fadeManager.fadeOut();

                        PlayerPrefs.SetInt("savedLevelNumber", SceneManager.GetActiveScene().buildIndex);
                        PlayerPrefs.SetInt("lives", GameManager.lives);
                    }
                }
                playing = false;
            }
        } else if (!running)
        {
            if(Time.time > idleMoment)
            {
                running = true;
                GameManager.run(running);
                pauseManager.enabled = true;
            }
        }
    }
}
