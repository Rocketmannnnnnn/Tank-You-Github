using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RaceTank : MonoBehaviour
{
    private TankManagerV2 tankManager;
    private GameObject enemy;
    private NavMeshAgent agent;
    private float attackMoment;
    private GameObject instantiatedMine;

    [SerializeField]
    private float dropdistance = 5f;

    [SerializeField]
    private float flyByTime = 5f;

    [SerializeField]
    private GameObject mine;

    [SerializeField]
    private RandomMovement rmScript;

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
            if(tankManager.isEnemyInSight(gameObject, enemy) != null)
            {
                agent.SetDestination(enemy.transform.position);
                rmScript.enabled = false;

                if (Vector3.Distance(transform.position, enemy.transform.position) <= dropdistance)
                {
                    drop();
                }
            } else
            {
                if(Vector3.Distance(transform.position, agent.destination) < 5)
                    rmScript.enabled = true;
                enemy = null;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == instantiatedMine)
        {
            instantiatedMine.GetComponent<NavMeshObstacle>().enabled = true;
        }
    }

    private void drop()
    {
        attackMoment = Time.time + flyByTime;

        instantiatedMine = Instantiate(mine, transform.position, transform.rotation);
        instantiatedMine.GetComponent<MineV2>().setParent(gameObject);
        instantiatedMine.GetComponent<NavMeshObstacle>().enabled = false;
    }
}
