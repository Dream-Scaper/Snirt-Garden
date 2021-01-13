using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ModColor : MonoBehaviour
{
    public SnirtEditorManager manager;
    public int color;

    [Header("UI Elements")]
    public Image colorSwatch;

    public TMP_InputField hexEntry;
    public string hexEntryPlaceholder = "Hex...";

    public Slider[] sliders;

    [Header("Default and Palette Colors")]
    public Color startingColor;

    public GameObject paletteHolder;
    public GameObject colorPaletteButtonPrefab;

    public Color[] paletteColors;

    private void Awake()
    {
        for (int i = 0; i < paletteColors.Length; i++)
        {
            CreatePaletteButton(i);
        }
    }

    public void UpdateUI(Color updateTo)
    {
        colorSwatch.color = updateTo;
        EditSliders(updateTo);

        DisplayPlaceholderText();
    }

    public void EditSliders(Color setColor)
    {
        sliders[0].value = setColor.r;
        sliders[1].value = setColor.g;
        sliders[2].value = setColor.b;
    }

    public void EditColorViaSliders(bool countAction)
    {
        manager.ChangeColor(new Color(sliders[0].value, sliders[1].value, sliders[2].value, 1.0f), color, countAction);
    }

    public void EditColorViaHex()
    {
        manager.ChangeColorViaHex(hexEntry.text, color, true);
        hexEntry.text = "";
    }

    public void EditColorFromPaletteButton(int colorIndex)
    {
        manager.ChangeColor(paletteColors[colorIndex], color, true);
    }

    public void RandomColor()
    {
        Color randyColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
        manager.ChangeColor(randyColor, color, true);
    }

    public void ResetColors()
    {
        manager.ChangeColor(startingColor, color, true);
    }

    public void DisplayCurrentColor()
    {
        hexEntry.text = ColorUtility.ToHtmlStringRGB(colorSwatch.color);
    }

    public void DisplayPlaceholderText()
    {
        hexEntry.placeholder.GetComponent<TextMeshProUGUI>().text = hexEntryPlaceholder + " " + ColorUtility.ToHtmlStringRGB(colorSwatch.color);
    }

    public void CreatePaletteButton(int colorIndex)
    {
        GameObject newButton = Instantiate(colorPaletteButtonPrefab, paletteHolder.transform);

        newButton.GetComponent<RectTransform>().Rotate(new Vector3(0, 0, Random.Range(0f, 360f)));
        newButton.GetComponent<Image>().color = paletteColors[colorIndex];
        newButton.GetComponent<Button>().onClick.RemoveAllListeners();
        newButton.GetComponent<Button>().onClick.AddListener(delegate { EditColorFromPaletteButton(colorIndex); });
    }

    public void TestMethod()
    {
        Debug.Log("Woah!!");
    }
}