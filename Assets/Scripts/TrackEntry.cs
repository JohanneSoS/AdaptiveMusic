using UnityEngine;

[CreateAssetMenu(fileName = "TrackEntry", menuName = "Scriptable Objects/TrackEntry")]
public class TrackEntry : ScriptableObject
{
    public int trackID;
    public string trackName;
    public Scenario scenario;
    public Theme theme;
    public Composer composer;
    public SoundType soundType = SoundType.BGM;
}
