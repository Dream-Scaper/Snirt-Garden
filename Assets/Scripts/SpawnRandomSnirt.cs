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

    void Start()
    {
        SnirtSaveLoader.LoadFile();

        foreach (Transform spawnPoint in spawnLocations)
        {
            int randy = UnityEngine.Random.Range(0, SnirtSaveLoader.savedSnirts.Count);

            string[] snirtTraits = SnirtSaveLoader.savedSnirts[randy].Split(',');

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
}
