using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public AudioSource audioSource;

    public AudioClip[] clips;
    [SerializeField] private int currentClip;

    private const string masterVolName = "Master";
    private const string musicVolName = "Music";
    private const string effectsVolName = "Effects";

    public bool otherSongPlaying;

    private void Start()
    {
        OnLoad();
        GetVolumeOnStart(SaveData.PlayerProfile.masterVolume, masterVolName);
        GetVolumeOnStart(SaveData.PlayerProfile.musicVolume, musicVolName);
        GetVolumeOnStart(SaveData.PlayerProfile.effectsVolume, effectsVolName);

        currentClip = Random.Range(0, clips.Length);
        audioSource.clip = clips[currentClip];
        audioSource.Play();
    }

    private void Update()
    {
        if (audioSource.isPlaying) return;
        else
        {
            ChangeToNextSong();
            otherSongPlaying = true;
        }

    }

    public string GetSongName()
    {
        return audioSource.clip.name;
    }

    public string ChangeToNextSong()
    {
        currentClip++;
        if (currentClip > clips.Length - 1)
        {
            currentClip = 0;
        }
        audioSource.clip = clips[currentClip];
        audioSource.Play();

        return GetSongName();
    }

    public string ChangeToPrevSong()
    {
        currentClip--;
        if (currentClip < 0)
        {
            currentClip = clips.Length - 1;
        }
        audioSource.clip = clips[currentClip];
        audioSource.Play();

        return GetSongName();
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
