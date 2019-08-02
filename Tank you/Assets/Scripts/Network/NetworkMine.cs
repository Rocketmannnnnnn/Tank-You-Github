using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkMine : NetworkBehaviour
{
    private GameObject playerID;
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
        setNullParent(other.transform.root.gameObject);

        if (wentOff || !(playerID == other.transform.root.gameObject))
        {
            if (!(other.gameObject.CompareTag(notDestroyableTag) || other.transform.root.gameObject.CompareTag(notDestroyableTag)) &&
                !(other.gameObject.CompareTag("Mine") || other.transform.root.gameObject.CompareTag("Mine")))
            {
                explode();

                if (other.transform.root.gameObject.CompareTag("Tank"))
                {
                    other.transform.root.gameObject.BroadcastMessage("Cmddie");
                }
                else
                {
                    Destroy(other.transform.root.gameObject);
                }
            }
            else if (other.gameObject.CompareTag("Mine") || other.transform.root.gameObject.CompareTag("Mine"))
            {
                explode();
            }
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

    public void setParent(GameObject parent)
    {
        this.playerID = parent;
    }

    public void setNullParent(GameObject parent)
    {
        if (playerID == null && parent.CompareTag("Tank"))
        {
            playerID = parent;
        }
    }
}
