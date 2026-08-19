using UnityEngine;

[CreateAssetMenu(fileName = "LinearSectionEntry", menuName = "Scriptable Objects/LinearSectionEntry")]
public class LinearSectionEntry : ScriptableObject
{
    public int trackID;
    public string trackName;
    public Scenario scenario;
    public Theme theme;
    public Composer composer;
    public SoundType soundType = SoundType.Linear;
}
