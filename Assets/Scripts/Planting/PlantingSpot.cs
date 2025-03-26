using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantingSpot : MonoBehaviour, IDropHandler
{
    public string currentPlant; // used to find scriptable object of the plant type's stats
    public int plantStage = 0; // 0 = empty / seed planted; 1-3 = incomplete growth stages; 4 = ready to harvest; 5 = dead 
    bool isGrowing;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    PlantGrowthManager plantInfo;
    PlantingSO currentPlantInfo;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        plantInfo = GetComponentInParent<PlantGrowthManager>();
    }
    void Update()
    {
        plantStage = Mathf.Clamp(plantStage, 0, 5); // limits plant growth stage range 
    }
    public bool IsSpaceEmpty()
    {
        if (transform.childCount == 0)
        {
            return true;
        }
        else return false;
    }
    public void OnDrop(PointerEventData eventData) // allows planting via dragging and dropping seed from inventory
    {
        InventoryItem inventoryItem = eventData.pointerDrag.GetComponent<InventoryItem>();
        Item item = inventoryItem.item;
        if (item.type.ToString() == "Seed")
        {
            Debug.Log("Attempting to plant " + item.name);
            Sow(item.name);
        }
    }
    void Sow(string seedType) // plants seed
    {
        if (IsSpaceEmpty() == true) // checks if space is available
        {
            isGrowing = true;
            currentPlant = seedType;
            currentPlantInfo = plantInfo.GetPlantInfo(currentPlant);
            Debug.Log("Getting planting information for " + currentPlantInfo.name);
        }
        else Debug.Log("Space is occupied");
    }

    void PlantEvolve()
    {
        Sprite sprite;
        plantStage += 1;
        if (plantStage == 1)
        {
            sprite = currentPlantInfo.stage1;
        }
        else if (plantStage == 2)
        {
            sprite = currentPlantInfo.stage2;
        }
        else if (plantStage == 3)
        {
            sprite = currentPlantInfo.stage3;
        }
        else if (plantStage == 4)
        {
            sprite = currentPlantInfo.stage4;
        }
        else sprite = null;

        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }
}