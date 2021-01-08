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
    public GameObject[] PartGameObjects;

    [Header("Editable Materials")]
    public Material[] PartMaterials;

    [Header("Part Lists")]
    public PartListSO Crests;
    public PartListSO Frills;
    public PartListSO Tails;
    public PartListSO Patterns;

    private SnirtPartSO[][] AllParts;
    private int[] activeParts = new int[4];

    [Header("UI Elements")]
    public TMP_InputField nameInput;
    public ModPart[] PartDropdowns;
    public ModColor[] ColorSliders;
    public ScaleUIToChildren SavedSnirtsMenu;

    [Header("Required Prefabs")]
    public GameObject SaveFileUI;

    private void Awake()
    {
        // Set up dropdowns to be fully populated with part options.
        AllParts = new SnirtPartSO[][] { Crests.Parts, Frills.Parts, Tails.Parts, Patterns.Parts };

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
        // Load from file. 
        SnirtSaveLoader.LoadFile();

        PopulateSaveUI();
    }

    public void LoadSnirt(int index)
    {
        string snirtData = SnirtSaveLoader.savedSnirts[index];

        // Use the array created by the inital loading of the file.
        // Set all values to the snirt at the index and update all ui.
        string[] snirtTraits = snirtData.Split(',');

        ChangeName(snirtTraits[0]);

        for (int i = 0; i < PartGameObjects.Length; i++)
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

        for (int i = 0; i < PartMaterials.Length; i++)
        {
            ChangeColorViaHex(snirtTraits[i + 5], (SnirtColors)i);
        }
    }

    public void DeleteSnirt(int index)
    {
        // Just a wrapper function.
        SnirtSaveLoader.DeleteSnirt(index);

        // Reload UI.
        PopulateSaveUI();
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

        for (int i = 0; i < PartMaterials.Length; i++)
        {
            snirtTraits += "," + ColorUtility.ToHtmlStringRGB(PartMaterials[i].GetColor("_BaseColor"));
        }

        SnirtSaveLoader.SaveSnirt(snirtTraits);

        PopulateSaveUI();
    }

    private void PopulateSaveUI()
    {
        SavedSnirtsMenu.ClearChildren();

        // Create Save UI as many times as there are lines.
        for (int i = 0; i < SnirtSaveLoader.savedSnirts.Count; i++)
        {
            CreateSaveUI(SnirtSaveLoader.savedSnirts[i], i);
        }
    }

    public void CreateSaveUI(string snirtData, int index)
    {
        // Create a new snirtSave UI Element
        GameObject newSaveSlot = Instantiate(SaveFileUI, SavedSnirtsMenu.gameObject.transform);

        // Child the object to the Saved Snirts window.
        SavedSnirtsMenu.AddChild(newSaveSlot);

        // Update its UI with the proper values.
        string[] snirtTraits = snirtData.Split(',');

        List<Color> snirtColors = new List<Color>();

        for (int i = 0; i < PartMaterials.Length; i++)
        {
            ColorUtility.TryParseHtmlString("#" + snirtTraits[i + 5], out Color newCol);
            snirtColors.Add(newCol);
        }

        newSaveSlot.GetComponent<SavedSnirtUI>().UpdateUI(snirtTraits[0], index, snirtColors.ToArray(), this);
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
        if (PartGameObjects[(int)part].TryGetComponent(out MeshFilter meshF))
        {
            meshF.sharedMesh = AllParts[(int)part][Mathf.Min(activeParts[(int)part], AllParts[(int)part].Length - 1)].partMesh;
        }
        PartDropdowns[(int)part].UpdateUI(activeParts[(int)part]);
    }
    #endregion

    #region Color
    public void ChangeColor(Color changeTo, SnirtColors color)
    {
        PartMaterials[(int)color].SetColor("_BaseColor", changeTo);
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
