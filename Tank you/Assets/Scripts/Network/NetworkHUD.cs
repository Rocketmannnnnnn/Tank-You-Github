using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class NetworkHUD : NetworkBehaviour
{
    private bool visable;
    private float showTime;

    [SerializeField]
    private int mainMenuBuildIndex = 0;

    [SerializeField]
    private float hudDelay = 0.5f;

    [SerializeField]
    private MonoBehaviour networkManagerHUD;

    [SerializeField]
    private GameObject mainButton;

    void Start()
    {
        visable = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Visable -> hide
        if(visable && Input.GetAxis("Pause") > 0 && Time.time > showTime)
        {
            networkManagerHUD.enabled = false;
            mainButton.active = false;
            visable = false;
            showTime = Time.time + hudDelay;
        } 
        // invisable -> show
        else if (!visable && Input.GetAxis("Pause") > 0 && Time.time > showTime)
        {
            networkManagerHUD.enabled = true;
            mainButton.active = true;
            visable = true;
            showTime = Time.time + hudDelay;
        }
    }

    public void loadMainMenu()
    {
        Debug.Log("LoadMainMenu()");
        GetComponent<NetworkManager>().StopHost();
        SceneManager.LoadScene(mainMenuBuildIndex);
    }
}
