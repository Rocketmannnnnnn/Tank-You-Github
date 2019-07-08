using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Farter : MonoBehaviour
{
    private float fartMoment = 0;

    [SerializeField]
    private float fartDelay = 6;

    [SerializeField]
    private GameObject fart;

    void Update()
    {
        if (Time.time >= fartMoment)
        {
            fartMoment = Time.time + fartDelay;
            GameObject fartInstance = Instantiate(fart, transform.position, transform.rotation);
            fartInstance.GetComponent<Fart>().setParent(gameObject);
        }
    }
}
