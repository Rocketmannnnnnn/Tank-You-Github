using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTankController : MonoBehaviour
{
    private int floorMask;
    private float camRayLength = 100f;
    private Rigidbody barrelRigidbody;
    private float nextFire;
    private float nextMine;

    [SerializeField]
    private Transform bulletSpawnpos;

    [SerializeField]
    private GameObject bullet;

    [SerializeField]
    private GameObject mine;

    [SerializeField]
    private GameObject barrel;

    [SerializeField]
    private GameObject frame;

    [SerializeField]
    private GameObject tracks;

    [SerializeField]
    private float fireDelay = 0.5f;

    [SerializeField]
    private float tankMovementSpeed = 1f;

    [SerializeField]
    private float mineDelay = 5f;

    private void Awake()
    {
        //floorMask = LayerMask.GetMask("MouseFloor");
        floorMask = 1 << 11;
        barrelRigidbody = barrel.GetComponent<Rigidbody>();
        nextFire = 0f;
        nextMine = 0f;
        tankMovementSpeed = tankMovementSpeed / 10;
    }

    private void FixedUpdate()
    {
        //If movement input, move and rotate frame and tracks
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            Vector3 direction = new Vector3(Input.GetAxis("Vertical"), 0, -Input.GetAxis("Horizontal")).normalized;

            direction += direction * Time.deltaTime;
            transform.Translate(direction * tankMovementSpeed);

            frame.transform.LookAt(transform.position + direction);
            tracks.transform.LookAt(transform.position + direction);
        }

        rotateBarrel();

        if (Time.timeScale > 0)
        {
            //If fire input, shoot bullet
            if (Input.GetButtonDown("PrimaryFire") && Time.time > nextFire)
            {
                fire();
            }

            //If mine input, deploy mine
            if (Input.GetButtonDown("SecondaryFire") && Time.time > nextMine)
            {
                deployMine();
            }
        }

        barrel.transform.position = transform.position + new Vector3(0, 0.75f, 0);
    }

    //Point barrel at mouse / croishair
    private void rotateBarrel()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit groundHit;

        if (Physics.Raycast(camRay, out groundHit, 200, floorMask))
        {
            Vector3 barrelToMouse = groundHit.point - transform.position;
            barrelToMouse.y = 0f;
            barrelToMouse *= -1;

            Quaternion newRotation = Quaternion.LookRotation(barrelToMouse);
            barrelRigidbody.MoveRotation(newRotation);
        }
    }

    //Create and shoot bullet
    private void fire()
    {
        nextFire = Time.time + fireDelay;
        Instantiate(bullet, bulletSpawnpos.position, bulletSpawnpos.rotation);
    }

    //Create and deploy mine
    private void deployMine()
    {
        nextMine = Time.time + mineDelay;
        GameObject mineInstance = Instantiate(mine, transform.position, transform.rotation);
        mineInstance.GetComponent<MineV2>().setParent(gameObject);
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
