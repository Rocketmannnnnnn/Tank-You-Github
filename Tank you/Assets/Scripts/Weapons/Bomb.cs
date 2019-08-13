using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private bool wentOff = false;
    private Animator anim;
    private float explosionMoment = 0f;
    private Rigidbody rb;

    [SerializeField]
    private string notDestroyableTag;

    [SerializeField]
    private AudioSource bombAudio;

    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Time.time > explosionMoment && wentOff)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(rb != null)
        {
            rb.isKinematic = true;
        }
        
        explode();

        if (other.transform.root.gameObject.CompareTag("Tank"))
        {
            other.transform.root.gameObject.BroadcastMessage("die");
        }
        else
        {
            if (!other.transform.root.gameObject.CompareTag(notDestroyableTag) && !other.gameObject.CompareTag(notDestroyableTag) && !other.transform.gameObject.CompareTag("Mine"))
            {
                Destroy(other.transform.root.gameObject);
            }
        }
    }

    private void explode()
    {
        if (!wentOff)
        {
            anim.Play("Explode");
            explosionMoment = Time.time + 1;
            bombAudio.Play();
        }
        wentOff = true;
    }
}
