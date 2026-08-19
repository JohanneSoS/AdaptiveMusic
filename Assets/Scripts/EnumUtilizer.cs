using System.Collections.Generic;
using UnityEngine;

public class EnumUtilizer : MonoBehaviour
{

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
    Leo,
    other
}

public enum AdaptiveParameter
{
    None,
    InsideOutside,
    DistanceToDestination,
    Vividness
}