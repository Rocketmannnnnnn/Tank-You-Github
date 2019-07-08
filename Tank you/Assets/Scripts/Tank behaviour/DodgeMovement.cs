using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DodgeMovement : MonoBehaviour
{
    private TankManagerV2 tankManager;
    private GameObject enemy;
    private NavMeshAgent agent;

    [SerializeField]
    private float bulletScanRadius = 30;

    [SerializeField]
    private float dodgeOffset = 5;

    [SerializeField]
    private MonoBehaviour movementScript;

    private void Start()
    {
        movementScript.enabled = true;
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (enemy == null)
        {
            enemy = tankManager.getClosestTargetInSight(gameObject);
        }

        if (enemy != null)
        {
            if (tankManager.isEnemyInSight(gameObject, enemy))
            {
                if (incommingBullet())
                {
                    //dodge
                    movementScript.enabled = false;
                } else
                {
                    movementScript.enabled = true;
                }
            }
            else
            {
                movementScript.enabled = true;
            }
        }
    }

    private bool incommingBullet()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, bulletScanRadius);

        if(hitColliders.Length == 0)
        {
            return false;
        }

        List<GameObject> bullets = new List<GameObject>();

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].CompareTag("Bullet"))
            {
                bullets.Add(hitColliders[i].gameObject);
            }
        }

        if (bullets.Count == 0)
        {
            return false;
        }

        GameObject firstToHit = null;
        float closestDistance = Mathf.Infinity;

        //Check if bullets's line would collide with gameObject's line
        foreach(GameObject bullet in bullets)
        {
            Ray ray = new Ray(bullet.transform.position, bullet.transform.forward);
            RaycastHit hit;
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.transform.gameObject == gameObject)
                {
                    float distance = Vector3.Distance(transform.position, bullet.transform.position);

                    if (closestDistance > distance)
                    {
                        closestDistance = distance;
                        firstToHit = bullet;
                    }
                }
            }
        }

        if(firstToHit == null)
        {
            return false;
        }

        if (Vector3.Distance(transform.position, firstToHit.transform.position - firstToHit.transform.right) < 
            Vector3.Distance(transform.position, firstToHit.transform.position + firstToHit.transform.right))
        {
            Vector3 position = transform.position - firstToHit.transform.right * dodgeOffset;
            agent.SetDestination(position);
        }
        else
        {
            Vector3 position = transform.position + firstToHit.transform.right * dodgeOffset;
            agent.SetDestination(position);
        }
        return true;
    }
}
