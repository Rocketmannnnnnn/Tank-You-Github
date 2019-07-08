using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


public class MainMenuUI : MonoBehaviour
{
    private int startLevelNumber = 1;
    private int maxLevelIndex = 1;
    private int resumeLevelNumber;
    private int resumeLives;
    private TextMeshProUGUI startLevelText;
    private TextMeshProUGUI resumeLevelText;
    private int sceneToLoad;

    [SerializeField]
    private string onlineScene;

    [SerializeField]
    private CanvasGroup fullCanvas;

    [SerializeField]
    private GameObject startLevelObject;

    [SerializeField]
    private GameObject resumeLevelObject;

    [SerializeField]
    private List<GameObject> resumeGroup;

    [SerializeField]
    private List<CanvasGroup> groups;

    [SerializeField]
    private AudioMixer mainMixer;

    [SerializeField]
    private AudioMixer musicMixer;

    [SerializeField]
    private Slider volumeSlider;

    [SerializeField]
    private Slider musicSlider;

    [SerializeField]
    private Slider cameraSlider;

    [SerializeField]
    private GameObject fadeObject;

    [SerializeField]
    private MainMenuTanks tankSpawner;

    public MainMenuUI()
    {
        resumeGroup = new List<GameObject>();
        groups = new List<CanvasGroup>();
    }

    private void Start()
    {
        startLevelNumber = 1;
        startLevelText = startLevelObject.GetComponent<TextMeshProUGUI>();
        resumeLevelText = resumeLevelObject.GetComponent<TextMeshProUGUI>();

        if (PlayerPrefs.HasKey("maxPassedLevelIndex"))
        {
            maxLevelIndex = PlayerPrefs.GetInt("maxPassedLevelIndex");
        }

        if (PlayerPrefs.HasKey("savedLevelNumber") && PlayerPrefs.HasKey("lives"))
        {
            resumeLevelNumber = PlayerPrefs.GetInt("savedLevelNumber");
            resumeLives = PlayerPrefs.GetInt("lives");
            resumeLevelText.text = resumeLevelNumber.ToString();
        } else
        {
            foreach(GameObject go in resumeGroup)
            {
                go.SetActive(false);
            }
        }

        if (PlayerPrefs.HasKey("volume"))
        {
            setVolume(PlayerPrefs.GetFloat("volume"));
            volumeSlider.value = PlayerPrefs.GetFloat("volume");
        }

        if (PlayerPrefs.HasKey("musicVolume"))
        {
            setMusicVolume(PlayerPrefs.GetFloat("musicVolume"));
            musicSlider.value = PlayerPrefs.GetFloat("musicVolume");
        }

        if (PlayerPrefs.HasKey("camera"))
        {
            setCameraDistance(PlayerPrefs.GetFloat("camera"));
            cameraSlider.value = PlayerPrefs.GetFloat("camera");
        }
    }

    public void loadGroup(CanvasGroup groupToLoad)
    {
        foreach(CanvasGroup group in groups)
        {
            if(group != groupToLoad)
            {
                group.alpha = 0;
                group.interactable = false;
                group.blocksRaycasts = false;
            } else
            {
                group.alpha = 1;
                group.interactable = true;
                group.blocksRaycasts = true;
            }
        }
    }

    public void solo(bool solo)
    {
        GameManager.soloGame = solo;
    }

    private void setStartLevelText(int number)
    {
        startLevelText.text = number + "";
    }

    public void adaptStartLevelNumber(bool add)
    {
        if(maxLevelIndex == 1)
        {
            return;
        }

        if (add)
        {
            if(startLevelNumber + 10 <= maxLevelIndex && startLevelNumber + 10 < 100)
            {
                startLevelNumber += 10;
                setStartLevelText(startLevelNumber);
            }
        } else
        {
            if(startLevelNumber - 10 > 0)
            {
                startLevelNumber -= 10;
                setStartLevelText(startLevelNumber);
            }
        }
    }

    public void startGame()
    {
        GameManager.lives = 3;
        sceneToLoad = startLevelNumber;
        prepareGame();
    }

    public void resumeGame()
    {
        GameManager.lives = resumeLives;
        sceneToLoad = resumeLevelNumber;
        prepareGame();
    }

    public void prepareGame()
    {
        tankSpawner.enabled = false;

        foreach (CanvasGroup group in groups)
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
        }

        fullCanvas.alpha = 0;
        fullCanvas.interactable = false;
        fullCanvas.blocksRaycasts = false;

        Cursor.visible = false;
        
        fadeObject.GetComponent<FadeManager>().sceneToLoad(sceneToLoad);
        fadeObject.GetComponent<FadeManager>().fadeOut();
    }

    public void quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void setVolume()
    {
        setVolume(volumeSlider.value);
    }

    public void setMusicVolume()
    {
        setMusicVolume(musicSlider.value);
    }

    public void setCameraDistance()
    {
        setCameraDistance(cameraSlider.value);
    }

    private void setVolume(float volume)
    {
        mainMixer.SetFloat("volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
        GameManager.volume = volume;
    }

    private void setMusicVolume(float volume)
    {
        musicMixer.SetFloat("musicVolume", volume);
        PlayerPrefs.SetFloat("musicVolume", volume);
        GameManager.musicVolume = volume;
    }

    private void setCameraDistance(float distance)
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera");
        camera.GetComponent<Camera>().orthographicSize = distance;
        GameManager.cameraDistance = distance;
        PlayerPrefs.SetFloat("camera", distance);
    }

    public void reset()
    {
        setVolume(0);
        volumeSlider.value = 0;

        setMusicVolume(0);
        musicSlider.value = 0;

        setCameraDistance(30);
        cameraSlider.value = 30;
    }

    public void onlineGame()
    {
        SceneManager.LoadScene(onlineScene);
    }
}
