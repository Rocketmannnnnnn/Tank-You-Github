using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TpObjectTimer
{
    public GameObject toTeleport;
    public float enterTime;

    public TpObjectTimer (GameObject toTeleport, float enterTime)
    {
        this.toTeleport = toTeleport;
        this.enterTime = enterTime;
    }
}
