using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    [HideInInspector] public AudioSource Source;
    public AudioClip Clip;
    [Range(0f, 1f)] public float Volume = 1;
    [Range(-3f, 3f)]public float Pitch = 1;
    public bool Loop = false;
    public bool PlayOnAwake = false;
}
