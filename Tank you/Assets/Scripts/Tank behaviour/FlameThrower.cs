using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FlameThrower : MonoBehaviour
{
    private GameObject enemy;
    private TankManagerV2 tankManager;
    private NavMeshAgent agent;
    private ParticleSystem flamethrower;

    [SerializeField]
    private float fireRange = 40f;

    [SerializeField]
    private float rotateSpeed = 1;

    [SerializeField]
    private GameObject barrel;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
        flamethrower = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (enemy == null)
        {
            enemy = tankManager.getClosestTargetInSight(gameObject);
        }

        //Checks if there are multiple teams
        if (tankManager.multipleTeams() && enemy != null)
        {
            //Get playerDirection
            RaycastHit hit;
            Ray ray = new Ray(transform.position, enemy.transform.position - transform.position);

            //If raycast hits
            if (Physics.Raycast(ray, out hit))
            {
                Debug.DrawLine(transform.position, hit.point, Color.red);

                //If raycast hits player
                if (hit.transform.position == enemy.transform.position)
                {
                    aim();
                    agent.SetDestination(enemy.transform.position);

                    if (Vector3.Distance(transform.position, enemy.transform.position) < fireRange)
                    {
                        var emission = flamethrower.emission;
                        emission.rateOverTime = 30;
                    } else
                    {
                        var emission = flamethrower.emission;
                        emission.rateOverTime = 0;
                    }
                } else
                {
                    enemy = null;
                    var emission = flamethrower.emission;
                    emission.rateOverTime = 0;
                }
            }
        } else
        {
            var emission = flamethrower.emission;
            emission.rateOverTime = 0;
        }
    }

    //Aim towards the player or its last known location
    private void aim()
    {
        float step = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(barrel.transform.forward, barrel.transform.position - enemy.transform.position, step, 0.0f);
        newDirection.y = 0.0f;
        barrel.transform.rotation = Quaternion.LookRotation(newDirection);
    }
}
