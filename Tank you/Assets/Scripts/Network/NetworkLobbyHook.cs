using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prototype.NetworkLobby;
using UnityEngine.Networking;

public class NetworkLobbyHook : LobbyHook
{
    private List<GameObject> players;

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        SetupLocalPlayer localPlayer = gamePlayer.GetComponent<SetupLocalPlayer>();
        
        localPlayer.playerColor = lobby.playerColor;
        localPlayer.playerName = lobby.playerName;
    }

    public void initPlayerList()
    {
        players = new List<GameObject>();
        
        foreach(GameObject tank in GameObject.FindGameObjectsWithTag("Tank"))
        {
            players.Add(tank);
        }
    }

    public List<GameObject> getPlayerList()
    {
        return players;
    }
}