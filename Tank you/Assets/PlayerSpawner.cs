using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerSpawner : NetworkManager
{
    private List<NetworkConnection> players;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        players = new List<NetworkConnection>();
    }

    void Update()
    {
        if (NetworkServer.connections.Count > 0)
        {
            for(int i = 0; i < NetworkServer.connections.Count; i++)
            {
                spawnPlayers(i);
            }
        }
    }

    void spawnPlayers(int playerNumber)
    {
        if (players.Contains(NetworkServer.connections[playerNumber]))
        {
            return;
        }
        
        if (NetworkServer.connections[playerNumber].isReady)
        {
            GameObject newPlayer = Instantiate(playerPrefab);
            NetworkServer.SpawnWithClientAuthority(newPlayer, NetworkServer.connections[playerNumber]);
            players.Add(NetworkServer.connections[playerNumber]);
        }
    }
}
