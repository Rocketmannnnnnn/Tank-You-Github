using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Vector3 direction;
    private int bounces;

    [SerializeField]
    private bool isLaser = false;

    [SerializeField]
    private GameObject bulletExplosion;

    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private int maxBounces = 1;

    [SerializeField]
    private AudioSource impact;

    private void Start()
    {
        direction = transform.TransformDirection(Vector3.forward);
        bounces = 0;
        speed *= 20f;
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(impact != null)
        {
            impact.volume = impact.volume / 2;
            impact.Play();
        }

        if (collision.gameObject.tag.Contains("Tank"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            Instantiate(bulletExplosion, transform.position, transform.rotation);
        }
        else
        {
            if (!collision.gameObject.name.Equals("MouseFloor"))
            {
                if (bounces < maxBounces)
                {
                    changeCourse(collision);
                    bounces++;
                }
                else
                {
                    if (!isLaser)
                    {
                        Instantiate(bulletExplosion, transform.position, transform.rotation);
                    }
                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.name.Equals("FlameThrower"))
        {
            Destroy(gameObject);
        }
    }

    private void changeCourse(Collision collision)
    {
        Vector3 oldDirection = direction;
        direction = Vector3.Reflect(oldDirection, collision.contacts[0].normal);
        transform.rotation = Quaternion.LookRotation(direction);
    }
}