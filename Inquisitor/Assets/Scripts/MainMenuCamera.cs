using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCamera : MonoBehaviour
{
    public float spin = 10.0f;

    void Update()
    {
        // rotate the camera spinº every second
        transform.Rotate(0.0f, spin * Time.deltaTime, 0.0f, Space.Self);
    }
}
