using UnityEngine;
using UnityEngine.Audio;

public class EnemySound : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField, Range(0, .5f)] private float malePitchRange;
    [SerializeField, Range(0, .5f)] private float femalePitchRange;
    [SerializeField] private Sound[] sounds;
    private EnemyAI eAI;

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
        eAI = this.GetComponent<EnemyAI>();
    }

    public void PlaySound(string name)
    {
        foreach (Sound s in sounds)
            if (s.name == name)
            {
                if (s.Source.isPlaying)
                    return;
                float pitchRange = ((name.StartsWith("Male"))? malePitchRange : femalePitchRange);
                s.Source.pitch = Random.Range(s.Pitch - pitchRange, s.Pitch + pitchRange);
                s.Source.Play();
            }
    }
}
