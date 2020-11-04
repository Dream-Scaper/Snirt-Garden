using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpawnRandomSnirt : MonoBehaviour
{
    public GameObject snirtPrefab;
    public Vector3 spawnSize;

    public PartListSO Crests;
    public PartListSO Frills;
    public PartListSO Tails;
    public PartListSO Patterns;

    public Transform[] spawnLocations;

    private string savePath;
    private List<string> savedSnirts;
    private const string defaultSnirt = "Snirt,0,0,0,0,D4D4D4,808080,353535,000000";

    void Start()
    {
        savePath = Application.persistentDataPath + "/savedSnirts.txt";
        savedSnirts = new List<string>();

        LoadFile();

        foreach (Transform spawnPoint in spawnLocations)
        {
            int randy = UnityEngine.Random.Range(0, savedSnirts.Count);

            string[] snirtTraits = savedSnirts[randy].Split(',');

            SpawnedSnirt currentSnirt = Instantiate(snirtPrefab, spawnPoint.position, spawnPoint.rotation).GetComponent<SpawnedSnirt>();

            currentSnirt.ChangeSize(spawnSize);

            currentSnirt.ChangeParts(Crests.Parts[int.Parse(snirtTraits[1])].partMesh, Frills.Parts[int.Parse(snirtTraits[2])].partMesh, Tails.Parts[int.Parse(snirtTraits[3])].partMesh, Patterns.Parts[int.Parse(snirtTraits[4])].partMesh);

            Color[] snirtColors = new Color[4];
            for (int i = 0; i < 4; i++)
            {
                bool success = ColorUtility.TryParseHtmlString("#" + snirtTraits[i + 5], out Color newCol);
                if (success)
                {
                    snirtColors[i] = newCol;
                }
            }

            currentSnirt.ChangeMaterials(snirtColors[0], snirtColors[1], snirtColors[2], snirtColors[3]);
        }
    }

    public void LoadFile()
    {
        savedSnirts.Clear();

        try
        {
            using (StreamReader reader = new StreamReader(savePath))
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
            File.WriteAllText(savePath, defaultSnirt + Environment.NewLine);
            LoadFile();
        }
    }


}
