using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnedSnirt : MonoBehaviour
{
    public Material originalBodyMat;
    public Material originalCrestsMat;
    public Material originalPatternMat;
    public Material originalEyesMat;

    private Material BodyMat;
    private Material CrestsMat;
    private Material PatternMat;
    private Material EyesMat;

    public MeshFilter Crest;
    public MeshFilter Frill;
    public MeshFilter Tail;
    public MeshFilter Pattern;

    public MeshRenderer[] BodyParts;
    public MeshRenderer[] CrestParts;
    public MeshRenderer[] PatternParts;
    public MeshRenderer[] EyeParts;

    public void ChangeSize(Vector3 newScale)
    {
        gameObject.transform.localScale = newScale;
    }

    public void ChangeMaterials(Color newBodyColor, Color newCrestColor, Color newPatternColor, Color newEyeColor)
    {
        BodyMat = new Material(originalBodyMat);
        BodyMat.SetColor("_BaseColor", newBodyColor);

        CrestsMat = new Material(originalCrestsMat);
        CrestsMat.SetColor("_BaseColor", newCrestColor);

        PatternMat = new Material(originalPatternMat);
        PatternMat.SetColor("_BaseColor", newPatternColor);

        EyesMat = new Material(originalEyesMat);
        EyesMat.SetColor("_BaseColor", newEyeColor);

        foreach (MeshRenderer mr in BodyParts)
        {
            mr.sharedMaterial = BodyMat;
        }

        foreach (MeshRenderer mr in CrestParts)
        {
            mr.sharedMaterial = CrestsMat;
        }

        foreach (MeshRenderer mr in PatternParts)
        {
            mr.sharedMaterial = PatternMat;
        }

        foreach (MeshRenderer mr in EyeParts)
        {
            mr.sharedMaterial = EyesMat;
        }
    }

    public void ChangeParts(Mesh newCrest, Mesh newFrill, Mesh newTail, Mesh newPattern)
    {
        Crest.sharedMesh = newCrest;
        Frill.sharedMesh = newFrill;
        Tail.sharedMesh = newTail;
        Pattern.sharedMesh = newPattern;
    }
}
