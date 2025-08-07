using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasCam : MonoBehaviour
{
    public Canvas canvas;

    private void Update()
    {
        if (canvas.worldCamera == null)
        {
            canvas.worldCamera = Camera.main;
        }
        if (canvas.worldCamera != null)
        {
            canvas.planeDistance = 333;
        }
    }
}
