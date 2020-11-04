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
    //public SnirtPart[] Crests;
    //public SnirtPart[] Frills;
    //public SnirtPart[] Tails;
    //public SnirtPart[] Patterns;

    public PartListSO Crests;
    public PartListSO Frills;
    public PartListSO Tails;
    public PartListSO Patterns;

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
    private const string defaultSnirt = "Snirt,0,0,0,0,D4D4D4,808080,353535,000000";

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/savedSnirts.txt";
        savedSnirts = new List<string>();

        // Set up dropdowns to be fully populated with part options.
        AllParts = new SnirtPart[][] { Crests.Parts, Frills.Parts, Tails.Parts, Patterns.Parts };

        for (int i = 0; i < AllParts.Length; i++)
        {
            List<string> partNames = new List<string>();

            for (int j = 0; j < AllParts[i].Length; j++)
            {
                partNames.Add(AllParts[i][j].partName);
            }

            PartDropdowns[i].ChangeDropDownOptions(partNames);
        }

        LoadFile();
        LoadSnirt(0);
    }

    #region Save/Load
    public void LoadFile()
    {
        savedSnirts.Clear();
        SavedSnirtsMenu.ClearChildren();

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

        // Create Save UI as many times as there are lines.
        for (int i = 0; i < savedSnirts.Count; i++)
        {
            CreateSaveUI(i);
        }
    }

    public void LoadSnirt(int index)
    {
        // Use the array created by the inital loading of the file.
        // Set all values to the snirt at the index and update all ui.
        string[] snirtTraits = savedSnirts[index].Split(',');

        ChangeName(snirtTraits[0]);

        for (int i = 0; i < CrestFrillTailPattern.Length; i++)
        {
            if (int.TryParse(snirtTraits[i + 1], out int part))
            {
                ChangePart(part, (SnirtParts)i);
            }
            else
            {
                ChangePart(0, (SnirtParts)i);
            }
        }

        for (int i = 0; i < BodyCrestsPatternEyes.Length; i++)
        {
            ChangeColorViaHex(snirtTraits[i + 5], (SnirtColors)i);
        }
    }

    public void DeleteSnirt(int index)
    {
        // Delete the entry in savedSnirts
        savedSnirts.RemoveAt(index);

        // Re-write save file.
        File.WriteAllText(savePath, "");
        try
        {
            using (StreamWriter writer = File.AppendText(savePath))
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

    public void SaveSnirt()
    {   
        // Make a string out of all the snirt properties.
        string snirtTraits = snirtName.Replace(',', ' '); // Just in case there are any commas, replace them with spaces.

        if (snirtTraits == "")
        {
            snirtTraits = "Unnamed";
        }

        for (int i = 0; i < activeParts.Length; i++)
        {
            snirtTraits += "," + activeParts[i].ToString();
        }

        for (int i = 0; i < BodyCrestsPatternEyes.Length; i++)
        {
            snirtTraits += "," + ColorUtility.ToHtmlStringRGB(BodyCrestsPatternEyes[i].GetColor("_BaseColor"));
        }

        // Save it to the end of the save file.
        try
        {
            using (StreamWriter writer = File.AppendText(savePath))
            {
                writer.WriteLine(snirtTraits);
            }
        }
        catch
        {
            Debug.LogError("Save failed! Could you be missing a save file?");
        }

        savedSnirts.Add(snirtTraits);

        // Reload to update the UI
        LoadFile();
    }

    public void CreateSaveUI(int saveLine)
    {
        // Create a new snirtSave UI Element
        GameObject newSaveSlot = Instantiate(SaveFileUI, SavedSnirtsMenu.gameObject.transform);

        // Child the object to the Saved Snirts window.
        SavedSnirtsMenu.AddChild(newSaveSlot);

        // Update its UI with the proper values.
        string[] snirtTraits = savedSnirts[saveLine].Split(',');

        List<Color> snirtColors = new List<Color>();

        for (int i = 0; i < BodyCrestsPatternEyes.Length; i++)
        {
            ColorUtility.TryParseHtmlString("#" + snirtTraits[i + 5], out Color newCol);
            snirtColors.Add(newCol);
        }

        newSaveSlot.GetComponent<SavedSnirtUI>().UpdateUI(snirtTraits[0], saveLine, snirtColors.ToArray(), this);
    }
    #endregion

    #region Name
    public void ChangeName(string newName)
    {
        snirtName = newName;
        nameInput.text = newName;
    }

    public void RandomizeName()
    {
        int randy = UnityEngine.Random.Range(0, randomName.Length);
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
    public void ChangeColor(Color changeTo, SnirtColors color)
    {
        BodyCrestsPatternEyes[(int)color].SetColor("_BaseColor", changeTo);
        ColorSliders[(int)color].UpdateUI(changeTo);
    }

    public void ChangeColorViaHex(string changeTo, SnirtColors color)
    {
        bool success = ColorUtility.TryParseHtmlString("#" + changeTo, out Color newCol);
        if (success)
        {
            ChangeColor(newCol, color);
        }
    }
    #endregion
}
