using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasStation : MonoBehaviour
{
    List<GameObject> poweredUpTanks;
    List<GameObject> noPowerupAllowed;
    Dictionary<GameObject, float> dictionairy;

    [SerializeField]
    private float fireRateIncrease = 1.5f;

    [SerializeField]
    private float powerupDuration = 7f;

    [SerializeField]
    private AudioSource powerUp;

    [SerializeField]
    private AudioSource powerDown;

    public GasStation()
    {
        poweredUpTanks = new List<GameObject>();
        noPowerupAllowed = new List<GameObject>();
        dictionairy = new Dictionary<GameObject, float>();
    }

    private void Update()
    {
        foreach(GameObject go in poweredUpTanks)
        {
            if (!noPowerupAllowed.Contains(go))
            {
                dictionairy[go] = dictionairy[go] - Time.deltaTime;
                if (dictionairy[go] <= 0)
                {
                    if (go.GetComponent<PlayerTankController>() != null)
                    {
                        PlayerTankController ptc = go.GetComponent<PlayerTankController>();
                        ptc.adaptFireDelay(ptc.getFireDelay() * fireRateIncrease);
                    }
                    else if (go.GetComponent<KeyboardMultiplayerTankController>() != null)
                    {
                        KeyboardMultiplayerTankController kmt = go.GetComponent<KeyboardMultiplayerTankController>();
                        kmt.adaptFireDelay(kmt.getFireDelay() * fireRateIncrease);
                    }
                    powerDown.Play();
                    noPowerupAllowed.Add(go);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Tank"))
        {
            if (other.transform.root.GetComponent<TankTag>().IsPlayer())
            {
                if (!poweredUpTanks.Contains(other.gameObject))
                {
                    if(other.transform.root.GetComponent<PlayerTankController>() != null)
                    {
                        PlayerTankController ptc = other.transform.root.GetComponent<PlayerTankController>();
                        ptc.adaptFireDelay(ptc.getFireDelay() / fireRateIncrease);
                    } else if (other.transform.root.GetComponent<KeyboardMultiplayerTankController>() != null)
                    {
                        KeyboardMultiplayerTankController kmt = other.transform.root.GetComponent<KeyboardMultiplayerTankController>();
                        kmt.adaptFireDelay(kmt.getFireDelay() / fireRateIncrease);
                    }
                    poweredUpTanks.Add(other.gameObject);
                    dictionairy.Add(other.gameObject, powerupDuration);
                    powerUp.Play();
                }
            }
        }
    }
}
