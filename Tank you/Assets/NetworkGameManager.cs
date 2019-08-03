using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Prototype.NetworkLobby;

public class NetworkGameManager : NetworkBehaviour
{
    private Animator anim;

    [SerializeField]
    private GameObject fireworks;

    [SerializeField]
    private TMP_Text winText;

    [SerializeField]
    private bool isLastLevel = false;

    [SerializeField]
    private string nextLevel;

    private void Start()
    {
        anim = GetComponent<Animator>();
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
        if (isLastLevel)
        {
            fireworks.SetActive(true);
            winText.SetText(winner + " won!");
            anim.SetBool("GameOver", true);
            GameObject.Find("LobbyManager").GetComponentInChildren<LobbyTopPanel>().ToggleVisibility(true);
        } else
        {

        }
    }

    public bool IsLastLevel()
    {
        return isLastLevel;
    }
}
