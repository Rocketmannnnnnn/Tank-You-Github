using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SonarMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private TankManagerV2 tankManager;
    private GameObject enemy;
    private SonarArm scanner;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
        scanner = GetComponentInChildren<SonarArm>();
    }

    private void Update()
    {
        if(enemy == null)
        {
            enemy = tankManager.getClosestTargetInSight(gameObject);
        }

        if(enemy != null)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, enemy.transform.position - transform.position);

            if (Physics.Raycast(ray, out hit))
            {
                //Enemy is in sight, else if: enemy not in sight but hit by scanner
                if (hit.transform == enemy.transform)
                {
                    agent.SetDestination(enemy.transform.position);
                } else if (scanner.hitLastScan())
                {
                    agent.SetDestination(scanner.getEnemy());
                } else
                {
                    enemy = null;
                }
            }
        } else 
        {
            //Scanner has a target
            if (scanner.hitLastScan())
            {
                agent.SetDestination(scanner.getEnemy());
            }
        }
    }
}
