using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AmbienceEntry", menuName = "Scriptable Objects/AmbienceEntry")]
public class AmbienceEntry : ScriptableObject
{
    public int trackID;
    public string trackName;
    public Composer composer;
    public List<AdaptiveParameter> adaptiveParameter = new List<AdaptiveParameter>();
    public SoundType soundType = SoundType.Ambience;
}
