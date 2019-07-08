using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    private bool running = true;
    private float retriggerTime;

    [SerializeField]
    private float pressDelay = 0.5f;

    [SerializeField]
    private CanvasGroup group;

    private void Start()
    {
        retriggerTime = 0;
    }

    private void Update()
    {
        if(Input.GetAxis("Pause") > 0 && Time.time >= retriggerTime)
        {
            menu();
            retriggerTime = Time.time + pressDelay;
        }
    }

    public void menu()
    {
        running = !running;

        if (running)
        {
            group.alpha = 0;
            group.interactable = false;
            group.blocksRaycasts = false;
            GameManager.run(true);
            Cursor.visible = false;
        } else
        {
            group.alpha = 1;
            group.interactable = true;
            group.blocksRaycasts = true;
            GameManager.run(false);
            Cursor.visible = true;
        }
    }

    public void saveAndQuit()
    {
        PlayerPrefs.SetInt("savedLevelNumber", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("lives", GameManager.lives);
        Cursor.visible = true;
        Destroy(GameObject.FindWithTag("Music"));
        SceneManager.LoadScene(0);
    }
}
