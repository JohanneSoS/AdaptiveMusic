using TMPro;
using UnityEngine;


public class CurrentAmbienceDisplay : MonoBehaviour
{
    [Header("CurrentTrackInfo")]
    [SerializeField] private TextMeshProUGUI currentTrackName;
    [SerializeField] private TextMeshProUGUI currentTrackID;
    [SerializeField] private TextMeshProUGUI currentTrackComposer;
    
    [Header("CurrentSliders")]
    public GameObject insideOutsideSlider;
    public GameObject distanceToDestinationSlider;
    public GameObject vividnessSlider;
    
    public void ChangeCurrentTrack(AmbienceEntry currentTrack)
    {
        currentTrackName.text = currentTrack.trackName;
        currentTrackID.text = currentTrack.trackID.ToString();
        currentTrackComposer.text = currentTrack.composer.ToString();   
        
        //string transitionFromText = currentTrack.trackID + "/" + currentTrack.trackName;
    }
}
