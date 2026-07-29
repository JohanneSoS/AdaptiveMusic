using UnityEngine;
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

    public void ChangeIntensity(float newValue)
    {
        RuntimeManager.StudioSystem.setParameterByName("Intensity", newValue);
    }

    public void TransitionToNewBGM(TrackEntry newTrack)
    {
        RuntimeManager.StudioSystem.setParameterByName("TargetBgmID", newTrack.trackID);
    }
    
}
