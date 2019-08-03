using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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
        RpcDie();
        Instantiate(deathExplosion, transform.position, transform.rotation);
        Vector3 crossPosition = transform.position;
        crossPosition.y = 0.01f;
        Instantiate(cross, crossPosition, transform.rotation);
        Destroy(gameObject);
    }

    [ClientRpc]
    private void RpcDie()
    {
        Instantiate(deathExplosion, transform.position, transform.rotation);
        Vector3 crossPosition = transform.position;
        crossPosition.y = 0.01f;
        Instantiate(cross, crossPosition, transform.rotation);
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");
        if (tanks.Length == 1)
        {
            GameObject.FindWithTag("Managers").GetComponent<NetworkGameManager>().gameOverUI(tanks[0].GetComponent<SetupLocalPlayer>().playerName);
        }
    }
}
