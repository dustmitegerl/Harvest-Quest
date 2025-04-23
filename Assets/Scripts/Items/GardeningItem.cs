using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Create new gardening")]
public class GardeningItem : Item
{
    [SerializeField] bool Fertillizers;
    [SerializeField] bool Seeds;
    [SerializeField] bool Plants;
    [SerializeField] bool Tools;
}
