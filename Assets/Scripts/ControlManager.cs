using UnityEngine;
using UnityEngine.Events;


public class ControlManager : MonoBehaviour
{
    public static ControlManager instance;
    
    public ControlDisplayer controlDisplayer;
    
    public TrackEntry currentBGMTrack;
    public TrackEntry selectedBGMTrack;
    

    private void Awake()
    {
        instance = this;
    }
    
    public void SelectBGMTrack(TrackEntry track)
    {
        selectedBGMTrack = track;
        controlDisplayer.ChangeSelectedTrack(track);
    }

    public void StartTransition()
    {
        if (selectedBGMTrack != null)
        {
            AudioPlayer.instance.TransitionToNewBGM(selectedBGMTrack);
            Debug.Log("Start Transition from " + currentBGMTrack.trackID + currentBGMTrack.name + " to " + selectedBGMTrack.trackID + selectedBGMTrack.name + ".");
            currentBGMTrack = selectedBGMTrack;
            controlDisplayer.ChangeCurrentTrack(currentBGMTrack);
            selectedBGMTrack = null;
        }
        else
        {
            Debug.Log("No Track Selected for Transition.");
        }
    }

    public void PlaySection(TrackEntry track)
    {
        //pause bgm
        //play section
        Debug.Log("Playing Section " + track.trackID + " to " + track.name + ".");
        //start bgm again
    }

    public void PlaySFX(TrackEntry track)
    {
        //play SFX
        Debug.Log("Playing SFX " + track.trackID + " to " + track.name + ".");
    }
}
