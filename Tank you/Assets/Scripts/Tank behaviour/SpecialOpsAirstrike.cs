using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialOpsAirstrike : MonoBehaviour
{
    private TankManagerV2 tankManager;
    private float strikeTime;
    private GameObject enemy;
    private ParticleSystem ps;

    [SerializeField]
    private GameObject particleObject;

    [SerializeField]
    private float strikeDelay = 20;

    [SerializeField]
    private GameObject airstrike;

    private void Start()
    {
        tankManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<TankManagerV2>();
        ps = particleObject.GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(enemy == null)
        {
            enemy = tankManager.getClosestTargetInSight(gameObject);
        }

        if(enemy != null)
        {
            if(Time.time > strikeTime)
            {
                ps.Play();
                GameObject instantiatedAirstrike = Instantiate(airstrike);
                instantiatedAirstrike.GetComponent<Airstrike>().setTarget(enemy);
                strikeTime = Time.time + strikeDelay;
            }
        }
    }
}
