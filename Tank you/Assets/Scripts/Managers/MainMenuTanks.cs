using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTanks : MonoBehaviour
{
    private float t1SpawnTime = 0;
    private float t2SpawnTime = 0;
    private GameObject tank1;
    private GameObject tank2;

    [SerializeField]
    private GameObject team1Tank;

    [SerializeField]
    private GameObject team2Tank;

    [SerializeField]
    private Transform team1Spawn;

    [SerializeField]
    private Transform team2Spawn;

    [SerializeField]
    private float spawnDelay = 5;
    
    private void Update()
    {
        if(tank1 == null && Time.time > t1SpawnTime)
        {
            tank1 = Instantiate(team1Tank, team1Spawn.position, team1Spawn.rotation);
            tank1.GetComponent<TankTag>().setTeamTag("team1");
            t1SpawnTime = Time.time + spawnDelay;
        } else if(tank1 != null)
        {
            t1SpawnTime = Time.time + spawnDelay;
        }

        if (tank2 == null && Time.time > t2SpawnTime)
        {
            tank2 = Instantiate(team2Tank, team2Spawn.position, team2Spawn.rotation);
            tank2.GetComponent<TankTag>().setTeamTag("team2");
            t2SpawnTime = Time.time + spawnDelay;
        } else if (tank2 != null)
        {
            t2SpawnTime = Time.time + spawnDelay;
        }

        if(tank1 == null && tank2 == null)
        {
            t1SpawnTime = Time.time;
            t2SpawnTime = Time.time;
        }
    }
}
