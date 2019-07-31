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

    [SyncVar]
    public Color playerColor = Color.white;

    void Start()
    {
        if (isLocalPlayer)
        {
            GetComponent<MPPlayerTankController>().enabled = true;
        }

        barrel.GetComponent<Renderer>().material.color = playerColor;
        frame.GetComponent<Renderer>().material.color = playerColor;
    }
}
