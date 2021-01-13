using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModPart : MonoBehaviour
{
    public SnirtEditorManager manager;
    public int part;

    public GameObject buttonParent;
    public GameObject PartChangePrefab;

    public List<Button> partButtons;

    public TextMeshProUGUI readout;
    public string readoutDefault = "Style - ";

    public void AddUIButton(PartListSO partList, int index)
    {
        GameObject newButton = Instantiate(PartChangePrefab, buttonParent.gameObject.transform);
        newButton.GetComponent<Button>().onClick.RemoveAllListeners();
        newButton.GetComponent<Button>().onClick.AddListener(delegate { ChangePart(index); });

        newButton.GetComponent<Image>().sprite = partList.Parts[index].partMenuImage;
        partButtons.Add(newButton.GetComponent<Button>());
    }

    public void UpdateUI(int value, string readoutValue)
    {
        for (int i = 0; i < partButtons.Count; i++)
        {
            // We only want the part to be interactable if it isnt the currently equipt one.
            partButtons[i].interactable = i != value;
        }

        readout.text = readoutDefault + readoutValue;
    }

    public void ChangePart(int changeTo)
    {
        manager.ChangePart(changeTo, part, true);
    }
}
