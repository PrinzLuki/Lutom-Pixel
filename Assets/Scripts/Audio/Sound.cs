using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{

    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(-3f, 3f)]
    public float pitch;

    public bool loop = false;
    public float spatialBand;

    public AudioType type;

    [HideInInspector]
    public AudioSource source;


}