using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmLight : MonoBehaviour
{
    private TankManagerV2 tankManager;
    private GameObject enemy;

    [SerializeField]
    private GameObject parent;

    [SerializeField]
    private List<GameObject> lights = new List<GameObject>();

    [SerializeField]
    private float degreesPerSec;

    private void Start()
    {
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
    }

    private void Update()
    {
        if (enemy == null)
        {
            LAMP(false);
            enemy = tankManager.getClosestTargetInSight(parent);
        }
        else if (tankManager.isEnemyInSight(parent, enemy))
        {
            LAMP(true);
            transform.Rotate(Vector3.up, degreesPerSec * Time.deltaTime);
        }
        else
        {
            LAMP(false);
        }
    }

    private void LAMP(bool on)
    {
        foreach (GameObject light in lights)
        {
            light.SetActive(on);
        }
    }
}
