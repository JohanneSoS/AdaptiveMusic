using UnityEngine;
using System.Collections.Generic;
public class ListSpawner : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private List<GameObject> rows = new List<GameObject>();
    [SerializeField] private List<TrackEntry> tracks = new List<TrackEntry>();
    [SerializeField] private ListManager listManager;
    [SerializeField] private SoundType soundType;

    private void Awake()
    {
        switch (soundType)
        {
            case SoundType.BGM:
                tracks = listManager.bgmTracks;
                break;
            case SoundType.Linear:
                tracks = listManager.linearTracks;
                break;
            case SoundType.SFX:
                tracks = listManager.sfxTracks;
                break;
        }
        InitializeTable();
    }

    private void InitializeTable()
    {
        foreach (TrackEntry track in tracks)
        {
            SpawnRow(track);
        }
    }
    private void SpawnRow(TrackEntry trackEntry)
    {
        GameObject row = Instantiate(rowPrefab, transform);
        RowEntry rowScript = row.GetComponent<RowEntry>();
        rowScript.Initialize(trackEntry);
        Debug.Log("Spawned Row with" + trackEntry);
    }
}
