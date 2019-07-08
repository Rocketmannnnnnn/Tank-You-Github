using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MPPlayerTankController : NetworkBehaviour
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
        floorMask = LayerMask.GetMask("MouseFloor");
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
                Cmdfire();
            }

            //If mine input, deploy mine
            if (Input.GetButtonDown("SecondaryFire") && Time.time > nextMine)
            {
                CmddeployMine();
            }
        }

        barrel.transform.position = transform.position + new Vector3(0, 0.75f, 0);
    }

    //Point barrel at mouse / croishair
    private void rotateBarrel()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit groundHit;

        if (Physics.Raycast(camRay, out groundHit, floorMask))
        {
            Vector3 barrelToMouse = groundHit.point - transform.position;
            barrelToMouse.y = 0f;
            barrelToMouse *= -1;

            Quaternion newRotation = Quaternion.LookRotation(barrelToMouse);
            barrelRigidbody.MoveRotation(newRotation);
        }
    }

    //Create and shoot bullet
    [Command]
    private void Cmdfire()
    {
        nextFire = Time.time + fireDelay;
        GameObject newBullet = Instantiate(bullet, bulletSpawnpos.position, bulletSpawnpos.rotation);
        NetworkServer.Spawn(newBullet);
    }

    //Create and deploy mine
    [Command]
    private void CmddeployMine()
    {
        nextMine = Time.time + mineDelay;
        GameObject mineInstance = Instantiate(mine, transform.position, transform.rotation);
        mineInstance.GetComponent<NetworkMine>().setParent(gameObject);
        NetworkServer.Spawn(mineInstance);
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
