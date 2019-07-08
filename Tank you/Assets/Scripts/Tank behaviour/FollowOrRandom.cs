using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowOrRandom : MonoBehaviour
{
    RandomMovement rmScript;
    GameObject enemy;
    TankManagerV2 tankManager;
    NavMeshAgent agent;

    void Start()
    {
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
        agent = GetComponent<NavMeshAgent>();
        gameObject.AddComponent<RandomMovement>();
        rmScript = GetComponent<RandomMovement>();
    }

    void Update()
    {
        if (enemy == null)
        {
            enemy = tankManager.getClosestTargetInSight(gameObject);
        }

        if (enemy != null)
        {
            if (tankManager.isEnemyInSight(gameObject, enemy) != null)
            {
                agent.SetDestination(enemy.transform.position);
                rmScript.enabled = false;
            }
            else
            {
                rmScript.enabled = true;
                enemy = null;
            }
        }
    }
}
