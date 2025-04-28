using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/PlantingSO")]
public class PlantingSO : ScriptableObject
{
    [Header("Sprites for each stage")]
    public Sprite[] stageSprites;
    [Header("days til next stage")]
    public int[] stageSpans;
}