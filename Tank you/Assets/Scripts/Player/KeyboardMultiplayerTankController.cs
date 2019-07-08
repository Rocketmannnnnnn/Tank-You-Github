using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardMultiplayerTankController : MonoBehaviour
{
    private float nextFire;

    [SerializeField]
    private string verticalAxis;

    [SerializeField]
    private string horizontalAxis;

    [SerializeField]
    private string fireButton;

    [SerializeField]
    private float fireDelay = 0.75f;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private List<Transform> bulletSpawnPositions = new List<Transform>();

    [SerializeField]
    private List<GameObject> tankParts = new List<GameObject>();

    [SerializeField]
    private float tankMovementSpeed = 2f;


    void Start()
    {
        nextFire = 0f;
        tankMovementSpeed = tankMovementSpeed / 10;
    }

    void FixedUpdate()
    {
        if (Input.GetAxis(verticalAxis) != 0 || Input.GetAxis(horizontalAxis) != 0)
        {
            Vector3 direction = new Vector3(Input.GetAxis(verticalAxis), 0, -Input.GetAxis(horizontalAxis)).normalized;

            direction += direction * Time.deltaTime;
            transform.Translate(direction * tankMovementSpeed);

            foreach (GameObject part in tankParts)
            {
                part.transform.LookAt(part.transform.position - direction);
            }
        }
    }

    private void Update()
    {
        if (Input.GetAxis(fireButton) > 0)
        {
            fire();
        }
    }

    //Create and shoot bullet
    private void fire()
    {
        if(Time.time > nextFire)
        {
            nextFire = Time.time + fireDelay;
            foreach (Transform t in bulletSpawnPositions)
                Instantiate(bullet, t.position, t.rotation);
        }
    }

    public void adaptFireDelay(float delay)
    {
        fireDelay = delay;
    }

    public float getFireDelay()
    {
        return fireDelay;
    }
}
