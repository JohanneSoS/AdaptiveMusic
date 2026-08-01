using UnityEngine;

[CreateAssetMenu(fileName = "TrackEntry", menuName = "Scriptable Objects/TrackEntry")]
public class TrackEntry : ScriptableObject
{
    public int trackID;
    public string trackName;
    public Scenario scenario;
    public Theme theme;
    public SoundType soundType;
    public Composer composer;
}

public enum Scenario
{
    None,
    Ruhig,
    Emotional,
    Vorahnung,
    Anspannung,
    Gluecklich,
    Herrlich,
    Mysterioes,
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

public enum Composer
{
    None,
    Tom,
    Johannes,
    other
}