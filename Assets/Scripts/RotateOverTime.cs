using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateOverTime : MonoBehaviour
{
    public Vector3 speed;

    void Update()
    {
        gameObject.transform.Rotate(speed * Time.deltaTime, Space.World);
    }
}
