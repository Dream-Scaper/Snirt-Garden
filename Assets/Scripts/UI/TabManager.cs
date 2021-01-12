using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabManager : MonoBehaviour
{
    public GameObject[] tabContent;
    public Button[] tabButtons;

    private void Start()
    {
        ToggleActiveTab(0);
    }

    public void ToggleActiveTab(int tabIndex)
    {
        for (int i = 0; i < tabContent.Length; i++)
        {
            if (i == tabIndex)
            {
                tabContent[i].SetActive(true);
                tabButtons[i].interactable = false;
                continue;
            }

            tabContent[i].SetActive(false);
            tabButtons[i].interactable = true;
        }
    }
}
