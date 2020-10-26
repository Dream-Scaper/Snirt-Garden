using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RandomName : MonoBehaviour
{
    public TMP_InputField nameInput;

    public string[] names;

    public void RandomizeName()
    {
        int randy = Random.Range(0, names.Length - 1);
        nameInput.text = names[randy];
    }
}
