using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artillery : MonoBehaviour
{
    private float fireTime = 0f;

    [SerializeField]
    private float startFireingAfter = 0;

    [SerializeField]
    private float fireDelay = 5f;

    [SerializeField]
    private float force = 10f;

    [SerializeField]
    private GameObject bomb;

    [SerializeField]
    private Transform bombSpawnPos;

    [SerializeField]
    private AudioSource shot;

    private void Start()
    {
        fireTime = Time.time + startFireingAfter;
    }

    private void OnEnable()
    {
        fireTime = Time.time + startFireingAfter;
    }

    private void Update()
    {
        if(Time.time > fireTime)
        {
            GameObject newBomb = Instantiate(bomb, bombSpawnPos.position, bombSpawnPos.rotation);
            newBomb.GetComponent<Rigidbody>().AddForce(bombSpawnPos.up * force, ForceMode.Impulse);
            fireTime = Time.time + fireDelay;
            shot.Play();
        }
    }
}
