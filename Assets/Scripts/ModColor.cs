using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModColor : MonoBehaviour
{
    public SnirtEditorManager manager;
    public SnirtEditorManager.SnirtColors color;

    public Image colorSwatch;

    public Color startingColor;

    public TMP_InputField hexEntry;

    public Slider[] sliders;

    private void Awake()
    {
        ResetColors();
    }

    public void UpdateUI(Color updateTo)
    {
        colorSwatch.color = updateTo;
        EditSliders(updateTo);
    }

    public void EditColor()
    {
        manager.EditColor(new Color(sliders[0].value, sliders[1].value, sliders[2].value, 1.0f), color);
    }

    public void EditColorViaHex()
    {
        manager.EditColorViaHex(hexEntry.text, color);
        hexEntry.text = "";
    }

    public void EditSliders(Color setColor)
    {
        sliders[0].value = setColor.r;
        sliders[1].value = setColor.g;
        sliders[2].value = setColor.b;
    }

    public void RandomColor()
    {
        Color randyColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        manager.EditColor(randyColor, color);
    }

    public void ResetColors()
    {
        manager.EditColor(startingColor, color);
    }
}