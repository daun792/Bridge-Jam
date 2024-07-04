using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraFlip : MonoBehaviour
{
    void Start()
    {
        Camera cam = GetComponent<Camera>();

        Matrix4x4 mat = cam.projectionMatrix;

        mat *= Matrix4x4.Scale(new Vector3(1, -1, 1));

        cam.projectionMatrix = mat;
    }
}
