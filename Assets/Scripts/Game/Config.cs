using UnityEngine;

[CreateAssetMenu(fileName = "New Config")]
public class Config : ScriptableObject
{
    public int highscore = 0;
    public string channelName;
    public int[] configValues;
    public float volume = 0;
}
