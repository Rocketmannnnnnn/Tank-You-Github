using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fart : MonoBehaviour
{
    private GameObject parent;
    private string teamtag;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.gameObject != parent)
        {
            if (other.transform.root.gameObject.CompareTag("Tank"))
            {
                if(other.transform.root.GetComponent<TankTag>() != null)
                {
                    if (!other.transform.root.GetComponent<TankTag>().getTeamTag().Equals(teamtag))
                    {
                        other.transform.root.gameObject.BroadcastMessage("die");
                    }
                }
            }
        }

        if(other.gameObject != parent)
        {
            if (other.gameObject.CompareTag("Tank"))
            {
                if(other.gameObject.GetComponent<TankTag>() != null)
                {
                    if (!other.gameObject.GetComponent<TankTag>().getTeamTag().Equals(teamtag))
                    {
                        other.gameObject.BroadcastMessage("die");
                    }
                }
            }
        }
    }

    public void setParent(GameObject parent)
    {
        this.parent = parent;
        this.teamtag = parent.GetComponent<TankTag>().getTeamTag();
    }
}
