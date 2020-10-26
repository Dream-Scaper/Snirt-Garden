using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiscButtonFunctions : MonoBehaviour
{
    public Animator[] animators;
    public string[] parameterNames;

    public void ToggleAnimBools()
    {
        for (int i = 0; i < animators.Length; i++)
        {
            animators[i].SetBool(parameterNames[i], !animators[i].GetBool(parameterNames[i]));
        }
    }
}
