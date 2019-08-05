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

    private void Start()
    {
        anim = GetComponent<Animator>();
        lobbyManager = GameObject.Find("LobbyManager").GetComponent<LobbyManager>();
    }

    public void deadTank()
    {
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");
        if (tanks.Length == 1)
        {
            gameOverUI(tanks[0].GetComponent<SetupLocalPlayer>().playerName);
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
