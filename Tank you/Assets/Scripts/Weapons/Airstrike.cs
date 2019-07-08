using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airstrike : MonoBehaviour
{
    private GameObject target;

    private bool dropped = false;

    [SerializeField]
    private float rotateSpeed = 360;

    [SerializeField]
    private float dropDistance = 5f;

    [SerializeField]
    private GameObject bomb;

    [SerializeField]
    private float speed = 20f;

    private void FixedUpdate()
    {
        if (!dropped)
        {
            if(target != null)
            {
                float step = rotateSpeed * Time.deltaTime;
                Vector3 newDirection = Vector3.RotateTowards(transform.forward, target.transform.position - transform.position, step, 0.0f);
                newDirection.y = 0.0f;
                transform.rotation = Quaternion.LookRotation(newDirection);

                if (Mathf.Sqrt(Mathf.Pow(target.transform.position.x - transform.position.x, 2) +
                              Mathf.Pow(target.transform.position.z - transform.position.z, 2)) < dropDistance)
                {
                    drop();
                }
            } else
            {
                drop();
            } 
        } else
        {
            Destroy(gameObject, 10);
        }
        transform.Translate(transform.forward * speed * Time.deltaTime);
    }

    public void drop()
    {
        Instantiate(bomb, transform.position, transform.rotation);
        dropped = true;
    }

    public void setTarget(GameObject target)
    {
        this.target = target;
    }
}
