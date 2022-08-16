using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class PlayerData
{
    public string playerName;
    public int kills;
    public bool hasData;

    public PlayerData(string name, bool hasData, int kills)
    {
        playerName = name;
        this.hasData = hasData;
        this.kills = kills; 
    }
}
