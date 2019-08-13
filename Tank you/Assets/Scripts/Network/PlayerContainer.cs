using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerContainer
{
    List<PlayerData> players;

    public PlayerContainer()
    {
        players = new List<PlayerData>();
    }

    public void addPlayer(NetworkConnection conn, Color color, string name)
    {
        PlayerData p = new PlayerData();
        p.conn = conn;
        p.tankColor = color;
        p.name = name;
        players.Add(p);
    }

    public List<PlayerData> getPlayerData()
    {
        return players;
    }
}

public struct PlayerData
{
    public NetworkConnection conn;
    public Color tankColor;
    public string name;
}
