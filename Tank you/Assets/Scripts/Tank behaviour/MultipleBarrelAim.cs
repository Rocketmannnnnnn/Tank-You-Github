using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultipleBarrelAim : MonoBehaviour
{
    private GameObject enemy;
    private float nextFire;
    private TankManagerV2 tankManager;

    [SerializeField]
    private float rotateSpeed = 1;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private List<Transform> bulletSpawnPositions = new List<Transform>();

    [SerializeField]
    private float fireDelay = 1f;

    private void Start()
    {
        nextFire = 0;
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
    }

    private void Update()
    {
        if (enemy == null)
        {
            enemy = tankManager.getClosestTargetInSight(gameObject);
        }

        if (tankManager.multipleTeams() && enemy != null)
        {
            RaycastHit hit;
            Ray ray = new Ray(transform.position, enemy.transform.position - transform.position);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.position == enemy.transform.position)
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
                } else
                {
                    enemy = null;
                }
            }
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

            foreach(Transform t in bulletSpawnPositions)
            {
                Instantiate(bullet, t.position, t.rotation);
            }
        }
    }
}
