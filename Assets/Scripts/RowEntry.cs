using TMPro;
using UnityEngine;

public class RowEntry : MonoBehaviour
{
    public TrackEntry entry;
    
    [SerializeField] private TextMeshProUGUI trackID;
    [SerializeField] private TextMeshProUGUI trackName;
    [SerializeField] private TextMeshProUGUI scenario;
    [SerializeField] private TextMeshProUGUI theme;
    [SerializeField] private TextMeshProUGUI composer;

    public void Initialize(TrackEntry trackEntry)
    {
        entry = trackEntry;
        
        trackID.text = entry.trackID.ToString();
        trackName.text = entry.trackName;
        scenario.text = entry.scenario.ToString();
        theme.text = entry.theme.ToString();
        composer.text = entry.composer.ToString();
    }

    public void OnClick()
    {
        switch (entry.soundType)
        {
            case SoundType.BGM:
                ControlManager.instance.SelectBGMTrack(entry);
                break;
            case SoundType.Linear:
                ControlManager.instance.PlaySection(entry);
                break;
            case SoundType.SFX:
                ControlManager.instance.PlaySFX(entry);
                break;
            case SoundType.Ambience:
                break;
        }
        if (entry.soundType == SoundType.BGM)
        {
            ControlManager.instance.SelectBGMTrack(entry);
        }
    }
}
