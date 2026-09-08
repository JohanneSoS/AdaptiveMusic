using UnityEngine;
using UnityEngine.UI;
using FMODUnity;
using FMOD.Studio;

public class AudioPlayer : MonoBehaviour
{
    public static AudioPlayer instance;
    
    [SerializeField] private EventReference bgmMusic;
    [SerializeField] private EventReference[] linearMusic;
    [SerializeField] private EventReference ambience;
    [SerializeField] private EventReference[] oneShotSounds;
    
    private FMOD.Studio.EventInstance bgmMusicInstance;
    private FMOD.Studio.EventInstance ambienceInstance;
    
    [SerializeField] private Slider intensitySlider;
    [SerializeField] private Slider insideOutsideSlider;
    [SerializeField] private Slider distanceToDestinationSlider;
    [SerializeField] private Slider vividnessSlider;
    [SerializeField] private Slider peopleNearbySlider;
    [SerializeField] private Slider dayToNightSlider;
    
    [SerializeField] private float intensity;
    [SerializeField] private float insideOutside;
    [SerializeField] private float distanceToDestination;
    [SerializeField] private float vividness;
    [SerializeField] private float peopleNearby;
    [SerializeField] private float dayToNight;
    public int currentBgmID;
    public int currentAmbienceID;
    

    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        bgmMusicInstance = FMODUnity.RuntimeManager.CreateInstance(bgmMusic);
        ambienceInstance = FMODUnity.RuntimeManager.CreateInstance(ambience);
        RuntimeManager.StudioSystem.setParameterByName("CurrentBgmID", 0);
        RuntimeManager.StudioSystem.setParameterByName("TargetBgmID", 0);
        RuntimeManager.StudioSystem.setParameterByName("Intensity", 0);
        RuntimeManager.StudioSystem.setParameterByName("Music_State", 0);
        RuntimeManager.StudioSystem.setParameterByName("TransitionState", 1);
        bgmMusicInstance.start();
        RuntimeManager.StudioSystem.setParameterByName("CurrentAmbience", 0);
        ambienceInstance.start();
    }

    void Update()
    {
        RuntimeManager.StudioSystem.getParameterByName("CurrentBgmID", out float currentID);
        currentBgmID = (int)currentID;
    }

    public void ChangeIntensity()
    {
        intensity = intensitySlider.value;
        RuntimeManager.StudioSystem.setParameterByName("Intensity", intensity);
    }

    public void TransitionToNewBGM(TrackEntry newTrack)
    {
        RuntimeManager.StudioSystem.setParameterByName("TargetBgmID", newTrack.trackID);
    }

    public void ChangeAmbienceTrack(AmbienceEntry newTrack)
    {
        RuntimeManager.StudioSystem.setParameterByName("CurrentAmbienceID", newTrack.trackID);
    }

    public void ChangeInsideOutside()
    {
        insideOutside = insideOutsideSlider.value;
        RuntimeManager.StudioSystem.setParameterByName("OutsideToInside", insideOutside);
        Debug.Log("Inside outside: " + insideOutside);
    }

    public void ChangeDistanceToDestination()
    {
        distanceToDestination = distanceToDestinationSlider.value;
        RuntimeManager.StudioSystem.setParameterByName("DistToDest", distanceToDestination);
        Debug.Log("Distance to destination: " + distanceToDestination);
    }

    public void ChangeVividness()
    {
        vividness = vividnessSlider.value;
        Debug.Log("Vividness: " + vividness);
    }

    public void ChangePeopleNearby()
    {
        peopleNearby = peopleNearbySlider.value;
        RuntimeManager.StudioSystem.setParameterByName("PeopleNearby", peopleNearby);
    }

    public void ChangeDayToNight()
    {
        dayToNight = dayToNightSlider.value;
        RuntimeManager.StudioSystem.setParameterByName("DayToNight", dayToNight);
    }
}
