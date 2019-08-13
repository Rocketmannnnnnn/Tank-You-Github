using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using Prototype.NetworkLobby;

public class TankActivionSetter : NetworkBehaviour
{
    [SerializeField]
    private List<GameObject> children;

    public bool isActive = true;

    public TankActivionSetter()
    {
        children = new List<GameObject>();
    }

    public void SetAllActive(bool active)
    {
        setActive(active);
        RpcSetActive(active);
    }

    [ClientRpc]
    private void RpcSetActive(bool active)
    {
        setActive(active);
    }

    private void setActive(bool active)
    {
        isActive = active;
        foreach (MonoBehaviour monoBehaviour in GetComponents<MonoBehaviour>())
        {
            if (monoBehaviour.GetType() == typeof(MPPlayerTankController) || monoBehaviour.GetType() == typeof(NetworkLineAimAssist))
            {
                if (isLocalPlayer)
                {
                    monoBehaviour.enabled = active;
                }
            } else if (!monoBehaviour.Equals(this))
            {
                monoBehaviour.enabled = active;
            }
        }
        
        foreach(GameObject child in children)
        {
            child.SetActive(active);
        }

        if (isLocalPlayer)
        {
            GetComponent<LineRenderer>().enabled = active;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
    }

    private void OnLevelLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        if (isLocalPlayer)
        {
            transform.position = GameObject.Find("LobbyManager").GetComponent<LobbyManager>().startPositions[GetComponent<SetupLocalPlayer>().playerNumber].position;
        }
        setActive(true);
    }
}
