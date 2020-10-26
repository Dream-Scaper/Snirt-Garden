using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SavedSnirtUI : MonoBehaviour
{
    public TextMeshProUGUI snirtName;
    public Button loadButton;
    public Image[] colorSwatches;

    public SnirtEditorManager manager;

    public void UpdateUI(string snirtNameTex, int index, Color[] colors, SnirtEditorManager snirtMan)
    {
        manager = snirtMan;

        snirtName.text = snirtNameTex;

        for (int i = 0; i < colors.Length; i++)
        {
            colorSwatches[i].color = colors[i];
        }

        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(delegate { manager.LoadSnirt(index); });
    }
}