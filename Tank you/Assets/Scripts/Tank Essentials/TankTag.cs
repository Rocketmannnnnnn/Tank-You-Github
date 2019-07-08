using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankTag : MonoBehaviour
{
    [SerializeField]
    private string teamTag;

    [SerializeField]
    private bool isPlayer = false;

    private void Start()
    {
        TankManagerV2 tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
        tankManager.addTank(gameObject);

        if (isPlayer)
        {
            tankManager.addPlayer(gameObject);
        }
    }

    public string getTeamTag()
    {
        return teamTag;
    }

    public void setTeamTag(string teamName)
    {
        teamTag = teamName;
    }

    public bool IsPlayer()
    {
        return isPlayer;
    }
}
