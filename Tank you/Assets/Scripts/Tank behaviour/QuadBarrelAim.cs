using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuadBarrelAim : MonoBehaviour
{
    private GameObject enemy;
    private float nextFire;
    private TankManagerV2 tankManager;
    private int frontBarrelNumber = 0;
    private int backBarrelNumber = 0;

    [SerializeField]
    private GameObject frontBarrel;

    [SerializeField]
    private List<Transform> frontSpawnPoints = new List<Transform>();

    [SerializeField]
    private GameObject backBarrel;

    [SerializeField]
    private List<Transform> backSpawnPoints = new List<Transform>();

    [SerializeField]
    private float fireDelay = 1f;

    [SerializeField]
    private float rotateSpeed = 1;

    [SerializeField]
    private GameObject smartBullet;

    private void Start()
    {
        nextFire = 0;
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
    }

    private void Update()
    {
        enemy = tankManager.getClosestTargetInSight(gameObject);

        if (tankManager.multipleTeams() && enemy != null)
        {
            if(tankManager.isEnemyInSight(gameObject, enemy))
            {
                if (Vector3.Distance(transform.position - transform.forward, enemy.transform.position) <=
                Vector3.Distance(transform.position + transform.forward, enemy.transform.position))
                {
                    forwardAim();

                    RaycastHit objectHit;
                    Ray barrelDirection = new Ray(transform.position, -transform.forward);

                    if (Physics.Raycast(barrelDirection, out objectHit))
                    {
                        if (objectHit.transform.gameObject == enemy)
                        {
                            fireFront();
                        }
                    }
                } else
                {
                    backAim();

                    RaycastHit objectHit;
                    Ray barrelDirection = new Ray(transform.position, transform.forward);

                    if (Physics.Raycast(barrelDirection, out objectHit))
                    {
                        if (objectHit.transform.gameObject == enemy)
                        {
                            fireBack();
                        }
                    }
                }
            }
        }
    }

    private void forwardAim()
    {
        float step = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(transform.forward, transform.position - enemy.transform.position, step, 0.0f);
        newDirection.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void backAim()
    {
        float step = rotateSpeed * Time.deltaTime;
        Vector3 newDirection = Vector3.RotateTowards(-transform.forward, transform.position - enemy.transform.position, step, 0.0f);
        newDirection.y = 0.0f;
        transform.rotation = Quaternion.LookRotation(newDirection);
    }

    private void fireFront()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireDelay;
            GameObject bulletInstance = Instantiate(smartBullet, frontSpawnPoints[frontBarrelNumber].position, frontSpawnPoints[frontBarrelNumber].rotation);
            bulletInstance.GetComponent<SmartBullet>().setTarget(enemy);
            frontBarrelNumber++;
            frontBarrelNumber %= 2;
        }
    }

    private void fireBack()
    {
        if (Time.time > nextFire)
        {
            nextFire = Time.time + fireDelay;
            GameObject bulletInstance = Instantiate(smartBullet, backSpawnPoints[backBarrelNumber].position, backSpawnPoints[backBarrelNumber].rotation);
            bulletInstance.GetComponent<SmartBullet>().setTarget(enemy);
            backBarrelNumber++;
            backBarrelNumber %= 2;
        }
    }
}
