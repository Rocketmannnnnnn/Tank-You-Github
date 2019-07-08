using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvicibleAim : MonoBehaviour
{
    private Animator anim;
    private bool visible;

    private float fadeMoment;

    [SerializeField]
    private float animLenght = 1.5f;

    [SerializeField]
    private float visibleTime;

    [SerializeField]
    private float invisibleTime;

    private void Start()
    {
        anim = GetComponent<Animator>();
        visible = false;
        fadeMoment = Time.time + invisibleTime;
    }

    private void Update()
    {
        if(Time.time > fadeMoment && !visible)
        {
            visible = true;
            fadeMoment = Time.time + animLenght + visibleTime;
        } else if (Time.time > fadeMoment && visible)
        {
            visible = false;
            fadeMoment = Time.time + animLenght + invisibleTime;
        }

        anim.SetBool("visible", visible);
    }
}
