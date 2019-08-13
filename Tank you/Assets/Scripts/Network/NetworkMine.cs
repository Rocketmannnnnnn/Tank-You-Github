using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkMine : NetworkBehaviour
{
    [SyncVar]
    private int playerID = -1;

    private bool wentOff = false;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private bool usesTimer = true;

    [SerializeField]
    private string notDestroyableTag;

    [SerializeField]
    private float explosionMoment = 4f;

    [SerializeField]
    private AudioSource mineExplosion;

    private void Start()
    {
        explosionMoment += Time.time;
    }

    private void Update()
    {
        if (usesTimer)
        {
            if (Time.time > explosionMoment && !wentOff)
            {
                explode();
            }
        }

        if (Time.time > explosionMoment && wentOff)
        {
            Destroy(gameObject);
        }
    }

    private void collisionCheck(Collider other)
    {
        if (other.CompareTag(notDestroyableTag) || other.transform.root.CompareTag(notDestroyableTag))
        {
            //Do nothing, its ground or something like that
            return;
        }
        else if (other.CompareTag("Mine") || other.transform.root.CompareTag("Mine"))
        {
            explode();
        }
        else if (other.transform.root.gameObject.CompareTag("Tank"))
        {
            int playerNumber = other.transform.root.GetComponent<SetupLocalPlayer>().playerNumber;

            if (!(playerID == playerNumber))
            {
                other.transform.root.gameObject.BroadcastMessage("die");
                explode();
            }
            else if (wentOff)
            {
                other.transform.root.gameObject.BroadcastMessage("die");
            }
        }
        else if (wentOff)
        {
            Destroy(other.transform.root.gameObject);
        } else
        {
            explode();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.isTrigger || other.CompareTag("Mine"))
        {
            collisionCheck(other);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!other.isTrigger || other.CompareTag("Mine"))
        {
            collisionCheck(other);
        }
    }

    private void explode()
    {
        if (!wentOff)
        {
            anim.Play("Mine");
            explosionMoment = Time.time + 1;

            if (mineExplosion != null)
            {
                mineExplosion.Play();
            }
        }
        wentOff = true;
    }

    public void setExplosionMoment(float time)
    {
        explosionMoment = time;
    }

    public void setParent(int playerNumber)
    {
        playerID = playerNumber;
    }
}
