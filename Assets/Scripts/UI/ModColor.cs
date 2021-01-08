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
    public string hexEntryPlaceholder = "Hex...";

    public Slider[] sliders;

    public void UpdateUI(Color updateTo)
    {
        colorSwatch.color = updateTo;
        EditSliders(updateTo);

        DisplayPlaceholderText();
    }

    public void EditColor()
    {
        manager.ChangeColor(new Color(sliders[0].value, sliders[1].value, sliders[2].value, 1.0f), color);
    }

    public void EditColorViaHex()
    {
        manager.ChangeColorViaHex(hexEntry.text, color);
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
        manager.ChangeColor(randyColor, color);
    }

    public void ResetColors()
    {
        manager.ChangeColor(startingColor, color);
    }

    public void DisplayCurrentColor()
    {
        hexEntry.text = ColorUtility.ToHtmlStringRGB(colorSwatch.color);
    }

    public void DisplayPlaceholderText()
    {
        hexEntry.placeholder.GetComponent<TextMeshProUGUI>().text = hexEntryPlaceholder + " " + ColorUtility.ToHtmlStringRGB(colorSwatch.color);
    }
}