using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAimV3 : MonoBehaviour
{
    private GameObject enemy;
    private float nextFire;
    private TankManagerV2 tankManager;

    [SerializeField]
    private float rotateSpeed = 1;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private Transform bulletSpawnpos;

    [SerializeField]
    private float fireDelay = 1f;

    private void Start()
    {
        nextFire = 0;
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
    }

    private void Update()
    {
        if(enemy == null)
        {
            enemy = tankManager.getClosestTargetInSight(gameObject);
        }

        //Checks if there are multiple teams
        if (tankManager.multipleTeams() && enemy != null)
        {
            //Get playerDirection
            RaycastHit hit;
            Ray ray = new Ray(transform.position, enemy.transform.position - transform.position);
            Debug.DrawLine(transform.position, enemy.transform.position, Color.red);

            //If raycast hits
            if (Physics.Raycast(ray, out hit))
            {
                //If raycast hits player
                if (hit.transform.position == enemy.transform.position)
                {
                    aim();
                    
                    //If the direction of the barrel hits the player, fire
                    RaycastHit objectHit;
                    Ray barrelDirection = new Ray(transform.position, -transform.forward);

                    if (Physics.Raycast(barrelDirection, out objectHit))
                    {
                        if(objectHit.transform.gameObject == enemy)
                        {
                            fire();
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
