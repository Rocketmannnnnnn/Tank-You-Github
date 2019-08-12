using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Prototype.NetworkLobby;

public class NetworkGameManager : NetworkBehaviour
{
    private Animator anim;
    private LobbyManager lobbyManager;
    private NetworkLobbyHook hook;

    [SerializeField]
    private GameObject fireworks;

    [SerializeField]
    private TMP_Text winText;

    [SerializeField]
    private bool isLastLevel = false;

    [SerializeField]
    private string nextLevel;

    [SerializeField]
    private float loadDelay = 6f;

    [SerializeField]
    private bool firstLevel = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        GameObject lm = GameObject.Find("LobbyManager");
        lobbyManager = lm.GetComponent<LobbyManager>();
        hook = lm.GetComponent<NetworkLobbyHook>();

        if (firstLevel)
        {
            hook.initPlayerList();
        } else
        {
            if (isServer)
            {
                foreach (GameObject tank in hook.getPlayerList())
                {
                    tank.GetComponent<TankActivionSetter>().SetAllActive(true);
                }
            }
        }
    }

    public void deadTank()
    {
        int activeTanks = 0;
        GameObject activeTank = null;

        foreach(GameObject tank in GameObject.FindGameObjectsWithTag("Tank"))
        {
            if (tank.GetComponent<TankActivionSetter>().isActive)
            {
                activeTanks++;
                activeTank = tank;
            }
        }
        
        if (activeTanks == 1)
        {
            gameOverUI(activeTank.GetComponent<SetupLocalPlayer>().playerName);
        }
    }

    public void gameOverUI(string winner)
    {
        fireworks.SetActive(true);
        winText.SetText(winner + " won!");
        anim.SetBool("GameOver", true);

        if (isLastLevel)
        {
            anim.SetBool("LastLevel", true);
            lobbyManager.gameObject.GetComponentInChildren<LobbyTopPanel>().ToggleVisibility(true);
        } else
        {
            if (isServer)
            {
                Invoke("loadNextLevel", loadDelay);
            }
        }
    }

    public bool IsLastLevel()
    {
        return isLastLevel;
    }

    void loadNextLevel()
    {
        lobbyManager.ServerChangeScene(nextLevel);
    }
}
