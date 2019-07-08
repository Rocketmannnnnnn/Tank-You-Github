using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarArm : MonoBehaviour
{
    private string teamTag;
    private Vector3 enemyPosition;
    private float hitTime;
    private float circleTime;
    private bool enemyHit;

    [SerializeField]
    private float rotateSpeed = 1;

    private void Start()
    {
        teamTag = GetComponentInParent<TankTag>().getTeamTag();
        circleTime = rotateSpeed / 10;
        enemyHit = false;
    }

    private void Update()
    {
        transform.Rotate(0, Time.deltaTime * rotateSpeed, 0);

        /*
        if(Time.time <= 2.5f)
        {
            transform.localScale = new Vector3(1, 1, Time.time);
        }
        */
    }

    private void OnTriggerEnter(Collider other)
    {
        //No target yet, found object is a tank
        if(other.gameObject.CompareTag("Tank"))
        {
            //The target is not in this team
            if (!teamTag.Equals(other.gameObject.GetComponent<TankTag>().getTeamTag()))
            {
                enemyPosition = other.gameObject.transform.position;
                hitTime = Time.time;
                enemyHit = true;
            }
        }
    }

    //Is an enemy hit in the last scan
    public bool hitLastScan()
    {
        return hitTime + circleTime > Time.time && enemyHit;
    }

    public Vector3 getEnemy()
    {
        return enemyPosition;
    }
}
