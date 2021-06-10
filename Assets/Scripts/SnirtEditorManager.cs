using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class SnirtEditorManager : MonoBehaviour
{
    public string snirtName;
    public string[] randomName;

    [Header("Snirt Game Object Parts")]
    public GameObject[] PartGameObjects;

    [Header("Part Lists")]
    public PartListSO[] partLists;

    private int[] activeParts = new int[4];

    [Header("UI Elements")]
    public TMP_InputField nameInput;
    public ModPart[] PartDropdowns;
    public ModColor[] ColorSliders;
    public ScaleUIToChildren SavedSnirtsMenu;

    [Header("Required Prefabs")]
    public GameObject SaveFileUI;

    [Header("Undo/Redo")]
    public Button undoButton;
    public Button redoButton;

    public int actionIndex;
    public Action lastAction;
    public List<Action> history;

    [System.Serializable]
    public struct Action
    {
        public string snirtData;
    }

    private void Awake()
    {
        history = new List<Action>();

        // Set part buttons to have the proper sprites and function.
        for (int i = 0; i < partLists.Length; i++)
        {
            for (int j = 0; j < partLists[i].Parts.Length; j++)
            {
                PartDropdowns[i].AddUIButton(partLists[i], j);
            }
        }

        LoadFile();
        LoadSnirt(0, true, true);
    }

    #region Save/Load
    public void LoadFile()
    {
        // Load from file. 
        SnirtSaveLoader.LoadFile();

        PopulateSaveUI();
    }

    public void LoadSnirt(int index, bool recordAction, bool clearHistory)
    {
        LoadSnirtFromString(SnirtSaveLoader.savedSnirts[index], recordAction, clearHistory);
    }

    private void LoadSnirtFromString(string snirtData, bool recordAction, bool clearHistory)
    {
        // Use the array created by the inital loading of the file.
        // Set all values to the snirt at the index and update all ui.
        string[] snirtTraits = snirtData.Split(',');

        ChangeName(snirtTraits[0]);

        for (int i = 0; i < activeParts.Length; i++)
        {
            if (int.TryParse(snirtTraits[i + 1], out int part))
            {
                ChangePart(part, i, false);
            }
            else
            {
                ChangePart(0, i, false);
            }

        }

        for (int i = 0; i < PartGameObjects.Length; i++)
        {
            ChangeColorViaHex(snirtTraits[i + 5], i, false);
        }

        if (recordAction)
        {
            if (clearHistory)
            {
                ClearHistory();
            }

            RecordLastAction(SnirtTraits());
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
        SnirtSaveLoader.SaveSnirt(SnirtTraits());

        PopulateSaveUI();
    }

    private string SnirtTraits()
    {
        string snirtTraits = snirtName.Replace(',', ' '); // Just in case there are any commas, replace them with spaces.

        if (snirtTraits == "")
        {
            snirtTraits = "Unnamed";
        }

        for (int i = 0; i < activeParts.Length; i++)
        {
            snirtTraits += "," + activeParts[i].ToString();
        }

        for (int i = 0; i < PartGameObjects.Length; i++)
        {
            if (PartGameObjects[i].TryGetComponent(out MeshRenderer meshR))
            {
                snirtTraits += "," + ColorUtility.ToHtmlStringRGB(meshR.sharedMaterial.GetColor("_BaseColor"));
            }
            else
            {
                if (PartGameObjects[i].TryGetComponent(out SkinnedMeshRenderer skinnedMeshR))
                {
                    snirtTraits += "," + ColorUtility.ToHtmlStringRGB(skinnedMeshR.sharedMaterial.GetColor("_BaseColor"));
                }
            }
        }

        return snirtTraits;
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

        for (int i = 0; i < PartGameObjects.Length; i++)
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
    public void ChangePart(int changeTo, int part, bool recordAction)
    {
        if (part < partLists.Length)
        {
            activeParts[part] = changeTo;
            if (PartGameObjects[part].TryGetComponent(out MeshFilter meshF))
            {
                meshF.sharedMesh = partLists[part].Parts[activeParts[part]].partMesh;
            }

            PartDropdowns[part].UpdateUI(activeParts[part], partLists[part].Parts[activeParts[part]].partName);

            if (recordAction)
            {
                RecordLastAction(SnirtTraits());
            }
        }
    }
    #endregion

    #region Color
    public void ChangeColor(Color changeTo, int part, bool recordAction)
    {
        if (PartGameObjects[part].TryGetComponent(out MeshRenderer meshR))
        {
            meshR.sharedMaterial.SetColor("_BaseColor", changeTo);
        }
        else
        {
            if (PartGameObjects[part].TryGetComponent(out SkinnedMeshRenderer skinnedMeshR))
            {
                skinnedMeshR.sharedMaterial.SetColor("_BaseColor", changeTo);
            }
        }

        ColorSliders[part].UpdateUI(changeTo);

        if (recordAction)
        {
            RecordLastAction(SnirtTraits());
        }
    }

    public void ChangeColorViaHex(string changeTo, int part, bool recordAction)
    {
        bool success = ColorUtility.TryParseHtmlString("#" + changeTo, out Color newCol);
        if (success)
        {
            ChangeColor(newCol, part, recordAction);
        }
    }
    #endregion

    #region UndoRedo
    public void RecordLastAction(string snirtSnapshot)
    {
        Action incomingAction = new Action { snirtData = snirtSnapshot };

        if (!(lastAction.snirtData == incomingAction.snirtData))
        {
            // Increment action index.
            actionIndex = Math.Min(actionIndex + 1, history.Count);

            // Check if the index is at the end of the list.
            // If its not, delete all list entries ahead of this one.
            if (actionIndex < history.Count - 1)
            {
                // If the list is longer than our current index...
                history.RemoveRange(actionIndex, (history.Count - actionIndex));
            }

            // Add last to history.
            history.Add(incomingAction);

            // Make last action the incoming one.
            lastAction = incomingAction;

            // Update the UI.
            UpdateActionButtons();
        }
    }

    public void UndoRedo(bool redo)
    {
        if (redo)
        {
            // Increment the index.
            actionIndex = Math.Min(actionIndex + 1, history.Count - 1);
        }
        else
        {
            // Decrement the index.
            actionIndex = Math.Max(actionIndex - 1, 0);
        }

        // Check if the last action is part or color.
        // Use the action's values in calling changepart/color respectively.
        LoadSnirtFromString(history[actionIndex].snirtData, false, false);

        UpdateActionButtons();
    }

    private void ClearHistory()
    {
        history.Clear();
        actionIndex = 0;

        UpdateActionButtons();
    }

    private void UpdateActionButtons()
    {
        undoButton.interactable = actionIndex != 0;
        redoButton.interactable = actionIndex != history.Count - 1;
    }
    #endregion
}
