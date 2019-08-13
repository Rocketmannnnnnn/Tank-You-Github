using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class FadeManager : MonoBehaviour
{
    private int nextLevelIndex;
    private Animator anim;

    private float loadMoment;
    private float loadDelay = 8.35f;
    private bool loading = false;
    private PauseManager pauseManager;

    public bool killMusic = false;

    [SerializeField]
    private GameObject levelmanager;
    
    [SerializeField]
    private bool mainMenu = false;

    [SerializeField]
    private TextMeshProUGUI missionText;

    [SerializeField]
    private TextMeshProUGUI livesText;

    [SerializeField]
    private CanvasGroup group;

    private void Start()
    {
        nextLevelIndex = SceneManager.GetActiveScene().buildIndex + 1;

        if (nextLevelIndex > 100)
        {
            nextLevelIndex = 0;
        }

        anim = GetComponent<Animator>();

        if (!mainMenu)
        {
            anim.Play("FadeIn");
        } else
        {
            anim.Play("FadeIdle");
        }

        if(!mainMenu)
        pauseManager = levelmanager.GetComponent<PauseManager>();
    }

    void Update()
    {
        if (loading)
        {
            if(Time.time > loadMoment)
            {
                if (killMusic)
                {
                    Destroy(GameObject.FindWithTag("Music"));
                }
                SceneManager.LoadScene(nextLevelIndex);
            }
        }
    }

    public void fadeOut()
    {
        anim.Play("FadeOut");
        loadMoment = Time.time + loadDelay;
        fade();
    }

    public void bonusFadeOut()
    {
        anim.Play("BonusTank");
        loadDelay = 9.35f;
        loadMoment = Time.time + loadDelay;
        fade();
    }

    private void fade()
    {
        loading = true;
        missionText.text = "MISSION " + nextLevelIndex;
        livesText.text = "LIVES: " + GameManager.lives;
        if (!mainMenu)
            pauseManager.enabled = false;
        GameManager.run(false);
    }

    public void win()
    {
        anim.Play("Win");
        loadDelay = 8.9f;
        loadMoment = Time.time + loadDelay;
        loading = true;
        GameManager.run(false);
        if (!mainMenu)
            pauseManager.enabled = false;
    }

    public void lose()
    {
        anim.Play("Lose");
        loadMoment = Time.time + loadDelay;
        loading = true;
        GameManager.run(false);
        Cursor.visible = true;
        if (!mainMenu)
            pauseManager.enabled = false;
    }

    public void sceneToLoad(int buildindex)
    {
        nextLevelIndex = buildindex;
    }
}
