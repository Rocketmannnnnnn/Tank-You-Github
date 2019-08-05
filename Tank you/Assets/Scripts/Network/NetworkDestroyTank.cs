using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class NetworkDestroyTank : NetworkBehaviour
{
    private NetworkGameManager ngm;

    [SerializeField]
    private GameObject deathExplosion;

    [SerializeField]
    private GameObject cross;

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

    private void die()
    {
        if (isServer)
        {
            RpcDie();
        } else
        {
            Cmddie();
        }
    }

    [Command]
    private void Cmddie()
    {
        RpcDie();
        spawnDeathStuff();
    }

    [ClientRpc]
    private void RpcDie()
    {
        spawnDeathStuff();
    }

    private void spawnDeathStuff()
    {
        Instantiate(deathExplosion, transform.position, transform.rotation);
        Vector3 crossPosition = transform.position;
        crossPosition.y = 0.01f;
        Instantiate(cross, crossPosition, transform.rotation);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        try
        {
            ngm = GameObject.FindWithTag("Managers").GetComponent<NetworkGameManager>();

            if (ngm != null)
            {
                ngm.deadTank();
            }
        } catch (NullReferenceException)
        {

        }
    }
}
