using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class SnirtEditorManager : MonoBehaviour
{
    public enum SnirtParts { CREST, FRILL, TAIL, PATTERN }
    public enum SnirtColors { BODY, CRESTS, PATTERN, EYES }

    public string snirtName;
    public string[] randomName;

    [Header("Snirt Game Object Parts")]
    public GameObject[] CrestFrillTailPattern;

    [Header("Editable Materials")]
    public Material[] BodyCrestsPatternEyes;

    [Header("Part Lists")]
    public SnirtPart[] Crests;
    public SnirtPart[] Frills;
    public SnirtPart[] Tails;
    public SnirtPart[] Patterns;

    private SnirtPart[][] AllParts;
    private int[] activeParts = new int[4];

    [Header("UI Elements")]
    public TMP_InputField nameInput;
    public ModPart[] PartDropdowns;
    public ModColor[] ColorSliders;
    public ScaleUIToChildren SavedSnirtsMenu;

    [Header("Required Prefabs")]
    public GameObject SaveFileUI;

    private string savePath;
    private List<string> savedSnirts;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savedSnirts.txt";
        savedSnirts = new List<string>();

        // Set up dropdowns to be fully populated with part options.
        AllParts = new SnirtPart[][] { Crests, Frills, Tails, Patterns };

        for (int i = 0; i < AllParts.Length; i++)
        {
            List<string> partNames = new List<string>();

            for (int j = 0; j < AllParts[i].Length; j++)
            {
                partNames.Add(AllParts[i][j].partName);
            }

            PartDropdowns[i].ChangeDropDownOptions(partNames);
        }
    }

    #region Save/Load
    public void LoadFile()
    {
        try
        {
            StreamReader reader = new StreamReader(savePath);
            while (!reader.EndOfStream)
            {
                savedSnirts.Add(reader.ReadLine());
            }
            reader.Close();
        }
        catch
        {
            // Default a new save to just the default Snirt.
            Debug.LogWarning("No save file found! Creating new save...");
            File.WriteAllText(savePath, "Snirt,0,0,0,0,D4D4D4,808080,353535,000000");
            LoadFile();
        }

        // Create Save UI as many times as there are lines.
    }

    public void LoadSnirt(int index)
    {
        // Use the array created by the inital loading of the file.
        // Set all values to the snirt at the index and update all ui.
    }

    public void SaveSnirt()
    {   // Make a string out of all the snirt properties.
        // Save it to the end of the save file.
        // Create new Save File UI
    }

    public void CreateSaveUI()
    {
        // Create a new snirtSave UI Element
        // Update its UI with the proper values.
        // Child the object to the Saved Snirts window.
    }
    #endregion

    #region Name
    public void ChangeName(string newName)
    {
        snirtName = newName;
    }

    public void RandomizeName()
    {
        int randy = UnityEngine.Random.Range(0, randomName.Length - 1);
        nameInput.text = randomName[randy];
    }
    #endregion

    #region Part
    public void ChangePart(int changeTo, SnirtParts part)
    {
        activeParts[(int)part] = changeTo;
        if (CrestFrillTailPattern[(int)part].TryGetComponent(out MeshFilter meshF))
        {
            meshF.sharedMesh = AllParts[(int)part][Mathf.Min(activeParts[(int)part], AllParts[(int)part].Length - 1)].partMesh;
        }
        PartDropdowns[(int)part].UpdateUI(activeParts[(int)part]);
    }
    #endregion

    #region Color
    public void EditColor(Color changeTo, SnirtColors color)
    {
        BodyCrestsPatternEyes[(int)color].SetColor("_BaseColor", changeTo);
        ColorSliders[(int)color].UpdateUI(changeTo);
    }

    public void EditColorViaHex(string changeTo, SnirtColors color)
    {
        bool success = ColorUtility.TryParseHtmlString("#" + changeTo, out Color newCol);
        if (success)
        {
            EditColor(newCol, color);
        }
    }
    #endregion
}
