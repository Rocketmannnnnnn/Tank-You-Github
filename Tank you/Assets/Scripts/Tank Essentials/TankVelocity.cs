using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankVelocity : MonoBehaviour
{
    private Vector3 velocity;
    private Vector3 oldPosition;

    private void Start()
    {
        oldPosition = transform.position;
    }

    private void Update()
    {
        velocity = (transform.position - oldPosition) / Time.deltaTime;
        oldPosition = transform.position;
    }

    public Vector3 getVelocity()
    {
        return velocity;
    }
}
