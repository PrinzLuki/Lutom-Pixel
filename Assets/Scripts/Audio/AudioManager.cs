using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Mirror;

public class AudioManager : NetworkBehaviour
{
    public static AudioManager instance;

    public AudioMixer audioMixer;
    public AudioSource audioSource;

    public AudioClip[] songs;
    [SerializeField] private int currentClip;

    private const string masterVolName = "Master";
    private const string musicVolName = "Music";
    private const string effectsVolName = "Effects";

    public AudioMixerGroup masterGroup;
    public AudioMixerGroup musicGroup;
    public AudioMixerGroup effectsGroup;

    public bool otherSongPlaying;


    public Sound[] sounds;

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }


        OnLoad();
        GetVolumeOnStart(SaveData.PlayerProfile.masterVolume, masterVolName);
        GetVolumeOnStart(SaveData.PlayerProfile.musicVolume, musicVolName);
        GetVolumeOnStart(SaveData.PlayerProfile.effectsVolume, effectsVolName);

        currentClip = UnityEngine.Random.Range(0, songs.Length);

        audioSource.clip = songs[currentClip];
        audioSource.Play();

        #region DirectlyOnAudioManager
        //foreach (Sound s in sounds)
        //{
        //    s.source = gameObject.AddComponent<AudioSource>();
        //    switch (s.type)
        //    {
        //        case AudioType.Master:
        //            s.source.outputAudioMixerGroup = masterGroup;
        //            break;
        //        case AudioType.Music:
        //            s.source.outputAudioMixerGroup = musicGroup;
        //            break;
        //        case AudioType.Effects:
        //            s.source.outputAudioMixerGroup = effectsGroup;
        //            break;
        //        default:
        //            break;
        //    }
        //    s.source.clip = s.clip;
        //    s.source.volume = s.volume;
        //    s.source.pitch = s.pitch;
        //    s.source.loop = s.loop;
        //}

        #endregion
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

    public void Play(string sound)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        s.source.Play();
    }

    public void PlayOnObject(string sound, GameObject obj)
    {
        Sound s = Array.Find(sounds, item => item.name == sound);
        AudioSource objectAudioSource = GetAudioSource(s, obj);

        switch (s.type)
        {
            case AudioType.Master:
                objectAudioSource.outputAudioMixerGroup = masterGroup;
                break;
            case AudioType.Music:
                objectAudioSource.outputAudioMixerGroup = musicGroup;
                break;
            case AudioType.Effects:
                objectAudioSource.outputAudioMixerGroup = effectsGroup;
                break;
            default:
                break;
        }
        objectAudioSource.clip = s.clip;
        objectAudioSource.volume = s.volume;
        objectAudioSource.pitch = s.pitch;
        objectAudioSource.loop = s.loop;
        objectAudioSource.spatialBlend = s.spatialBand;
        objectAudioSource.Play();
    }

    //[Command(requiresAuthority = false)]
    //public void CmdPlayOnObject(string sound, GameObject obj)
    //{
    //    RpcPlayOnObject(sound, obj);
    //}

    //[ClientRpc]
    //public void RpcPlayOnObject(string sound, GameObject obj)
    //{
    //    Sound s = Array.Find(sounds, item => item.name == sound);
    //    AudioSource objectAudioSource = Array.Find(obj.GetComponents<AudioSource>(), item => item.clip == s.clip);

    //    switch (s.type)
    //    {
    //        case AudioType.Master:
    //            objectAudioSource.outputAudioMixerGroup = masterGroup;
    //            break;
    //        case AudioType.Music:
    //            objectAudioSource.outputAudioMixerGroup = musicGroup;
    //            break;
    //        case AudioType.Effects:
    //            objectAudioSource.outputAudioMixerGroup = effectsGroup;
    //            break;
    //        default:
    //            break;
    //    }
    //    objectAudioSource.clip = s.clip;
    //    objectAudioSource.volume = s.volume;
    //    objectAudioSource.pitch = s.pitch;
    //    objectAudioSource.loop = s.loop;
    //    objectAudioSource.Play();
    //}


    public AudioSource GetAudioSource(Sound s, GameObject obj)
    {
        if (obj.GetComponents<AudioSource>().Length <= 0)
        {
            AudioSource objectAudioSource = obj.AddComponent<AudioSource>();
            return objectAudioSource;
        }
        else
        {
            AudioSource[] audioSourcesOnObject = obj.GetComponents<AudioSource>();
            AudioSource auds = Array.Find(audioSourcesOnObject, item => item.clip == s.clip);
            if (auds == null)
            {
                //Debug.Log("No AudioSource w that clip found - Creating one");
                AudioSource objectAudioSource = obj.AddComponent<AudioSource>();
                return objectAudioSource;
            }
            else
            {
                //Debug.Log("AudioSource found - using this one");
                AudioSource objectAudioSource = auds;
                return objectAudioSource;
            }

        }
    }


    public string GetSongName()
    {
        return audioSource.clip.name;
    }

    public string ChangeToNextSong()
    {
        currentClip++;
        if (currentClip > songs.Length - 1)
        {
            currentClip = 0;
        }
        audioSource.clip = songs[currentClip];
        audioSource.Play();

        return GetSongName();
    }

    public string ChangeToPrevSong()
    {
        currentClip--;
        if (currentClip < 0)
        {
            currentClip = songs.Length - 1;
        }
        audioSource.clip = songs[currentClip];
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
