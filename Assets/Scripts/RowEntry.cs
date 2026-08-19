using TMPro;
using UnityEngine;

public class RowEntry : MonoBehaviour
{
    public TrackEntry bgmEntry;
    public LinearSectionEntry linearEntry;
    public AmbienceEntry ambienceEntry;
    public OneShotEntry oneShotEntry;
    private SoundType soundType;
    [SerializeField] private TextMeshProUGUI trackID;
    [SerializeField] private TextMeshProUGUI trackName;
    [SerializeField] private TextMeshProUGUI scenario;
    [SerializeField] private TextMeshProUGUI theme;
    [SerializeField] private TextMeshProUGUI composer;
    [SerializeField] private TextMeshProUGUI station;
    //[SerializeField] private TextMeshProUGUI parameters;

    public void InitializeBGM(TrackEntry trackEntry)
    {
        soundType = SoundType.BGM;
        bgmEntry = trackEntry;
        trackID.text = trackEntry.trackID.ToString();
        trackName.text = trackEntry.trackName;
        scenario.text = trackEntry.scenario.ToString();
        theme.text = trackEntry.theme.ToString();
        composer.text = trackEntry.composer.ToString();
    }

    public void InitializeAmbience(AmbienceEntry trackEntry)
    {
        soundType = SoundType.Ambience;
        ambienceEntry = trackEntry;
        trackID.text = trackEntry.trackID.ToString();
        trackName.text = trackEntry.trackName;
        //parameters.text = trackEntry.adaptiveParameter.ToString();
        composer.text = trackEntry.composer.ToString();
    }

    public void InitializeOneShot(OneShotEntry trackEntry)
    {
        soundType = SoundType.SFX;
        oneShotEntry = trackEntry;
        trackID.text = trackEntry.trackID.ToString();
        trackName.text = trackEntry.trackName;
        station.text = trackEntry.stationName;
        composer.text = trackEntry.composer.ToString();
    }

    public void InitializeLinearSection(LinearSectionEntry trackEntry)
    {
        soundType = SoundType.Linear;
        linearEntry = trackEntry;
        trackID.text = trackEntry.trackID.ToString();
        trackName.text = trackEntry.trackName;
        scenario.text = trackEntry.scenario.ToString();
        theme.text = trackEntry.theme.ToString();
        composer.text = trackEntry.composer.ToString();
    }

    public void OnClick()
    {
        switch (soundType)
        {
            case SoundType.BGM:
                ControlManager.instance.SelectBGMTrack(bgmEntry);
                break;
            case SoundType.Linear:
                ControlManager.instance.PlaySection(linearEntry);
                break;
            case SoundType.SFX:
                ControlManager.instance.PlaySFX(oneShotEntry);
                break;
            case SoundType.Ambience:
                ControlManager.instance.SelectAmbience(ambienceEntry);
                break;
        }
    }
}
