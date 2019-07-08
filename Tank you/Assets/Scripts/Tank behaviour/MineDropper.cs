using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MineDropper : MonoBehaviour
{
    private float nextDrop;
    GameObject instantiatedMine;

    [SerializeField]
    private float firstDropAfter = 2;

    [SerializeField]
    private float dropDelay;

    [SerializeField]
    private GameObject mine;

    private void Start()
    {
        nextDrop = Time.time + firstDropAfter;
    }

    private void Update()
    {
        if(Time.time > nextDrop)
        {
            drop();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject == instantiatedMine)
        {
            instantiatedMine.GetComponent<NavMeshObstacle>().enabled = true;
        }
    }

    private void drop()
    {
        nextDrop = Time.time + dropDelay;

        instantiatedMine = Instantiate(mine, transform.position, transform.rotation);
        if(instantiatedMine.GetComponent<MineV2>() == null)
        {
            Debug.Log("Well shit");
        }
        instantiatedMine.GetComponent<MineV2>().setParent(gameObject);
        instantiatedMine.GetComponent<NavMeshObstacle>().enabled = false;
    }
}
