// OneShotEntryCsvImporter.cs
// Place this file inside an "Editor" folder in your Unity project
// (e.g. Assets/Editor/OneShotEntryCsvImporter.cs)
// Requires SoundEntryCsvImportUtils.cs in the same Editor folder.
//
// Usage: Unity menu -> Tools -> Music -> Import One-Shot Entries from CSV
// Imports the "Soundliste - OneShots" CSV into OneShotEntry ScriptableObjects.
//
// FIELD MAPPING (per project convention):
// - "Titel" is used as trackName.
// - "Station" is used as stationName.
// - "Ersteller" is used as composer.
// Only rows with a valid TrackID are imported; everything else is skipped.

using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static SoundEntryCsvImportUtils;

public static class OneShotEntryCsvImporter
{
    private const string LogPrefix = "OneShotEntryCsvImporter";
    private const string OutputFolder = "Assets/TrackEntries/OneShot";

    [MenuItem("Tools/Music/Import One-Shot Entries from CSV")]
    public static void ImportFromCsv()
    {
        string path = OpenCsvPanelAndReadRows("Select OneShots CSV", out List<string[]> rows);
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
        int colTitel = FindColumn(header, "Titel");
        int colStation = FindColumn(header, "Station");
        int colComposer = FindColumn(header, "Ersteller", "Composer");

        if (colTrackID < 0 || colTitel < 0 || colStation < 0)
        {
            Debug.LogError($"[{LogPrefix}] Required columns ('TrackID', 'Titel', 'Station') not found in header.");
            return;
        }

        EnsureFolder(OutputFolder);

        int created = 0;
        int skipped = 0;

        for (int i = headerRowIndex + 1; i < rows.Count; i++)
        {
            string[] row = rows[i];
            int minRequiredLength = Math.Max(colTrackID, Math.Max(colTitel, colStation)) + 1;
            if (row.Length < minRequiredLength)
                continue;

            string trackIDRaw = row[colTrackID].Trim();
            string titelRaw = row[colTitel].Trim();
            string stationRaw = row[colStation].Trim();
            string composerRaw = colComposer >= 0 && colComposer < row.Length ? row[colComposer].Trim() : "";

            if (!TryParseTrackID(trackIDRaw, out int trackID))
            {
                skipped++;
                continue;
            }

            Composer composer = ResolveComposer(composerRaw, LogPrefix, i + 1, trackID);

            OneShotEntry entry = ScriptableObject.CreateInstance<OneShotEntry>();
            entry.trackID = trackID;
            entry.trackName = titelRaw;
            entry.stationName = stationRaw;
            entry.composer = composer;
            entry.soundType = SoundType.SFX;

            string safeName = MakeSafeFileName($"{trackID}_{titelRaw}");
            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{OutputFolder}/{safeName}.asset");
            AssetDatabase.CreateAsset(entry, assetPath);
            created++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[{LogPrefix}] Done. Created {created} OneShotEntry assets in '{OutputFolder}' " +
                  $"({skipped} rows skipped - no TrackID assigned).");
    }
}
