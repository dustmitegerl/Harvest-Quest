using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Credits : MonoBehaviour
{ //Reference to script: https://www.youtube.com/watch?v=Eeee4TU69x4

    public float scrollSpeed = 40f;
    private RectTransform rectTransform;
    
    // Start is called before the first frame update
    void Start()
    { // Retreiving the RecTransform component 
        rectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {   // Moving the text upwards
        rectTransform.anchoredPosition += new Vector2(0, scrollSpeed * Time.deltaTime);
    }
}
