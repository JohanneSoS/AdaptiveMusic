using TMPro;
using UnityEngine;

public class ControlDisplayer : MonoBehaviour
{
    [Header("SelectedTrackInfo")]
    [SerializeField] private TextMeshProUGUI selectedTrackName;
    [SerializeField] private TextMeshProUGUI selectedTrackID;
    [SerializeField] private TextMeshProUGUI selectedTrackScenario;
    [SerializeField] private TextMeshProUGUI selectedTrackTheme;
    [SerializeField] private TextMeshProUGUI selectedTrackLenght;
    
    [Header("CurrentTrackInfo")]
    [SerializeField] private TextMeshProUGUI currentTrackName;
    [SerializeField] private TextMeshProUGUI currentTrackID;
    [SerializeField] private TextMeshProUGUI currentTrackScenario;
    [SerializeField] private TextMeshProUGUI currentTrackTheme;
    [SerializeField] private TextMeshProUGUI currentTrackLenght;
    
    [Header("TransitionInfo")]
    [SerializeField] private TextMeshProUGUI transitionFrom;
    [SerializeField] private TextMeshProUGUI transitionTo;

    public void Awake()
    {
        ChangeCurrentTrack(ControlManager.instance.currentBGMTrack);
    }
    
    public void ChangeSelectedTrack(TrackEntry selectedTrack)
    {
        selectedTrackName.text = selectedTrack.trackName;
        selectedTrackID.text = selectedTrack.trackID.ToString();
        selectedTrackScenario.text = selectedTrack.scenario.ToString();
        selectedTrackTheme.text = selectedTrack.theme.ToString();
        selectedTrackLenght.text = selectedTrack.lenght.ToString();
        
        string transitionToText = selectedTrack.trackID + "/" + selectedTrack.trackName;
        transitionTo.text = transitionToText;
    }

    public void ChangeCurrentTrack(TrackEntry currentTrack)
    {
        currentTrackName.text = currentTrack.trackName;
        currentTrackID.text = currentTrack.trackID.ToString();
        currentTrackScenario.text = currentTrack.scenario.ToString();
        currentTrackTheme.text = currentTrack.theme.ToString();
        currentTrackLenght.text = currentTrack.lenght.ToString();   
        
        string transitionFromText = currentTrack.trackID + "/" + currentTrack.trackName;
        transitionFrom.text = transitionFromText;
        transitionTo.text = "None";
        
        selectedTrackName.text = "None";
        selectedTrackID.text = "None";
        selectedTrackScenario.text = "None";
        selectedTrackTheme.text = "None";
        selectedTrackLenght.text = "None";
    }
}
