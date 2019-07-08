using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LaserTank : MonoBehaviour
{
    private Vector3 lastEnemyPosition;
    private GameObject enemy;
    private float nextFire;
    private TankManagerV2 tankManager;
    private Renderer barrelRenderer;
    private Material material;
    private Color color;
    private NavMeshAgent agent;
    private float colorTimer = 0f;
    private Status status;

    private enum Status {chargeing, charged, dischargeing};

    [SerializeField]
    private float rotateSpeed = 1;

    [SerializeField]
    private Transform bulletSpawnpos;
    
    [SerializeField]
    private float fireDelay = 1f;

    [SerializeField]
    private float chargeTime = 2f;

    [SerializeField]
    private Material defaultMaterial;

    [SerializeField]
    private Material laserRed;

    [SerializeField]
    private GameObject laser;

    private void Start()
    {
        nextFire = 0;
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
        barrelRenderer = GetComponent<Renderer>();
        material = new Material(defaultMaterial);
        color = new Color();
        color = material.color;
        agent = GetComponentInParent<NavMeshAgent>();
    }

    void Update()
    {
        if (enemy == null)
        {
            enemy = tankManager.getClosestTargetInSight(gameObject);
        }

        if (tankManager.multipleTeams() && enemy != null)
        {
            if(status == Status.charged)
            {
                
                if (enemyInSight())
                {
                    aim();

                    agent.isStopped = true;

                    if (agent.velocity == Vector3.zero)
                    {
                        fire();
                        status = Status.dischargeing;
                    }
                } else
                {
                    agent.isStopped = false;
                    agent.SetDestination(lastEnemyPosition);
                }
            } else if (status == Status.chargeing) {
                //CHARGE
                if (enemyInSight())
                {
                    aim();
                }
                charge();
            } else if (status == Status.dischargeing)
            {
                //DISCHARGE
                if (enemyInSight())
                {
                    aim();
                }
                discharge();
            }

            if (!enemyInSight())
            {
                enemy = null;
            }
        }

        if (!tankManager.multipleTeams())
        {
            discharge();
        }
    }

    private bool enemyInSight()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, enemy.transform.position - transform.position);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform.position == enemy.transform.position)
            {
                lastEnemyPosition = enemy.transform.position;
                return true;
            }
        }

        return false;
    }

    private void charge()
    {
        agent.isStopped = true;

        if (colorTimer > 1f)
        {
            colorTimer = 1f;
            status = Status.charged;
        } else if (colorTimer < 1f)
        {
            colorTimer += Time.deltaTime / chargeTime;
        }

        if(colorTimer <= 1f)
        {
            color = Color.Lerp(defaultMaterial.color, laserRed.color, colorTimer);
            barrelRenderer.material.color = color;
        }
    }

    private void discharge()
    {
        agent.isStopped = false;

        if(enemy != null)
        {
            agent.SetDestination(lastEnemyPosition);
        }

        if (colorTimer < 0)
        {
            colorTimer = 0f;
            status = Status.chargeing;
        }
        else if (colorTimer > 0f)
        {
            colorTimer -= Time.deltaTime / chargeTime;
        }

        if (colorTimer >= 0)
        {
            color = Color.Lerp(defaultMaterial.color, laserRed.color, colorTimer);
            barrelRenderer.material.color = color;
        }
    }
    
    private void aim()
    {
        float step = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, transform.position - enemy.transform.position, step, 0.0f);
        newDirection.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireDelay;
            Instantiate(laser, bulletSpawnpos.position, bulletSpawnpos.rotation);
        }
    }
}
