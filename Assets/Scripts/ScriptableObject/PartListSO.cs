using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Parts List", menuName = "SnirtSim/PartList")]
public class PartListSO : ScriptableObject
{
    public SnirtPartSO[] Parts;
}
