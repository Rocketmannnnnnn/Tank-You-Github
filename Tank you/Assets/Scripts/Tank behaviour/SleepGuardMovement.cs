using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SleepGuardMovement : MonoBehaviour
{
    private GameObject player;
    private NavMeshAgent agent;
    private bool awake;
    private TankManagerV2 tankManager;
    private ParticleSystem sleepZ;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
        awake = false;
        sleepZ = GetComponent<ParticleSystem>();
        sleepZ.Play();
    }

    private void Update()
    {
        if (awake)
        {
            if(player != null)
            {
                agent.SetDestination(player.transform.position);
            } else if (tankManager.multipleTeams())
            {
                player = tankManager.getClosestTargetInSight(gameObject);
            }
        }
        else
        {
            if(tankManager.getClosestTargetInSight(gameObject) != null)
            {
                player = tankManager.getClosestTargetInSight(gameObject);
                awake = true;
                sleepZ.Stop();
            }
        }
    }
}
