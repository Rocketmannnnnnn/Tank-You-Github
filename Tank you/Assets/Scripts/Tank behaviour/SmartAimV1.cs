using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartAimV1 : MonoBehaviour
{
    private GameObject enemy;
    private float nextFire;
    private TankManagerV2 tankManager;
    private Vector3 newEnemyPos;

    [SerializeField]
    private float smartAimAfter = 25f;

    [SerializeField]
    private float rotateSpeed = 1;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform bulletSpawnpos;

    [SerializeField]
    private float fireDelay = 1f;

    [SerializeField]
    private float bulletSpeed = 1f;

    private void Start()
    {
        nextFire = 0;
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
        bulletSpeed *= 20f;
    }

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
                //If raycast hits player
                if (hit.transform.position == enemy.transform.position)
                {
                    if(Vector3.Distance(transform.position, enemy.transform.position) > smartAimAfter)
                    {
                        smartAim();

                        if (Vector3.Angle(-transform.forward, (newEnemyPos - transform.position).normalized) < 2)
                        {
                            fire();
                        }
                    } else
                    {
                        aim();

                        RaycastHit objectHit;
                        Ray barrelDirection = new Ray(transform.position, -transform.forward);

                        if (Physics.Raycast(barrelDirection, out objectHit))
                        {
                            if (objectHit.transform.gameObject == enemy)
                            {
                                fire();
                            }
                        }
                    }
                } else
                {
                    enemy = null;
                }
            }
        }
    }

    //Aim towards the player or its last known location
    private void smartAim()
    {
        float travelTime = Vector3.Distance(transform.position, enemy.transform.position) / bulletSpeed;
        Vector3 enemyVelocity = enemy.GetComponent<TankVelocity>().getVelocity();
        newEnemyPos = enemy.transform.position + travelTime * enemyVelocity;
        float step = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, transform.position - newEnemyPos, step, 0.0f);
        newDirection.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void aim()
    {
        float step = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, transform.position - enemy.transform.position, step, 0.0f);
        newDirection.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    //Create and shoot bullet
    private void fire()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireDelay;
            Instantiate(bullet, bulletSpawnpos.position, bulletSpawnpos.rotation);
        }
    }
}
