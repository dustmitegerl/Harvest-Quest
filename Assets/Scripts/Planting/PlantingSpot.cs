using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantingSpot : MonoBehaviour, IDropHandler
{
    public string id;
    public bool isEmpty;
    public string currentPlant; // used to find scriptable object of the plant type's stats
    public int plantStage = 0; // 0 = empty / seed planted; 1-3 = incomplete growth stages; 4 = ready to harvest; 5 = dead 
    public int level = 0;
    public float daysTilEvolve;
    public bool isGrowing = false;
    [SerializeField]
    SpriteRenderer spriteRenderer;
    [SerializeField]
    PlantGrowthManager plantInfo;
    PlantingSO currentPlantInfo;

    [ContextMenu("Generate guid for id")]
    void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    void Start()
    { 
        // if lacking a GUID (which should be set in the Inspector)
        if (id == "")
        {   // Logs and assigns a new GUID
            Debug.Log("A planting spot in " 
                + transform.parent.name 
                + " has a null id");
        }
        
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
        // handle drag-and-drop here, call Sow(seed)
    }
    void Sow(string seedType) // plants seed
    {
        if (isEmpty) // checks if space is available
        {
            isGrowing = true;
            currentPlant = seedType;
            currentPlantInfo = plantInfo.GetPlantInfo(currentPlant);
            Debug.Log("Getting planting information for " + currentPlantInfo.name);
        }
        else Debug.Log("Space is occupied");
    }

    [ContextMenu("Evolve Plant")]
    void PlantEvolve()
    {
        if (plantStage <= 4)
        {
            plantStage += 1;
            spriteRenderer.sprite = currentPlantInfo.stageSprites[plantStage - 1];
        }
        else
        {
            Debug.Log("Plant is already ready to harvest!");
        }
    }

    public void Harvest()
    {
 
    }
}