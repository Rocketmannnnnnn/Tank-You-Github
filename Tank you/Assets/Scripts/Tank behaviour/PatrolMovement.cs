using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolMovement : MonoBehaviour
{
    private int targetPoint;
    private NavMeshAgent agent;
    private float targetSwapTime;
    private TankManagerV2 tankManager;

    [SerializeField]
    private Transform[] patrolPoints;

    private void Start()
    {
        targetPoint = 0;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(patrolPoints[targetPoint].position);
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
    }

    private void Update()
    {
        GameObject enemy = tankManager.getClosestTargetInSight(gameObject);

        if (enemy != null)
        {
            agent.SetDestination(enemy.transform.position);
        }
        if (agent.velocity == Vector3.zero && Time.time > targetSwapTime || Vector3.Distance(agent.destination, transform.position) < 2)
        {
            setDestination();
        }
    }

    private void setDestination()
    {
        agent.SetDestination(patrolPoints[targetPoint].position);
        targetPoint++;
        targetPoint = targetPoint % patrolPoints.Length;
        targetSwapTime = Time.time + 0.25f;
    }
}
