using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;


    [SerializeField] private const string masterVolName = "Master";
    [SerializeField] private const string musicVolName = "Music";
    [SerializeField] private const string effectsVolName = "Effects";

    private void Start()
    {
        OnLoad();
        GetVolumeOnStart(SaveData.PlayerProfile.masterVolume, masterVolName);
        GetVolumeOnStart(SaveData.PlayerProfile.musicVolume, musicVolName);
        GetVolumeOnStart(SaveData.PlayerProfile.effectsVolume, effectsVolName);
    }

    void GetVolumeOnStart(int volume, string name)
    {
        audioMixer.SetFloat(name, volume);
    }

    /// <summary>
    /// Saves file (PlayerProfile) and everything that is in SaveData
    /// </summary>
    public void OnSave()
    {
        SerializationManager.Save("playerData", SaveData.PlayerProfile);
    }

    /// <summary>
    /// Gets the SaveData file 
    /// </summary>
    public void OnLoad()
    {
        SaveData.PlayerProfile = (PlayerProfile)SerializationManager.Load(Application.persistentDataPath + "/saves/playerData.lutompixel");
    }

}
