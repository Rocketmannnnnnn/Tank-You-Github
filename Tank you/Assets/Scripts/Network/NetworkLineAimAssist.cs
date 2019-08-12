using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NetworkLineAimAssist : NetworkBehaviour
{
    private int floorMask;
    private LineRenderer line;

    [SerializeField]
    private GameObject lineEndObject;

    [SerializeField]
    private Transform lineStartPos;

    [SerializeField]
    private float lineRange = 40;

    void Start()
    {
        line = GetComponent<LineRenderer>();

        if (!isLocalPlayer)
        {
            line.enabled = false;
            enabled = false;
        } else
        {
            line.startColor = GetComponent<SetupLocalPlayer>().playerColor;
            floorMask = 1 << 11;
            lineEndObject.transform.position = transform.root.position;
        }
    }

    void Update()
    {
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit groundHit;

        if (Physics.Raycast(camRay, out groundHit, 200, floorMask))
        {
            Vector3 bulletPos = lineStartPos.position;
            line.SetPosition(0, bulletPos);
            Vector3 hitpos = groundHit.point;
            hitpos.y = lineStartPos.position.y;

            if (Vector3.Distance(bulletPos, hitpos) > lineRange)
            {
                hitpos = bulletPos + (hitpos - bulletPos).normalized * lineRange;
            }

            line.SetPosition(1, hitpos);
            lineEndObject.transform.position = hitpos;

            //Debug.Log(groundHit.transform.gameObject.name);
        }
    }
}
