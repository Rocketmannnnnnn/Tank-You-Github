using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporters : MonoBehaviour
{
    List<TpObjectTimer> objectList = new List<TpObjectTimer>();

    [SerializeField]
    private float teleportDelay = 2;

    [SerializeField]
    private GameObject otherTeleporter;

    [SerializeField]
    private AudioSource teleportSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tank"))
        {
            objectList.Add(new TpObjectTimer(other.gameObject, Time.time));
        }
    }

    private void OnTriggerStay(Collider other)
    {
        foreach (TpObjectTimer obj in objectList)
        {
            if (Time.time >= obj.enterTime + teleportDelay)
            {
                obj.toTeleport.transform.position = otherTeleporter.transform.position;
                teleportSound.Play();
                objectList.Remove(obj);
                break;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (TpObjectTimer obj in objectList)
        {
            if(obj.toTeleport == other.gameObject)
            {
                objectList.Remove(obj);
                break;
            }
        }
    }
}
