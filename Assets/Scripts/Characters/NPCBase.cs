using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBase : ScriptableObject
{
    [SerializeField] new string name;

    [SerializeField] string description;
    [SerializeField] Sprite backSprite;
}
