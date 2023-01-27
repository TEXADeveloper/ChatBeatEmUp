using UnityEngine;
using UnityEngine.Audio;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField, Range(0, .5f)] private float pitchRange;
    [SerializeField] private Sound[] sounds;

    void Start()
    {
        foreach (Sound s in sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.outputAudioMixerGroup = audioMixerGroup;
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.loop = s.Loop;
        }
    }

    public void PlaySound(string name)
    {
        foreach (Sound s in sounds)
            if (s.name == name)
            {
                if (s.Source.isPlaying)
                    return;
                s.Source.pitch = Random.Range(s.Pitch - pitchRange, s.Pitch + pitchRange);
                s.Source.Play();
            }
    }

    public void StopSound(string name)
    {
        foreach (Sound s in sounds)
            if (s.name == name)
            {
                if (!s.Source.isPlaying)
                    return;
                s.Source.Stop();
            }
    }
}
