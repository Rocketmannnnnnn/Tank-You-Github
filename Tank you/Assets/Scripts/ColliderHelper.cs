using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHelper : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.transform.root.gameObject.CompareTag("Tank"))
        {
            if(collision.contacts[0].normal.y == 0)
            {
                GameObject tank = collision.collider.transform.root.gameObject;
                tank.transform.position -= collision.contacts[0].normal * 0.5f;
                //Debug.Log(collision.contacts[0].normal);
            }
        }
    }
}
