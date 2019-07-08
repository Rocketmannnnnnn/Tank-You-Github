using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System;

public class HideMovement : MonoBehaviour
{
    private GameObject enemy;
    private NavMeshAgent agent;
    private TankManagerV2 tankManager;
    private List<NavMeshObstacle> obstacles;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
        findObstacles();
    }

    private void Update()
    {
        enemy = tankManager.getClosestTargetInSight(gameObject);

        if (tankManager.multipleTeams() && enemy != null)
        {
            Ray ray = new Ray(transform.position, enemy.transform.position - transform.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == enemy.transform)
                {
                    if(obstacles.Count != 0)
                    {
                        GameObject closestObject = null;
                        float distanceToClosest = 200;

                        try
                        {
                            foreach (NavMeshObstacle obstacle in obstacles)
                            {
                                if (obstacle == null)
                                {
                                    findObstacles();
                                    closestObject = null;
                                    break;
                                }

                                float distance = Vector3.Distance(transform.position, obstacle.transform.position);

                                if (distance < distanceToClosest)
                                {
                                    closestObject = obstacle.gameObject;
                                    distanceToClosest = distance;
                                }
                            }

                            if (closestObject != null)
                            {
                                Vector3 direction = closestObject.transform.position - enemy.transform.position;
                                direction = direction.normalized;

                                Bounds objectSize = closestObject.GetComponentInChildren<Renderer>().bounds;
                                Vector3 offset = new Vector3(objectSize.size.x, 0, objectSize.size.z) / 1.5f;

                                direction = direction * offset.magnitude;
                                agent.SetDestination(closestObject.transform.position + direction);
                            }
                        }
                        catch (Exception) { }

                    } else
                    {
                        if(enemy != null)
                        {
                            agent.SetDestination(enemy.transform.position);
                        }
                    }
                } else
                {
                    enemy = null;
                }
            }
        }
    }

    private void findObstacles()
    {
        NavMeshObstacle[] navMeshObstacles = FindObjectsOfType<NavMeshObstacle>();

        obstacles = new List<NavMeshObstacle>();

        foreach (NavMeshObstacle o in navMeshObstacles)
        {
            obstacles.Add(o);
        }
    }
}
