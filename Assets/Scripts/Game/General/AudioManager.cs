using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    private const int intensifierAmount = 10;
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField,Range(0f, .5f)] float timeRange;
    [SerializeField] private Sound[] sounds;
    private int enemyAmount = 0;

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
            if (s.PlayOnAwake)
                s.Source.Play();
        }
    }

    public void UpdateEnemyAmount(int value)
    {
        enemyAmount = value;
    }

    public IEnumerator EnemySpawned()
    {
        switch(enemyAmount)
        {
            case (>= 1) and (< intensifierAmount):
                yield return StartCoroutine(PlayThisLoop("Intro", "SFX"));
                yield return StartCoroutine(PlayAtTheEndOfLoop("Intro", "Loop"));
                StartCoroutine(StopAtTheEndOfLoop("Intro"));
                break;
            case (>= intensifierAmount):
                yield return StartCoroutine(PlayThisLoop("Intro", "SFX"));
                yield return StartCoroutine(PlayAtTheEndOfLoop("Intro", "Loop"));
                StartCoroutine(StopAtTheEndOfLoop("Intro"));
                StartCoroutine(PlayAtTheEndOfLoop("Loop", "Intensity"));
                break;
        }
    }

    public void EnemyDied()
    {
        switch(enemyAmount)
        {
            case 0:
                StopAllCoroutines();
                StopSound("SFX");
                StartCoroutine(PlayAtTheEndOfLoop("Loop", "Intro"));
                StartCoroutine(StopAtTheEndOfLoop("Loop"));
                StartCoroutine(StopAtTheEndOfLoop("Intensity"));
                break;
            case (< intensifierAmount) and (> 0):
                StartCoroutine(StopAtTheEndOfLoop("Intensity"));
                break;
        }
    }

    private Sound getSound(string name)
    {
        foreach (Sound s in sounds)
            if (s.name == name)
                return s;
        return null;
    }

    public IEnumerator PlayThisLoop(string playing, string toPlay)
    {
        Sound sPlaying = getSound(playing);
        Sound sToPlay = getSound(toPlay);

        if (sPlaying == null || sToPlay == null || !sPlaying.Source.isPlaying || sToPlay.Source.isPlaying)
            yield break;
        
        yield return new WaitUntil(() => (sPlaying.Source.time < sPlaying.Clip.length * 3/4));
        sToPlay.Source.time = sPlaying.Source.time;
        sToPlay.Source.Play();
    }

    public IEnumerator PlayAtTheEndOfLoop(string playing, string toPlay)
    {
        Sound s = getSound(playing);
        if (s == null || !s.Source.isPlaying)
            yield break;

        yield return new WaitUntil(() => ((s.Source.time >= s.Clip.length - timeRange && s.Source.time <= s.Clip.length)));
        PlaySound(toPlay);
    }

    public IEnumerator StopAtTheEndOfLoop(string playing)
    {
        Sound s = getSound(playing);
        if (s == null || !s.Source.isPlaying)
            yield break;

        yield return new WaitUntil(() => (s.Source.time >= 0 && s.Source.time <= timeRange));
        StopSound(playing);
    }

    public void PlaySound(string name)
    {
        Sound s = getSound(name);

        if (s != null && !s.Source.isPlaying)
            s.Source.Play();
    }

    public void StopSound(string name)
    {
        Sound s = getSound(name);
        if (s != null && s.Source.isPlaying)
            s.Source.Stop();
    }
}
