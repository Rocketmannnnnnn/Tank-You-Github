using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private float spawnTime;

    [SerializeField]
    private Transform spawnPos;

    [SerializeField]
    private GameObject tank;

    [SerializeField]
    private float spawnDelay;

    [SerializeField]
    private AudioSource sound;

    void Start()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        if(Time.time >= spawnTime)
        {
            Instantiate(tank, spawnPos.position, spawnPos.rotation);
            sound.Play();
            spawnTime = Time.time + spawnDelay;
        }
    }
}
