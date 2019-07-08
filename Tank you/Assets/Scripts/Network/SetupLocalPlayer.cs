using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SetupLocalPlayer : NetworkBehaviour
{
    [SerializeField]
    private Material mat;

    [SerializeField]
    private GameObject barrel;

    [SerializeField]
    private GameObject frame;

    void Start()
    {
        if (isLocalPlayer)
        {
            GetComponent<MPPlayerTankController>().enabled = true;
            barrel.GetComponent<Renderer>().material = mat;
            frame.GetComponent<Renderer>().material = mat;
            GetComponent<TankTag>().setTeamTag("Player");
        }
    }
}
