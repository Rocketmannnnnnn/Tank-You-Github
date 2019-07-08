using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomMovement : MonoBehaviour
{
    private NavMeshAgent agent;
    private bool atDestination = false;
    private Vector3 startPosition;
    private float moveTimer;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (atDestination)
        {
            //Get a Random position in the playfield
            agent.SetDestination(randomPosition());
            startPosition = transform.position;
            atDestination = false;
        }
        else
        {
            //This makes sure the tank moves every second || This makes sure the tank is at destination (which is not its start position)
            if (agent.velocity.magnitude < 1 && moveTimer < Time.time || agent.velocity == Vector3.zero && transform.position != startPosition)
            {
                moveTimer = Time.time + 1;
                atDestination = true;
            }
        }
    }

    private Vector3 randomPosition()
    {
        return new Vector3(Random.Range(-40, 40), 0, Random.Range(-50, 50));
    }
}
