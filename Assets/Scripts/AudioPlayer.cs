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
    private FMOD.Studio.EventInstance[] ambienceInstance;
    
    [SerializeField] private Slider intensitySlider;
    [SerializeField] private Slider insideOutsideSlider;
    [SerializeField] private Slider distanceToDestinationSlider;
    [SerializeField] private Slider vividnessSlider;

    [SerializeField] private float intensity;
    [SerializeField] private float insideOutside;
    [SerializeField] private float distanceToDestination;
    [SerializeField] private float vividness;
    public int currentBgmID; 
    
    void Awake()
    {
        instance = this;
    }
    
    void Start()
    {
        bgmMusicInstance = FMODUnity.RuntimeManager.CreateInstance(bgmMusic);
        RuntimeManager.StudioSystem.setParameterByName("CurrentBgmID", 0);
        RuntimeManager.StudioSystem.setParameterByName("TargetBgmID", 0);
        RuntimeManager.StudioSystem.setParameterByName("Intensity", 0);
        RuntimeManager.StudioSystem.setParameterByName("Music_State", 0);
        RuntimeManager.StudioSystem.setParameterByName("TransitionState", 1);
        bgmMusicInstance.start();
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

    public void ChangeInsideOutside()
    {
        insideOutside = insideOutsideSlider.value;
        Debug.Log("Inside outside: " + insideOutside);
    }

    public void ChangeDistanceToDestination()
    {
        distanceToDestination = distanceToDestinationSlider.value;
        Debug.Log("Distance to destination: " + distanceToDestination);
    }

    public void ChangeVividness()
    {
        vividness = vividnessSlider.value;
        Debug.Log("Vividness: " + vividness);
    }
    
}
