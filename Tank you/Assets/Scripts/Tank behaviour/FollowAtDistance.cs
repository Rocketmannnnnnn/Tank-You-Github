using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowAtDistance : MonoBehaviour
{
    private bool enemySeen;
    private Vector3 lastEnemyPos;
    private TankManagerV2 tankManager;
    private NavMeshAgent agent;
    private GameObject enemy;

    [SerializeField]
    private float distance = 15f;

    void Start()
    {
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (enemy == null)
        {
            enemy = tankManager.getClosestTargetInSight(gameObject);
        } else
        {
            if(tankManager.isEnemyInSight(gameObject, enemy))
            {
                Vector3 range = transform.position - enemy.transform.position;
                range = range.normalized * distance;
                agent.SetDestination(enemy.transform.position + range);
                enemySeen = true;
                lastEnemyPos = enemy.transform.position;
            } else if (enemySeen)
            {
                enemy = null;
                agent.SetDestination(lastEnemyPos);
            }
        }
    }
}
