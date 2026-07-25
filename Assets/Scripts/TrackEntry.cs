using UnityEngine;

[CreateAssetMenu(fileName = "TrackEntry", menuName = "Scriptable Objects/TrackEntry")]
public class TrackEntry : ScriptableObject
{
    public int trackID;
    public string trackName;
    public Scenario scenario;
    public Theme theme;
    public float lenght;
    public SoundType soundType;
}

public enum Scenario
{
    None,
    Ruhig,
    Emotional,
    UnguteVorahnung,
    Anspannung,
    GluecklicheMomente,
    HerrlicherOrt,
    MysterioeserOrt,
    Kampf,
    Taverne,
    Tempel
}

public enum Theme
{
    None,
    OrthosReich,
    FlioleKoenigreich,
    Echad,
    DodhKoenigreich,
    Tar,
    Dunkelheim,
    BozgFuerstentum
}

public enum SoundType
{
    BGM,
    Linear,
    SFX,
    Ambience
}