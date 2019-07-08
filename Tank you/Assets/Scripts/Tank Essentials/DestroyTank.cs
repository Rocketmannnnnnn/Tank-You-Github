using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTank : MonoBehaviour
{
    [SerializeField]
    private GameObject deathExplosion;

    [SerializeField]
    private GameObject cross;
    private TankManagerV2 tankManager;

    private void Start()
    {
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            die();
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
                    die();
                }
            }
        }
    }
    
    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.name.Contains("FlameThrower"))
        {
            die();
        }
    }

    private void die()
    {
        tankManager.removeFromList(gameObject);
        Instantiate(deathExplosion, transform.position, transform.rotation);
        Vector3 crossPosition = transform.position;
        crossPosition.y = 0.01f;
        if(cross != null)
        Instantiate(cross, crossPosition, transform.rotation);
        Destroy(gameObject);
    }
}
