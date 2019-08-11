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

    private void Start()
    {
        DontDestroyOnLoad(this);
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

    private void die()
    {
        deathAction();

        if (isServer)
        {
            RpcDie();
        } else
        {
            CmdDie();
        }
    }

    [Command]
    private void CmdDie()
    {
        deathAction();
        RpcDie();
    }

    [ClientRpc]
    private void RpcDie()
    {
        deathAction();
    }

    private void deathAction()
    {
        Instantiate(deathExplosion, transform.position, transform.rotation);
        Vector3 crossPosition = transform.position;
        crossPosition.y = 0.01f;
        Instantiate(cross, crossPosition, transform.rotation);
        gameObject.SetActive(false);
        try
        {
            ngm = GameObject.FindWithTag("Managers").GetComponent<NetworkGameManager>();

            if (ngm != null)
            {
                ngm.deadTank();
            }
        }
        catch (NullReferenceException)
        {

        }
    }
}
