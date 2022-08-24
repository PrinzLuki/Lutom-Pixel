using System;
using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerProfile
{
    public string playerName;
    public string playerid;
    public int kills;
    public int deaths;

    public int matchesPlayed;
    public int matchesWon;
    public int matchesLost;

    public bool volumeEdited;
    public int masterVolume;
    public int musicVolume;
    public int effectsVolume;
}
