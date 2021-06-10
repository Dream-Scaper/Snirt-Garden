using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulidboardSprite : MonoBehaviour
{
    public Transform CameraTransform;

    void Update()
    {
        gameObject.transform.LookAt(CameraTransform);
    }
}
