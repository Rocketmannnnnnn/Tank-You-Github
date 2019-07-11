using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankStation : MonoBehaviour
{
    private Dictionary<GameObject, float> poweredUp;
    private List<GameObject> hadPowerup;

    [SerializeField]
    private float fireRateIncrease = 2f;

    [SerializeField]
    private float powerupDuration = 7f;

    [SerializeField]
    private AudioSource powerUp;

    [SerializeField]
    private AudioSource powerDown;

    public TankStation()
    {
        poweredUp = new Dictionary<GameObject, float>();
        hadPowerup = new List<GameObject>();
    }

    void Update()
    {
        foreach(GameObject tank in poweredUp.Keys)
        {
            if(Time.time >= poweredUp[tank])
            {
                if (tank.GetComponent<PlayerTankController>() != null)
                {
                    PlayerTankController ptc = tank.GetComponent<PlayerTankController>();
                    ptc.adaptFireDelay(ptc.getFireDelay() * fireRateIncrease);
                }
                else if (tank.GetComponent<KeyboardMultiplayerTankController>() != null)
                {
                    KeyboardMultiplayerTankController kmt = tank.GetComponent<KeyboardMultiplayerTankController>();
                    kmt.adaptFireDelay(kmt.getFireDelay() * fireRateIncrease);
                }
                powerDown.Play();
                poweredUp.Remove(tank);
                break;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Tank"))
        {
            GameObject tank = other.transform.root.gameObject;

            if (!hadPowerup.Contains(tank))
            {
                poweredUp.Add(tank, Time.time + powerupDuration);
                hadPowerup.Add(tank);

                if (tank.GetComponent<PlayerTankController>() != null)
                {
                    PlayerTankController ptc = tank.GetComponent<PlayerTankController>();
                    ptc.adaptFireDelay(ptc.getFireDelay() / fireRateIncrease);
                }
                else if (tank.GetComponent<KeyboardMultiplayerTankController>() != null)
                {
                    KeyboardMultiplayerTankController kmt = tank.GetComponent<KeyboardMultiplayerTankController>();
                    kmt.adaptFireDelay(kmt.getFireDelay() / fireRateIncrease);
                }
                powerUp.Play();
            }
        }
    }
}
