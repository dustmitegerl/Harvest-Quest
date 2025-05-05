using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue 
{ //Reference to script: https://www.youtube.com/watch?v=_nRzoTzeyxU&t=407s
   public string name;

   [TextArea(3, 10)]
   public string[] sentences;
}
