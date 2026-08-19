using System.Collections.Generic;
using UnityEngine;
 
#if UNITY_EDITOR
using UnityEditor;
#endif

public class ListManager : MonoBehaviour
{
    [SerializeField] public List<TrackEntry> bgmTracks = new List<TrackEntry>();
    [SerializeField] public List<LinearSectionEntry> linearTracks = new List<LinearSectionEntry>();
    [SerializeField] public List<OneShotEntry> sfxTracks = new List<OneShotEntry>();
    [SerializeField] public List<AmbienceEntry> ambienceTracks = new List<AmbienceEntry>();


    [ContextMenu("Populate Track Lists")]
    private void PopulateTrackLists()
    {
#if UNITY_EDITOR
        bgmTracks = FindAllAssetsOfType<TrackEntry>();
        linearTracks = FindAllAssetsOfType<LinearSectionEntry>();
        sfxTracks = FindAllAssetsOfType<OneShotEntry>();
        ambienceTracks = FindAllAssetsOfType<AmbienceEntry>();
 
        Debug.Log($"[ListManager] Populated: {bgmTracks.Count} BGM, {linearTracks.Count} Linear, {sfxTracks.Count} SFX (OneShot), {ambienceTracks.Count} Ambience tracks.");
#else
        Debug.LogWarning("[ListManager] PopulateTrackLists relies on AssetDatabase and only works in the Editor, not in builds.");
#endif
    }
 
#if UNITY_EDITOR
    // Finds every asset of type T anywhere under Assets/.
    private static List<T> FindAllAssetsOfType<T>() where T : Object
    {
        var result = new List<T>();
        string typeName = typeof(T).Name;
        string[] guids = AssetDatabase.FindAssets($"t:{typeName}");
 
        foreach (string guid in guids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            T asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset != null)
                result.Add(asset);
        }
 
        return result;
    }
#endif
}
