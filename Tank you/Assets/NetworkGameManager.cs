using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;

public class NetworkGameManager : NetworkBehaviour
{
    private Animator anim;

    [SerializeField]
    private GameObject fireworks;

    [SerializeField]
    private TMP_Text winText;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void gameOverUI(string winner)
    {
        fireworks.SetActive(true);
        winText.SetText(winner + " won!");
        anim.SetBool("GameOver", true);
    }
}
