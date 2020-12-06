using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModPart : MonoBehaviour
{
    public SnirtEditorManager manager;
    public SnirtEditorManager.SnirtParts part;

    public TMP_Dropdown partsDropdown;

    public void ChangeDropDownOptions(List<string> newOptions)
    {
        partsDropdown.ClearOptions();
        partsDropdown.AddOptions(newOptions);
    }

    public void UpdateUI(int dropdownValue)
    {
        partsDropdown.value = dropdownValue;
    }

    public void ChangePart(int changeTo)
    {
        manager.ChangePart(changeTo, part);
    }
}
