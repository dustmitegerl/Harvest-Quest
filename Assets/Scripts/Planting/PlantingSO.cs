using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable object/PlantingSO")]
public class PlantingSO : ScriptableObject
{
    public Sprite stage1;
    public Sprite stage2;
    public Sprite stage3;
    public Sprite stage4;
    [Header("days til next stage")]
    public int[] stageSpans;
}