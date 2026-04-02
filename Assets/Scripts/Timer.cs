using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using UnityEngine;

public static class Timer
{
    private static readonly Stopwatch stopwatch = new();
    private static readonly List<long> steps = new();

    public static bool IsRunning
    {
        get => stopwatch.IsRunning;
    }

    public static double ElapsedSeconds
    {
        get => stopwatch.ElapsedMilliseconds * 0.001f;
    }

    public static int StepsCount
    {
        get => steps.Count;
    }

    public static double GetStepElapsedSeconds(int index)
    {
        return steps[index] * 0.001f;
    }

    /// <summary>
    /// Reset the timer and remove any steps.
    /// </summary>
    public static void Reset()
    {
        stopwatch.Reset();
        steps.Clear();
    }

    public static void Start()
    {
        stopwatch.Start();
    }

    public static void Stop()
    {
        stopwatch.Stop();
    }

    public static void Step()
    {
        steps.Add(stopwatch.ElapsedMilliseconds);
    }

    public static void Save()
    {
        // TODO : save our time steps (line 7 of this script) inside a file.
        string path = Path.Combine(Application.persistentDataPath, "timesteps.dat");

        // Check if a save file exists and compare the final time
        if (File.Exists(path))
        {
            byte[] existingBytes = Convert.FromBase64String(File.ReadAllText(path));
            using MemoryStream existingMs = new(existingBytes);
            using BinaryReader reader = new(existingMs);
            int count = reader.ReadInt32();
            for (int i = 0; i < count - 1; i++)
                reader.ReadInt64();
            long savedBestTime = reader.ReadInt64();

            if (steps.Last() >= savedBestTime)
            {
                UnityEngine.Debug.Log($"[Timer] No new highscore ({steps.Last() * 0.001f}s >= {savedBestTime * 0.001f}s), skipping save.");
                return;
            }
        }

        using MemoryStream ms = new();
        using BinaryWriter writer = new(ms);
        writer.Write(steps.Count);
        foreach (long step in steps)
            writer.Write(step);
        File.WriteAllText(path, Convert.ToBase64String(ms.ToArray()));

        UnityEngine.Debug.Log($"[Timer] New highscore! Saved {steps.Count} steps to {path}");
    }

    public static void Load()
    {
        // TODO : load our time steps from a file (if we have any)
        // and store them inside our steps variable (line 7 of this script)
        // to show them to the player before starting a race.
        string path = Path.Combine(Application.persistentDataPath, "timesteps.dat");
        if (!File.Exists(path)) return;

        byte[] bytes = Convert.FromBase64String(File.ReadAllText(path));
        using MemoryStream ms = new(bytes);
        using BinaryReader reader = new(ms);
        steps.Clear();
        int count = reader.ReadInt32();
        for (int i = 0; i < count; i++)
            steps.Add(reader.ReadInt64());
    }

    public static void DeleteSave()
    {
        string path = Path.Combine(Application.persistentDataPath, "timesteps.dat");
        if (!File.Exists(path))
        {
            UnityEngine.Debug.Log("[Timer] No save file found, nothing to delete.");
            return;
        }

        File.Delete(path);
        UnityEngine.Debug.Log("[Timer] Save file deleted, score has been reset.");
    }
}
