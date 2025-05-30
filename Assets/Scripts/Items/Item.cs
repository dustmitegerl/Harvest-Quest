using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField] string names;
    [SerializeField] string description;
    [SerializeField] Sprite icon;

    public string Name => names;
    public string Description => description;
    public Sprite Icon => icon;
}
