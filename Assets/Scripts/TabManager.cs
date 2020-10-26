using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabManager : MonoBehaviour
{
    public GameObject[] tabs;

    private void Start()
    {
        ToggleActiveTab(0);
    }

    public void ToggleActiveTab(int tabIndex)
    {
        for (int i = 0; i < tabs.Length; i++)
        {
            if (i == tabIndex)
            {
                tabs[i].SetActive(true);
                continue;
            }

            tabs[i].SetActive(false);
        }
    }
}
