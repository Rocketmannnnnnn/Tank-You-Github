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
            if (isLocalPlayer)
            {
                CmdUpdatePosition(transform.position);
            }
        }

        for(int i = 0; i < rotationObjects.Count; i++)
        {
            if(rotationObjects[i].transform.rotation != oldRotations[i])
            {
                oldRotations[i] = rotationObjects[i].transform.rotation;
                CmdUpdateRotation(i, rotationObjects[i].transform.rotation);
            }
        }
    }

    [Command]
    private void CmdUpdatePosition(Vector3 position)
    {
        RpcUpdatePosition(position);
        transform.position = position;
        oldPosition = position;
    }

    [ClientRpc]
    private void RpcUpdatePosition(Vector3 position)
    {
        if (!isLocalPlayer)
        {
            transform.position = position;
            oldPosition = position;
        }
    }

    [Command]
    private void CmdUpdateRotation(int listIndex, Quaternion rotation)
    {
        RpcUpdateRotation(listIndex, rotation);
        rotationObjects[listIndex].transform.rotation = rotation;
        oldRotations[listIndex] = rotation;
    }

    [ClientRpc]
    private void RpcUpdateRotation(int listIndex, Quaternion rotation)
    {
        if (!isLocalPlayer)
        {
            rotationObjects[listIndex].transform.rotation = rotation;
            oldRotations[listIndex] = rotation;
        }
    }

    [ClientRpc]
    public void RpcSetPosition(Vector3 position)
    {
        if (isLocalPlayer)
        {
            transform.position = position;
        }
    }
}
