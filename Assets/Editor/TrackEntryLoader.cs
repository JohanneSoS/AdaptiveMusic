// TrackEntryCsvImporter.cs
// Place this file inside an "Editor" folder in your Unity project
// (e.g. Assets/Editor/TrackEntryCsvImporter.cs)
//
// Usage: Unity menu -> Tools -> Music -> Import Track Entries from CSV
//
// NOTE ON DATA COVERAGE:
// The source CSV contains a "TrackID" column - only rows that have a valid
// (non-empty, non "#N/A") TrackID are imported; every other row is skipped.
// The CSV does NOT contain columns for Theme, SoundType, or lenght (duration).
// Those three fields are therefore created with default values
// (Theme.None, SoundType.BGM, lenght = 0) and a warning is logged for
// every asset so you know to fill them in manually afterwards.
//
// SCENARIO MAPPING:
// The Scenario enum's declaration order matches the Sz.-Nr. numbering
// (None=0, Ruhig=1, Emotional=2, ... Kampf=8, ...), so the scenario is
// resolved directly via an int-to-enum cast instead of name matching.
//
// COMPOSER MAPPING:
// The Composer enum now uses short names (Tom, Johannes, other) that are
// matched against the CSV's "Composer" column directly.
 
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class TrackEntryLoader : MonoBehaviour
{
       // Where the generated .asset files will be created.
    private const string OutputFolder = "Assets/TrackEntries";
 
    [MenuItem("Tools/Music/Import Track Entries from CSV")]
    public static void ImportFromCsv()
    {
        string path = EditorUtility.OpenFilePanel("Select Music Track CSV", Application.dataPath, "csv");
        if (string.IsNullOrEmpty(path))
            return;
 
        string csvText = File.ReadAllText(path, Encoding.UTF8);
        List<string[]> rows = ParseCsv(csvText);
 
        if (rows.Count == 0)
        {
            Debug.LogError("[TrackEntryCsvImporter] CSV file appears to be empty.");
            return;
        }
 
        // Find the header row dynamically (the row that contains "Titel"),
        // since the sheet has an extra merged-cell row above the real header.
        int headerRowIndex = rows.FindIndex(r => r.Any(c => c.Trim().Equals("Titel", StringComparison.OrdinalIgnoreCase)));
        if (headerRowIndex < 0)
        {
            Debug.LogError("[TrackEntryCsvImporter] Could not find header row (column 'Titel' not found).");
            return;
        }
 
        string[] header = rows[headerRowIndex];
        int colTrackID = FindColumn(header, "TrackID", "Track-ID", "Track ID", "ID");
        int colScenarioNr = FindColumn(header, "Sz.-Nr.", "Sz-Nr", "Szenario-Nr");
        int colTitel = FindColumn(header, "Titel");
        int colComposer = FindColumn(header, "Composer");
 
        if (colTrackID < 0 || colScenarioNr < 0 || colTitel < 0)
        {
            Debug.LogError("[TrackEntryCsvImporter] Required columns ('TrackID', 'Sz.-Nr.', 'Titel') not found in header.");
            return;
        }
 
        if (!AssetDatabase.IsValidFolder(OutputFolder))
        {
            Directory.CreateDirectory(OutputFolder);
            AssetDatabase.Refresh();
        }
 
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
 
            // Only import rows that have an actual TrackID assigned; skip everything else
            // (empty rows, placeholder "#N/A" rows, drafts without an assigned ID, etc.).
            if (IsEmptyOrNA(trackIDRaw) || !int.TryParse(trackIDRaw, NumberStyles.Integer, CultureInfo.InvariantCulture, out int trackID))
            {
                skipped++;
                continue;
            }
 
            // Scenario enum declaration order matches Sz.-Nr., so resolve it directly via cast.
            int scenarioNr = ParseLeadingInt(scenarioNrRaw);
            Scenario scenario;
            if (Enum.IsDefined(typeof(Scenario), scenarioNr))
            {
                scenario = (Scenario)scenarioNr;
            }
            else
            {
                scenario = Scenario.None;
                Debug.LogWarning($"[TrackEntryCsvImporter] Sz.-Nr. '{scenarioNrRaw}' (row {i + 1}, TrackID {trackID}) does not map to any Scenario enum value. Defaulting to None.");
            }
 
            Composer composer = MatchEnum<Composer>(composerRaw, Composer.other);
            if (composer == Composer.other && !string.IsNullOrEmpty(composerRaw))
                Debug.LogWarning($"[TrackEntryCsvImporter] Could not map composer '{composerRaw}' (row {i + 1}, TrackID {trackID}). Defaulting to other.");
 
            TrackEntry entry = ScriptableObject.CreateInstance<TrackEntry>();
            entry.trackID = trackID;
            entry.trackName = titelRaw;
            entry.scenario = scenario;
            entry.composer = Composer.None;                // not present in CSV - fill in manually
            entry.soundType = SoundType.BGM; // not present in CSV - fill in manually, defaulted to BGM
            entry.composer = composer;
 
            string safeName = MakeSafeFileName($"{trackID}_{titelRaw}");
            string assetPath = AssetDatabase.GenerateUniqueAssetPath($"{OutputFolder}/{safeName}.asset");
            AssetDatabase.CreateAsset(entry, assetPath);
            created++;
        }
 
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
 
        Debug.Log($"[TrackEntryCsvImporter] Done. Created {created} TrackEntry assets in '{OutputFolder}' " +
                  $"({skipped} rows skipped - no TrackID assigned). " +
                  "Reminder: 'theme', 'lenght' and 'soundType' were not present in the CSV and were set to default values - please review/fill them in manually.");
    }
 
    // ---------------------------------------------------------------
    // Helpers
    // ---------------------------------------------------------------
 
    private static bool IsEmptyOrNA(string s)
    {
        return string.IsNullOrWhiteSpace(s) || s.Trim().Equals("#N/A", StringComparison.OrdinalIgnoreCase);
    }
 
    private static int FindColumn(string[] header, params string[] names)
    {
        for (int i = 0; i < header.Length; i++)
        {
            string h = header[i].Trim();
            foreach (var n in names)
            {
                if (h.Equals(n, StringComparison.OrdinalIgnoreCase))
                    return i;
            }
        }
        return -1;
    }
 
    private static int ParseLeadingInt(string s)
    {
        if (string.IsNullOrEmpty(s))
            return 0;
        string digits = new string(s.Where(char.IsDigit).ToArray());
        return int.TryParse(digits, out int result) ? result : 0;
    }
 
    // Normalizes a string for fuzzy enum matching:
    // lowercases, strips whitespace/punctuation, converts German umlauts
    // to their ASCII digraphs (ä->ae, ö->oe, ü->ue, ß->ss).
    private static string Normalize(string s)
    {
        if (string.IsNullOrEmpty(s))
            return "";
 
        s = s.Replace("ä", "ae").Replace("ö", "oe").Replace("ü", "ue").Replace("ß", "ss")
             .Replace("Ä", "ae").Replace("Ö", "oe").Replace("Ü", "ue");
 
        var sb = new StringBuilder();
        foreach (char c in s)
        {
            if (char.IsLetterOrDigit(c))
                sb.Append(char.ToLowerInvariant(c));
        }
        return sb.ToString();
    }
 
    // Finds the enum value whose normalized name is the longest substring
    // match within the normalized input string. Falls back to defaultValue.
    private static T MatchEnum<T>(string raw, T defaultValue) where T : struct, Enum
    {
        if (string.IsNullOrWhiteSpace(raw))
            return defaultValue;
 
        string normalizedInput = Normalize(raw);
        if (normalizedInput.Length == 0)
            return defaultValue;
 
        T best = defaultValue;
        int bestLength = 0;
 
        foreach (T value in Enum.GetValues(typeof(T)))
        {
            string normalizedName = Normalize(value.ToString());
            if (normalizedName.Length == 0)
                continue;
 
            if (normalizedInput.Contains(normalizedName) && normalizedName.Length > bestLength)
            {
                best = value;
                bestLength = normalizedName.Length;
            }
        }
 
        return bestLength > 0 ? best : defaultValue;
    }
 
    private static string MakeSafeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name.Replace('\n', '_').Replace('\r', '_').Trim();
    }
 
    // Minimal CSV parser supporting quoted fields, escaped quotes (""),
    // commas inside quotes, and newlines inside quoted fields.
    private static List<string[]> ParseCsv(string text)
    {
        var rows = new List<string[]>();
        var field = new StringBuilder();
        var row = new List<string>();
        bool inQuotes = false;
 
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];
 
            if (inQuotes)
            {
                if (c == '"')
                {
                    if (i + 1 < text.Length && text[i + 1] == '"')
                    {
                        field.Append('"');
                        i++;
                    }
                    else
                    {
                        inQuotes = false;
                    }
                }
                else
                {
                    field.Append(c);
                }
            }
            else
            {
                if (c == '"')
                {
                    inQuotes = true;
                }
                else if (c == ',')
                {
                    row.Add(field.ToString());
                    field.Clear();
                }
                else if (c == '\r')
                {
                    // ignore, handled by \n
                }
                else if (c == '\n')
                {
                    row.Add(field.ToString());
                    field.Clear();
                    rows.Add(row.ToArray());
                    row = new List<string>();
                }
                else
                {
                    field.Append(c);
                }
            }
        }
 
        // last field/row if file doesn't end with newline
        if (field.Length > 0 || row.Count > 0)
        {
            row.Add(field.ToString());
            rows.Add(row.ToArray());
        }
 
        return rows;
    }
}