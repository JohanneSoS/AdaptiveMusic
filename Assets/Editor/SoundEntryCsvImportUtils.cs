// SoundEntryCsvImportUtils.cs
// Place this file inside an "Editor" folder in your Unity project
// (e.g. Assets/Editor/SoundEntryCsvImportUtils.cs)
//
// Shared helpers used by all *CsvImporter scripts (TrackEntry, LinearSectionEntry,
// AmbienceEntry, OneShotEntry). Not a MenuItem itself - just plumbing.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

internal static class SoundEntryCsvImportUtils
{
    public static bool IsEmptyOrNA(string s)
    {
        return string.IsNullOrWhiteSpace(s) || s.Trim().Equals("#N/A", StringComparison.OrdinalIgnoreCase);
    }

    public static int FindColumn(string[] header, params string[] names)
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

    public static int ParseLeadingInt(string s)
    {
        if (string.IsNullOrEmpty(s))
            return 0;
        string digits = new string(s.Where(char.IsDigit).ToArray());
        return int.TryParse(digits, out int result) ? result : 0;
    }

    public static bool TryParseTrackID(string raw, out int trackID)
    {
        if (IsEmptyOrNA(raw))
        {
            trackID = 0;
            return false;
        }
        return int.TryParse(raw.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out trackID);
    }

    // Normalizes a string for fuzzy enum matching: lowercases, strips
    // whitespace/punctuation, converts German umlauts to ASCII digraphs
    // (ä->ae, ö->oe, ü->ue, ß->ss).
    public static string Normalize(string s)
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
    public static T MatchEnum<T>(string raw, T defaultValue) where T : struct, Enum
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

    // Resolves a Scenario directly from a Sz.-Nr. number, since the Scenario
    // enum's declaration order matches the Sz.-Nr. numbering (None=0, Ruhig=1, ...).
    public static Scenario ResolveScenarioByNumber(string scenarioNrRaw, string logPrefix, int rowNumber, int trackID)
    {
        int scenarioNr = ParseLeadingInt(scenarioNrRaw);
        if (Enum.IsDefined(typeof(Scenario), scenarioNr))
            return (Scenario)scenarioNr;

        Debug.LogWarning($"[{logPrefix}] Sz.-Nr. '{scenarioNrRaw}' (row {rowNumber}, TrackID {trackID}) does not map to any Scenario enum value. Defaulting to None.");
        return Scenario.None;
    }

    public static Composer ResolveComposer(string composerRaw, string logPrefix, int rowNumber, int trackID)
    {
        Composer composer = MatchEnum(composerRaw, Composer.other);
        if (composer == Composer.other && !string.IsNullOrEmpty(composerRaw))
            Debug.LogWarning($"[{logPrefix}] Could not map composer '{composerRaw}' (row {rowNumber}, TrackID {trackID}). Defaulting to other.");
        return composer;
    }

    public static string MakeSafeFileName(string name)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
            name = name.Replace(c, '_');
        return name.Replace('\n', '_').Replace('\r', '_').Trim();
    }

    // Locates the header row dynamically (the row that contains "Titel"),
    // since these sheets have an extra merged-cell row above the real header.
    public static int FindHeaderRow(List<string[]> rows)
    {
        return rows.FindIndex(r => r.Any(c => c.Trim().Equals("Titel", StringComparison.OrdinalIgnoreCase)));
    }

    public static void EnsureFolder(string folder)
    {
        if (!AssetDatabase.IsValidFolder(folder))
        {
            Directory.CreateDirectory(folder);
            AssetDatabase.Refresh();
        }
    }

    public static string OpenCsvPanelAndReadRows(string title, out List<string[]> rows)
    {
        rows = null;
        string path = EditorUtility.OpenFilePanel(title, Application.dataPath, "csv");
        if (string.IsNullOrEmpty(path))
            return null;

        string csvText = File.ReadAllText(path, Encoding.UTF8);
        rows = ParseCsv(csvText);
        return path;
    }

    // Minimal CSV parser supporting quoted fields, escaped quotes (""),
    // commas inside quotes, and newlines inside quoted fields.
    public static List<string[]> ParseCsv(string text)
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
