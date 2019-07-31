using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkDestroyTank : NetworkBehaviour
{
    [SerializeField]
    private GameObject deathExplosion;

    [SerializeField]
    private GameObject cross;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            Cmddie();
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
                    Cmddie();
                }
            }
        }
    }

    [Command]
    private void Cmddie()
    {
        Instantiate(deathExplosion, transform.position, transform.rotation);
        Vector3 crossPosition = transform.position;
        crossPosition.y = 0.01f;
        Instantiate(cross, crossPosition, transform.rotation);
        Destroy(gameObject);
    }
}
