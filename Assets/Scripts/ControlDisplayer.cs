using TMPro;
using UnityEngine;

public class ControlDisplayer : MonoBehaviour
{
    [Header("SelectedTrackInfo")]
    [SerializeField] private TextMeshProUGUI selectedTrackName;
    [SerializeField] private TextMeshProUGUI selectedTrackID;
    [SerializeField] private TextMeshProUGUI selectedTrackScenario;
    [SerializeField] private TextMeshProUGUI selectedTrackTheme;
    [SerializeField] private TextMeshProUGUI selectedTrackComposer;
    
    [Header("CurrentTrackInfo")]
    [SerializeField] private TextMeshProUGUI currentTrackName;
    [SerializeField] private TextMeshProUGUI currentTrackID;
    [SerializeField] private TextMeshProUGUI currentTrackScenario;
    [SerializeField] private TextMeshProUGUI currentTrackTheme;
    [SerializeField] private TextMeshProUGUI currentTrackComposer;
    
    [Header("TransitionInfo")]
    [SerializeField] private TextMeshProUGUI transitionFrom;
    [SerializeField] private TextMeshProUGUI transitionTo;

    private void Start()
    {
        ChangeCurrentTrack(ControlManager.instance.currentBGMTrack);
    }
    
    public void ChangeSelectedTrack(TrackEntry selectedTrack)
    {
        selectedTrackName.text = selectedTrack.trackName;
        selectedTrackID.text = selectedTrack.trackID.ToString();
        selectedTrackScenario.text = selectedTrack.scenario.ToString();
        selectedTrackTheme.text = selectedTrack.theme.ToString();
        selectedTrackComposer.text = selectedTrack.composer.ToString();
        
        string transitionToText = selectedTrack.trackID + "/" + selectedTrack.trackName;
        transitionTo.text = transitionToText;
    }

    public void ChangeCurrentTrack(TrackEntry currentTrack)
    {
        currentTrackName.text = currentTrack.trackName;
        currentTrackID.text = currentTrack.trackID.ToString();
        currentTrackScenario.text = currentTrack.scenario.ToString();
        currentTrackTheme.text = currentTrack.theme.ToString();
        currentTrackComposer.text = currentTrack.composer.ToString();   
        
        string transitionFromText = currentTrack.trackID + "/" + currentTrack.trackName;
        transitionFrom.text = transitionFromText;
        transitionTo.text = "None";
        selectedTrackName.text = "None";
        selectedTrackID.text = "None";
        selectedTrackScenario.text = "None";
        selectedTrackTheme.text = "None";
        selectedTrackComposer.text = "None";
    }
}
