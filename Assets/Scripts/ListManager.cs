using System.Collections.Generic;
using UnityEngine;
 
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ListManager : MonoBehaviour
{
    [SerializeField] public List<TrackEntry> bgmTracks = new List<TrackEntry>();
    [SerializeField] public List<TrackEntry> linearTracks = new List<TrackEntry>();
    [SerializeField] public List<TrackEntry> sfxTracks = new List<TrackEntry>();
    [SerializeField] public List<TrackEntry> ambienceTracks = new List<TrackEntry>();


    [ContextMenu("Populate Track Lists")]
    private void PopulateTrackLists()
    {
#if UNITY_EDITOR
        bgmTracks.Clear();
        linearTracks.Clear();
        sfxTracks.Clear();
        ambienceTracks.Clear();
 
        string[] guids = AssetDatabase.FindAssets("t:TrackEntry");
 
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            TrackEntry entry = AssetDatabase.LoadAssetAtPath<TrackEntry>(assetPath);
            if (entry == null)
                continue;
 
            switch (entry.soundType)
            {
                case SoundType.BGM:
                    bgmTracks.Add(entry);
                    break;
                case SoundType.Linear:
                    linearTracks.Add(entry);
                    break;
                case SoundType.SFX:
                    sfxTracks.Add(entry);
                    break;
                case SoundType.Ambience:
                    ambienceTracks.Add(entry);
                    break;
                default:
                    Debug.LogWarning($"[ListManager] TrackEntry '{entry.name}' has an unhandled SoundType '{entry.soundType}' and was not added to any list.");
                    break;
            }
        }
 
        Debug.Log($"[ListManager] Populated: {bgmTracks.Count} BGM, {linearTracks.Count} Linear, {sfxTracks.Count} SFX, {ambienceTracks.Count} Ambience tracks.");
#else
        Debug.LogWarning("[ListManager] PopulateTrackLists relies on AssetDatabase and only works in the Editor, not in builds.");
#endif
    }
}
