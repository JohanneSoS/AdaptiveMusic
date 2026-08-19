// AmbienceEntryCsvImporter.cs
// Place this file inside an "Editor" folder in your Unity project
// (e.g. Assets/Editor/AmbienceEntryCsvImporter.cs)
// Requires SoundEntryCsvImportUtils.cs in the same Editor folder.
//
// Usage: Unity menu -> Tools -> Music -> Import Ambience Entries from CSV
// Imports the "Soundliste - Ambiences" CSV into AmbienceEntry ScriptableObjects.
//
// FIELD MAPPING (per project convention):
// - "Station" is used as trackName; "Titel" is ignored.
// - "Ersteller" is used as composer.
// - "Parameters" is a comma-separated list (e.g. "InsideOutside, DistanceToDestination")
//   that gets parsed into entry.adaptiveParameter.
// Only rows with a valid TrackID are imported; everything else is skipped.

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static SoundEntryCsvImportUtils;

public static class AmbienceEntryCsvImporter
{
    private const string LogPrefix = "AmbienceEntryCsvImporter";
    private const string OutputFolder = "Assets/TrackEntries/Ambience";

    [MenuItem("Tools/Music/Import Ambience Entries from CSV")]
    public static void ImportFromCsv()
    {
        string path = OpenCsvPanelAndReadRows("Select Ambiences CSV", out List<string[]> rows);
        if (path == null)
            return;

        if (rows == null || rows.Count == 0)
        {
            Debug.LogError($"[{LogPrefix}] CSV file appears to be empty.");
            return;
        }

        int headerRowIndex = FindHeaderRow(rows);
        if (headerRowIndex < 0)
        {
            Debug.LogError($"[{LogPrefix}] Could not find header row (column 'Titel' not found).");
            return;
        }

        string[] header = rows[headerRowIndex];
        int colTrackID = FindColumn(header, "TrackID", "Track-ID", "Track ID", "ID");
        int colStation = FindColumn(header, "Station");
        int colComposer = FindColumn(header, "Ersteller", "Composer");
        int colParameters = FindColumn(header, "Parameters", "Parameter");

        if (colTrackID < 0 || colStation < 0)
        {
            Debug.LogError($"[{LogPrefix}] Required columns ('TrackID', 'Station') not found in header.");
            return;
        }

        EnsureFolder(OutputFolder);

        int created = 0;
        int skipped = 0;

        for (int i = headerRowIndex + 1; i < rows.Count; i++)
        {
            string[] row = rows[i];
            int minRequiredLength = Math.Max(colTrackID, colStation) + 1;
            if (row.Length < minRequiredLength)
                continue;

            string trackIDRaw = row[colTrackID].Trim();
            string stationRaw = row[colStation].Trim();
            string composerRaw = colComposer >= 0 && colComposer < row.Length ? row[colComposer].Trim() : "";
            string parametersRaw = colParameters >= 0 && colParameters < row.Length ? row[colParameters].Trim() : "";

            if (!TryParseTrackID(trackIDRaw, out int trackID))
            {
                skipped++;
                continue;
            }

            Composer composer = ResolveComposer(composerRaw, LogPrefix, i + 1, trackID);
            List<AdaptiveParameter> adaptiveParameters = ParseAdaptiveParameters(parametersRaw, i + 1, trackID);

            AmbienceEntry entry = ScriptableObject.CreateInstance<AmbienceEntry>();
            entry.trackID = trackID;
            entry.trackName = stationRaw; // Station used as trackName, Titel intentionally ignored
            entry.composer = composer;
            entry.adaptiveParameter = adaptiveParameters;
            entry.soundType = SoundType.Ambience;

            string safeName = MakeSafeFileName($"{trackID}_{stationRaw}");
            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{OutputFolder}/{safeName}.asset");
            AssetDatabase.CreateAsset(entry, assetPath);
            created++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[{LogPrefix}] Done. Created {created} AmbienceEntry assets in '{OutputFolder}' " +
                  $"({skipped} rows skipped - no TrackID assigned).");
    }

    // Splits a comma-separated "Parameters" cell (e.g. "InsideOutside, DistanceToDestination")
    // and fuzzy-matches each piece against the AdaptiveParameter enum. Unmatched pieces are
    // logged and skipped rather than added as "None".
    private static List<AdaptiveParameter> ParseAdaptiveParameters(string raw, int rowNumber, int trackID)
    {
        var result = new List<AdaptiveParameter>();
        if (string.IsNullOrWhiteSpace(raw))
            return result;

        string[] pieces = raw.Split(',');
        foreach (string piece in pieces)
        {
            string trimmed = piece.Trim();
            if (trimmed.Length == 0)
                continue;

            AdaptiveParameter matched = MatchEnum(trimmed, AdaptiveParameter.None);
            if (matched == AdaptiveParameter.None)
            {
                Debug.LogWarning($"[{LogPrefix}] Could not map adaptive parameter '{trimmed}' (row {rowNumber}, TrackID {trackID}). Skipped.");
                continue;
            }

            if (!result.Contains(matched))
                result.Add(matched);
        }

        return result;
    }
}
