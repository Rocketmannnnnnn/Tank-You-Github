using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeOnContact : MonoBehaviour
{
    private bool hasSpawned = false;

    [SerializeField]
    private bool destroyByFire = true;

    [SerializeField]
    private GameObject newObject;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(collision.gameObject);
            destroy("Bullet");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mine"))
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, other.gameObject.transform.position);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.gameObject == other.gameObject)
                {
                    destroy();
                }
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.name.Contains("FlameThrower") && destroyByFire)
        {
            destroy();
        }
    }

    private void destroy(string killer)
    {
        if (newObject != null && killer.Equals("Bullet") && !hasSpawned)
        {
            Instantiate(newObject, transform.position, transform.rotation);
            hasSpawned = true;
        }
        Destroy(gameObject);
    }

    private void destroy()
    {
        destroy("");
    }
}
