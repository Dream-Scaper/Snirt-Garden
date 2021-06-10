using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SavedSnirtUI : MonoBehaviour
{
    public TextMeshProUGUI snirtName;
    public Button loadButton;
    public Button deleteButton;
    public Button deleteCODAButton;
    public Image[] colorSwatches;

    public SnirtEditorManager manager;

    public void UpdateUI(string snirtNameTex, int index, Color[] colors, SnirtEditorManager snirtMan)
    {
        manager = snirtMan;

        snirtName.text = snirtNameTex;

        for (int i = 0; i < colorSwatches.Length; i++)
        {
            colorSwatches[i].color = colors[i];
        }

        loadButton.onClick.RemoveAllListeners();
        loadButton.onClick.AddListener(delegate { manager.LoadSnirt(index, true, true); });

        // The first Snirt in the list can never be deleted in-game.
        if (index != 0)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(delegate { manager.DeleteSnirt(index); });
        }
        else
        {
            Destroy(deleteCODAButton.gameObject);
        }
    }
}