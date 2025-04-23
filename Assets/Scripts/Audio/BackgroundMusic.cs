using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{ // script reference: https://www.youtube.com/watch?v=63BEZMjcegE

    public static BackgroundMusic instance;

    // Using this function to load the script
    private void Awake() 
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else 
        {
            Destroy(gameObject);
        }
    }
}
