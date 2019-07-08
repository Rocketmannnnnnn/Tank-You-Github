using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveSounds : MonoBehaviour
{
    private TankVelocity tankVelocity;

    [SerializeField]
    private AudioSource drive;

    [SerializeField]
    private AudioSource idle;

    void Start()
    {
        tankVelocity = GetComponentInParent<TankVelocity>();
    }

    void Update()
    {
        if (tankVelocity.getVelocity().magnitude > 0)
        {
            drive.mute = false;
            idle.mute = true;
        }
        else
        {
            drive.mute = true;
            idle.mute = false;
        }
    }
}
