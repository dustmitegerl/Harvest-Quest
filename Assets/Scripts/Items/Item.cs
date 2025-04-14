using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : ScriptableObject
{
    [SerializeField] string mname;
    [SerializeField] string description;
    [SerializeField] Sprite icon;

    public string Name => mname;
    public string Description => description;
    public Sprite Icon => icon;
}
