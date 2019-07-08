using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartBullet : MonoBehaviour
{
    private int bounces;
    private GameObject enemy;
    private TankManagerV2 tankManager;

    [SerializeField]
    private GameObject bulletExplosion;

    [SerializeField]
    private float speed = 0.75f;

    [SerializeField]
    private float rotateSpeed = 1f;

    private void Start()
    {
        speed *= 20f;
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
    }

    private void FixedUpdate()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

        if(enemy != null)
        {
            if (tankManager.isEnemyInSight(gameObject, enemy) != null)
            {
                float step = rotateSpeed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, enemy.transform.position - transform.position, step, 0.0f);
                newDirection.y = 0.0f;
                transform.rotation = Quaternion.LookRotation(newDirection);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Contains("Tank"))
        {
            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Bullet"))
        {
            Destroy(gameObject);
            Instantiate(bulletExplosion, transform.position, transform.rotation);
        } else
        {
            if (!collision.gameObject.name.Equals("MouseFloor"))
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.gameObject.name.Equals("FlameThrower"))
        {
            Destroy(gameObject);
        }
    }

    public void setTarget(GameObject target)
    {
        enemy = target;
    }
}
