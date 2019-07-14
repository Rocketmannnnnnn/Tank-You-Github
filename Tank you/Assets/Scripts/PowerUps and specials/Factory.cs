using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour
{
    private float spawnTime;
    private TankManagerV2 tankManagerV2;

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
        tankManagerV2 = GameObject.FindWithTag("Managers").GetComponent<TankManagerV2>();
    }

    void Update()
    {
        if(Time.time >= spawnTime)
        {
            if (tankManagerV2.multipleTeams())
            {
                Instantiate(tank, spawnPos.position, spawnPos.rotation);
                sound.Play();
                spawnTime = Time.time + spawnDelay;
            }
        }
    }
}
