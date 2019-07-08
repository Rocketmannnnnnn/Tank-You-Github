using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineV2 : MonoBehaviour
{
    private GameObject parent;
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
        if (wentOff || !(parent == other.transform.root.gameObject))
        {
            /*
            if ((!other.gameObject.CompareTag(notDestroyableTag) || !other.transform.root.gameObject.CompareTag(notDestroyableTag)) &&
                (!other.gameObject.CompareTag("Mine") || !other.transform.root.gameObject.CompareTag("Mine")))
             */
            if (!(other.gameObject.CompareTag(notDestroyableTag) || other.transform.root.gameObject.CompareTag(notDestroyableTag)) &&
                !(other.gameObject.CompareTag("Mine") || other.transform.root.gameObject.CompareTag("Mine")))
            {
                explode();

                if (other.transform.root.gameObject.CompareTag("Tank"))
                {
                    other.transform.root.gameObject.BroadcastMessage("die");
                }
                else
                {
                    Destroy(other.transform.root.gameObject);
                }
            } else if (other.gameObject.CompareTag("Mine") || other.transform.root.gameObject.CompareTag("Mine"))
            {
                explode();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        collisionCheck(other);
    }

    private void OnTriggerStay(Collider other)
    {
        collisionCheck(other);
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
        this.parent = parent;
    }
}
