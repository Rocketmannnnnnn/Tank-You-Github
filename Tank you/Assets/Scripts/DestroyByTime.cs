using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    private float destroyMoment;

    [SerializeField]
    private float destroyAfter;

    private void Start()
    {
        destroyMoment = destroyAfter + Time.time;
    }

    private void Update()
    {
        if(Time.time > destroyMoment)
        {
            Destroy(gameObject);
        }
    }
}
