using UnityEngine;

[CreateAssetMenu(fileName = "New Config")]
public class Config : ScriptableObject
{
    public string channelName;
    public int[] configValues;
    public float volume = 0;
}
