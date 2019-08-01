using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TransformSync : NetworkBehaviour
{
    private Vector3 oldPosition;
    List<Quaternion> oldRotations = new List<Quaternion>();

    [SerializeField]
    private List<GameObject> rotationObjects = new List<GameObject>();

    public string playerName;

    private void Start()
    {
        oldPosition = transform.position;

        foreach(GameObject rotationObject in rotationObjects)
        {
            oldRotations.Add(rotationObject.transform.rotation);
        }
    }

    void FixedUpdate()
    {
        if(oldPosition != transform.position)
        {
            oldPosition = transform.position;
            CmdUpdatePosition(playerName, transform.position);
        }

        for(int i = 0; i < rotationObjects.Count; i++)
        {
            if(rotationObjects[i].transform.rotation != oldRotations[i])
            {
                oldRotations[i] = rotationObjects[i].transform.rotation;
                CmdUpdateRotation(playerName, i, rotationObjects[i].transform.rotation);
            }
        }
    }

    [Command]
    private void CmdUpdatePosition(string clientName, Vector3 position)
    {
        RpcUpdatePosition(clientName, position);
    }

    [ClientRpc]
    private void RpcUpdatePosition(string clientName, Vector3 position)
    {
        if (!playerName.Equals(clientName))
        {
            transform.position = position;
            oldPosition = position;
        }
    }

    [Command]
    private void CmdUpdateRotation(string clientName, int listIndex, Quaternion rotation)
    {
        RpcUpdateRotation(clientName, listIndex, rotation);
    }

    [ClientRpc]
    private void RpcUpdateRotation(string clientName, int listIndex, Quaternion rotation)
    {
        if (!playerName.Equals(clientName))
        {
            rotationObjects[listIndex].transform.rotation = rotation;
            oldRotations[listIndex] = rotation;
        }
    }
}
