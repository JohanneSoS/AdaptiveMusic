// TrackEntryCsvImporter.cs
// Place this file inside an "Editor" folder in your Unity project
// (e.g. Assets/Editor/TrackEntryCsvImporter.cs)
// Requires SoundEntryCsvImportUtils.cs in the same Editor folder.
//
// Usage: Unity menu -> Tools -> Music -> Import BGM Tracks from CSV
// Imports the "Übersicht" (BGM) CSV into TrackEntry ScriptableObjects.
//
// NOTE ON DATA COVERAGE:
// Only rows with a valid TrackID are imported; everything else is skipped.
// The CSV has no "Theme" column, so entry.theme is left at Theme.None -
// fill it in manually afterwards.
//
// SCENARIO MAPPING:
// The Scenario enum's declaration order matches the Sz.-Nr. numbering
// (None=0, Ruhig=1, Emotional=2, ... Kampf=8, ...), so the scenario is
// resolved directly via an int-to-enum cast instead of name matching.

using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static SoundEntryCsvImportUtils;

public static class TrackEntryCsvImporter
{
    private const string LogPrefix = "TrackEntryCsvImporter";
    private const string OutputFolder = "Assets/TrackEntries";

    [MenuItem("Tools/Music/Import BGM Tracks from CSV")]
    public static void ImportFromCsv()
    {
        string path = OpenCsvPanelAndReadRows("Select BGM Track CSV", out List<string[]> rows);
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
        int colScenarioNr = FindColumn(header, "Sz.-Nr.", "Sz-Nr", "Szenario-Nr");
        int colTitel = FindColumn(header, "Titel");
        int colComposer = FindColumn(header, "Composer", "Ersteller");

        if (colTrackID < 0 || colScenarioNr < 0 || colTitel < 0)
        {
            Debug.LogError($"[{LogPrefix}] Required columns ('TrackID', 'Sz.-Nr.', 'Titel') not found in header.");
            return;
        }

        EnsureFolder(OutputFolder);

        int created = 0;
        int skipped = 0;

        for (int i = headerRowIndex + 1; i < rows.Count; i++)
        {
            string[] row = rows[i];
            int minRequiredLength = Math.Max(colTrackID, Math.Max(colScenarioNr, colTitel)) + 1;
            if (row.Length < minRequiredLength)
                continue;

            string trackIDRaw = row[colTrackID].Trim();
            string scenarioNrRaw = row[colScenarioNr].Trim();
            string titelRaw = colTitel < row.Length ? row[colTitel].Trim() : "";
            string composerRaw = colComposer >= 0 && colComposer < row.Length ? row[colComposer].Trim() : "";

            if (!TryParseTrackID(trackIDRaw, out int trackID))
            {
                skipped++;
                continue;
            }

            Scenario scenario = ResolveScenarioByNumber(scenarioNrRaw, LogPrefix, i + 1, trackID);
            Composer composer = ResolveComposer(composerRaw, LogPrefix, i + 1, trackID);

            TrackEntry entry = ScriptableObject.CreateInstance<TrackEntry>();
            entry.trackID = trackID;
            entry.trackName = titelRaw;
            entry.scenario = scenario;
            entry.theme = Theme.None; // not present in CSV - fill in manually
            entry.composer = composer;
            entry.soundType = SoundType.BGM;

            string safeName = MakeSafeFileName($"{trackID}_{titelRaw}");
            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{OutputFolder}/{safeName}.asset");
            AssetDatabase.CreateAsset(entry, assetPath);
            created++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"[{LogPrefix}] Done. Created {created} TrackEntry assets in '{OutputFolder}' " +
                  $"({skipped} rows skipped - no TrackID assigned). " +
                  "Reminder: 'theme' is not present in the CSV and was defaulted to None - please review/fill it in manually.");
    }
}
