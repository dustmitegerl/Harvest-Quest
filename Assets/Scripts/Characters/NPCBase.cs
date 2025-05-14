using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : ScriptableObject
{
    [SerializeField] string name;

    [SerializeField] string description;
    [SerializeField] Sprite backSprite;
}
