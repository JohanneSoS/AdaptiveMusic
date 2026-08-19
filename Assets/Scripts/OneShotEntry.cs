using UnityEngine;

[CreateAssetMenu(fileName = "OneShotEntry", menuName = "Scriptable Objects/OneShotEntry")]
public class OneShotEntry : ScriptableObject
{
    public int trackID;
    public string trackName;
    public string stationName;
    public Composer composer;
    public SoundType soundType = SoundType.SFX;
}
