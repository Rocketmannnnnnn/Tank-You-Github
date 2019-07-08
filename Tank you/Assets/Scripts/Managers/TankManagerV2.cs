using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankManagerV2 : MonoBehaviour
{
    private ArrayList tankList;
    private ArrayList playerList;

    //Constructor to make sure the list exists
    public TankManagerV2()
    {
        tankList = new ArrayList();
        playerList = new ArrayList();
    }

    //Put all living tanks in tankList
    public void updateTanklist()
    {
        GameObject[] tanks = GameObject.FindGameObjectsWithTag("Tank");

        foreach(GameObject tank in tanks)
        {
            tankList.Add(tank);

            if (tank.GetComponent<TankTag>().IsPlayer())
            {
                playerList.Add(tank);
            }
        }
    }

    //Remove a dying tank from the list
    public void removeFromList(GameObject dieingTank)
    {
        if (tankList.Contains(dieingTank))
        {
            tankList.Remove(dieingTank);

            if (dieingTank.GetComponent<TankTag>().IsPlayer())
            {
                playerList.Remove(dieingTank);
            }
        }
    }

    //Get all given tanks enemy's
    public ArrayList getEnemyTanks(GameObject tank)
    {
        ArrayList tankEnemys = new ArrayList();
        string tanksTeam = tank.GetComponentInParent<TankTag>().getTeamTag();

        foreach (GameObject enemy in tankList)
        {
            string enemyTeam = enemy.GetComponent<TankTag>().getTeamTag();
            if (!tanksTeam.Equals(enemyTeam))
            {
                tankEnemys.Add(enemy);
            }
        }
        return tankEnemys;
    }

    //Return the closest tank in sight from the given tank
    public GameObject getClosestTargetInSight(GameObject tank)
    {
        ArrayList enemys = getEnemyTanks(tank);
        GameObject closestEnemy = null;

        foreach (GameObject enemy in enemys)
        {
            RaycastHit hit;
            Ray ray = new Ray(tank.transform.position, enemy.transform.position - tank.transform.position);

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform == enemy.transform)
                {
                    if(closestEnemy == null)
                    {
                        closestEnemy = enemy;
                    }
                    else if (Vector3.Distance(enemy.transform.position, tank.transform.position) < Vector3.Distance(closestEnemy.transform.position, tank.transform.position))
                    {
                        closestEnemy = enemy;
                    }
                }
            }
        }
        return closestEnemy;
    }

    public GameObject isEnemyInSight(GameObject self, GameObject enemy)
    {
        RaycastHit hit;
        Ray ray = new Ray(self.transform.position, enemy.transform.position - self.transform.position);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == enemy || hit.collider.transform.root.gameObject == enemy)
            {
                return enemy;
            }
        }
        return null;
    }

    public bool multipleTeams()
    {
        string teamTag = null;

        foreach(GameObject tank in tankList)
        {
            if(teamTag == null)
            {
                teamTag = tank.GetComponent<TankTag>().getTeamTag();
            }
            else if (!teamTag.Equals(tank.GetComponent<TankTag>().getTeamTag()))
            {
                return true;
            }
        }

        return false;
    }

    public bool isPlayerAlive()
    {
        return playerList.Count > 0;
    }

    public ArrayList getPlayerList()
    {
        return playerList;
    }

    public void addTank(GameObject tank)
    {
        if (!tankList.Contains(tank))
        {
            tankList.Add(tank);
        }
    }

    public void addPlayer(GameObject player)
    {
        if (!playerList.Contains(player))
        {
            playerList.Add(player);
        }
    }

    public List<GameObject> getAllTanks()
    {
        List<GameObject> tanks = new List<GameObject>();
        
        foreach(GameObject tank in tankList)
        {
            tanks.Add(tank);
        }
        return tanks;
    }
}
