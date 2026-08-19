using UnityEngine;
using System.Collections.Generic;
public class ListSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private List<GameObject> rows = new List<GameObject>();
    [SerializeField] private List<TrackEntry> bgmTracks = new List<TrackEntry>();
    [SerializeField] private List<LinearSectionEntry> linearTracks = new List<LinearSectionEntry>();
    [SerializeField] private List<AmbienceEntry> ambienceTracks = new List<AmbienceEntry>();
    [SerializeField] private List<OneShotEntry> oneShotTracks = new List<OneShotEntry>();
    [SerializeField] private ListManager listManager;
    [SerializeField] private SoundType soundType;

    private void Awake()
    {
        switch (soundType)
        {
            case SoundType.BGM:
                bgmTracks = listManager.bgmTracks;
                break;
            case SoundType.Linear:
                linearTracks = listManager.linearTracks;
                break;
            case SoundType.SFX:
                oneShotTracks = listManager.sfxTracks;
                break;
            case SoundType.Ambience:
                ambienceTracks = listManager.ambienceTracks;
                break;
        }
        InitializeTable(soundType);
    }

    private void InitializeTable(SoundType type)
    {
        switch (soundType)
        {
            case SoundType.BGM:
                foreach (TrackEntry track in bgmTracks)
                {
                    SpawnBGMRow(track);
                }
                break;
            case SoundType.Linear:
                foreach (LinearSectionEntry track in linearTracks)
                {
                    SpawnLinearSectionRow(track);
                }
                break;
            case SoundType.Ambience:
                foreach (AmbienceEntry track in ambienceTracks)
                {
                    SpawnAmbienceRow(track);
                }
                break;
            case SoundType.SFX:
                foreach (OneShotEntry track in oneShotTracks)
                {
                    SpawnOneShotRow(track);
                }
                break;
        }
    }
    private void SpawnBGMRow(TrackEntry trackEntry)
    {
        GameObject row = Instantiate(rowPrefab, transform);
        RowEntry rowScript = row.GetComponent<RowEntry>();
        rowScript.InitializeBGM(trackEntry);
        Debug.Log("Spawned Row with" + trackEntry);
    }

    private void SpawnAmbienceRow(AmbienceEntry ambienceEntry)
    {
        GameObject row = Instantiate(rowPrefab, transform);
        RowEntry rowScript = row.GetComponent<RowEntry>();
        rowScript.InitializeAmbience(ambienceEntry);
        Debug.Log("Spawned Row with" + ambienceEntry);
    }

    private void SpawnOneShotRow(OneShotEntry oneShotEntry)
    {
        GameObject row = Instantiate(rowPrefab, transform);
        RowEntry rowScript = row.GetComponent<RowEntry>();
        rowScript.InitializeOneShot(oneShotEntry);
        Debug.Log("Spawned Row with" + oneShotEntry);
    }

    private void SpawnLinearSectionRow(LinearSectionEntry linearEntry)
    {
        GameObject row = Instantiate(rowPrefab, transform);
        RowEntry rowScript = row.GetComponent<RowEntry>();
        rowScript.InitializeLinearSection(linearEntry);
        Debug.Log("Spawned Row with" + linearEntry);
    }
}
