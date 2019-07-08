using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ExplodeWhenNear : MonoBehaviour
{
    private TankManagerV2 tankManager;
    private NavMeshAgent agent;
    private GameObject enemy;

    [SerializeField]
    private Transform mineSpawnPosition;

    [SerializeField]
    private GameObject mine;

    [SerializeField]
    private float explodeDistance = 5f;

    private void Start()
    {
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if(enemy == null)
        {
            enemy = tankManager.getClosestTargetInSight(gameObject);
        }

        if(enemy != null)
        {
            if(tankManager.isEnemyInSight(gameObject, enemy))
            {
                agent.SetDestination(enemy.transform.position);

                if(Vector3.Distance(transform.position, enemy.transform.position) <= explodeDistance)
                {
                    GameObject instantiatedMine = Instantiate(mine, mineSpawnPosition.position, mineSpawnPosition.rotation);
                    instantiatedMine.GetComponent<MineV2>().setExplosionMoment(0);
                }
            }
        }
    }
}
