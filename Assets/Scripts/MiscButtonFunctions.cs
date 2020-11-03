using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MiscButtonFunctions : MonoBehaviour
{
    public Animator[] animators;
    public string[] parameterNames;

    public ScrollRect[] scrolls;
    public Slider sensitivtySlider;
    public TMP_InputField sensitivtyInput;

    public void ToggleAnimBools()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetBool(parameterNames[i], !animators[i].GetBool(parameterNames[i]));
        }
    }

    public void ChangeSensitivity(float newValue)
    {
        sensitivtyInput.text = newValue.ToString();

        foreach (ScrollRect scroll in scrolls)
        {
            scroll.scrollSensitivity = newValue;
        }
    }

    public void ChangeSensitivity(string newValue)
    {
        if (float.TryParse(newValue, out float FLNewValue))
        {
            sensitivtySlider.value = FLNewValue;

            foreach (ScrollRect scroll in scrolls)
            {
                scroll.scrollSensitivity = FLNewValue;
            }
        }
    }
}
