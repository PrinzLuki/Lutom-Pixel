using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.IO;

public class AudioSlider : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioMixerGroup mixerGroup;
    public Slider slider;

    public AudioType audioType;


    private void Start()
    {

        OnLoad();

        switch (audioType)
        {
            case AudioType.Master:
                slider.value = SaveData.PlayerProfile.masterVolume;
                break;
            case AudioType.Music:
                slider.value = SaveData.PlayerProfile.musicVolume;
                break;
            case AudioType.Effects:
                slider.value = SaveData.PlayerProfile.effectsVolume;
                break;
            default:
                break;
        }


        ChangeVolume();
    }


    public void ChangeVolume()
    {
        UpdateVolume((int)slider.value, mixerGroup);
    }

    public void IncreaseVolume()
    {
        slider.value += 10;
        ChangeVolume();
    }

    public void DecreaseVolume()
    {
        slider.value -= 10;
        ChangeVolume();
    }


    public void UpdateVolume(int volume, AudioMixerGroup mixerGroup)
    {
        audioMixer.SetFloat(mixerGroup.name, volume);
        OnVolumeChange();
    }

    public void OnVolumeChange()
    {
        switch (audioType)
        {
            case AudioType.Master:
                SaveData.PlayerProfile.masterVolume = (int)slider.value;
                break;
            case AudioType.Music:
                SaveData.PlayerProfile.musicVolume = (int)slider.value;
                break;
            case AudioType.Effects:
                SaveData.PlayerProfile.effectsVolume = (int)slider.value;
                break;
            default:
                break;
        }

        OnSave();

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

public enum AudioType
{
    Master,
    Music,
    Effects
}
