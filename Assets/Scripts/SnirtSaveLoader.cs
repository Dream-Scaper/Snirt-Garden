using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SnirtSaveLoader : MonoBehaviour
{
    public static List<string> savedSnirts;
    private const string defaultSnirt = "Snirt,0,0,0,0,808080,808080,808080,353535,D4D4D4,000000";
    public static string SavePath()
    {
        return Application.persistentDataPath + "/savedSnirts.txt";
    }

    public static void LoadFile()
    {
        savedSnirts = new List<string>();
        savedSnirts.Clear();

        try
        {
            using (StreamReader reader = new StreamReader(SavePath()))
            {
                while (!reader.EndOfStream)
                {
                    savedSnirts.Add(reader.ReadLine());
                }
            }
        }
        catch
        {
            // Default a new save to just the default Snirt.
            Debug.LogWarning("No save file found! Creating new save...");
            File.WriteAllText(SavePath(), defaultSnirt + Environment.NewLine);
            LoadFile();
        }
    }

    public static void SaveSnirt(string snirtData)
    {
        // Save it to the end of the save file.
        try
        {
            using (StreamWriter writer = File.AppendText(SavePath()))
            {
                writer.WriteLine(snirtData);
            }
        }
        catch
        {
            // The save file will be caught and created in the LoadFile() after this if this occurs.
            Debug.LogError("Save failed! Could you be missing a save file?");
        }

        // Reload to update the UI
        LoadFile();
    }

    public static void DeleteSnirt(int index)
    {
        // Delete the entry in savedSnirts
        savedSnirts.RemoveAt(index);

        // Re-write save file.
        File.WriteAllText(SavePath(), "");
        try
        {
            using (StreamWriter writer = File.AppendText(SavePath()))
            {
                foreach (string save in savedSnirts)
                {
                    writer.WriteLine(save);
                }
            }
        }
        catch
        {
            Debug.LogError("Save failed! Could you be missing a save file?");
        }

        LoadFile();
    }
}
