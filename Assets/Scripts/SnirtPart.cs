using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Snirt Part", menuName = "SnirtSim/Part")]
public class SnirtPart : ScriptableObject
{
    public string partName;
    public Sprite partMenuImage;
    public Mesh partMesh;
}