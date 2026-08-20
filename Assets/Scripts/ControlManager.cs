using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;


public class ControlManager : MonoBehaviour
{
    public static ControlManager instance;
    
    public ControlDisplayer controlDisplayer;
    public CurrentAmbienceDisplay currentAmbienceDisplay;
    
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

    public void PlaySection(LinearSectionEntry track)
    {
        //pause bgm
        //play section
        Debug.Log("Playing Section " + track.trackID + " to " + track.name + ".");
        //start bgm again
    }

    public void PlaySFX(OneShotEntry track)
    {
        //play SFX
        Debug.Log("Playing SFX " + track.trackID + " to " + track.name + ".");
    }

    public void SelectAmbience(AmbienceEntry track)
    {
        Debug.Log("Transition to Ambience " + track.trackID + " to " + track.name + ".");
        currentAmbienceDisplay.ChangeCurrentTrack(track);
        UpdateAdaptiveParameters(track);
    }

    private void UpdateAdaptiveParameters(AmbienceEntry track)
    {
        if (track.adaptiveParameter.Contains(AdaptiveParameter.InsideOutside))
        {
            currentAmbienceDisplay.insideOutsideSlider.SetActive(true);
        }
        else
        {
            currentAmbienceDisplay.insideOutsideSlider.SetActive(false);
        }

        if (track.adaptiveParameter.Contains(AdaptiveParameter.Vividness))
        {
            currentAmbienceDisplay.vividnessSlider.SetActive(true);
        }
        else
        {
            currentAmbienceDisplay.vividnessSlider.SetActive(false);
        }

        if (track.adaptiveParameter.Contains(AdaptiveParameter.DistanceToDestination))
        {
            currentAmbienceDisplay.distanceToDestinationSlider.SetActive(true);
        }
        else
        {
            currentAmbienceDisplay.distanceToDestinationSlider.SetActive(false);
        }
    }
}
