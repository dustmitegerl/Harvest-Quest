using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFullScreen : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Screen.fullScreen = !Screen.fullScreen;
            Debug.Log("F pressed; window is " + Screen.fullScreen);
        }
    }
}
