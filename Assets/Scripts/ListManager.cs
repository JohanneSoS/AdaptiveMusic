using UnityEngine;
using System.Collections.Generic;

public class ListManager : MonoBehaviour
{
    [SerializeField] public List<TrackEntry> bgmTracks = new List<TrackEntry>();
    [SerializeField] public List<TrackEntry> linearTracks = new List<TrackEntry>();
    [SerializeField] public List<TrackEntry> sfxTracks = new List<TrackEntry>();

    private void Awake()
    {
    }
}
