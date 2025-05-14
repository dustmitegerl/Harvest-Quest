using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlantingSpot : MonoBehaviour
{
    public string id;
    public bool isEmpty;
    public string currentPlant; // used to find scriptable object of the plant type's stats
    [HeaderAttribute("0=empty/just planted; 4=ready; 5=dead")]
    [Range(0, 5)]
    public int plantStage = 0; // 0 = empty / seed planted; 1-3 = incomplete growth stages; 4 = ready to harvest; 5 = dead 
    [HeaderAttribute("1=regular; 2,etc=composted")]
    public int level = 0;
    public float daysTilEvolve;
    public bool isGrowing = false;
    SpriteRenderer spriteRenderer;
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
        UpdateSprite();
    }
    void Update()
    {
    }
    public bool IsSpaceEmpty()
    {
        if (currentPlant == "")
        {
            return true;
        }
        else return false;
    }
    
    public void Sow(string seedType) // plants seed
    {
        if (isEmpty) // checks if space is available
        {
            isGrowing = true;
            currentPlant = seedType;
            GetPlantInfo();
        }
        else Debug.Log("Space is occupied");
    }

    [ContextMenu("Evolve Plant")]
    void PlantEvolve()
    {
        isEmpty = IsSpaceEmpty(); // check to make sure it isn't empty
        if (!isEmpty)
        {
            if (plantStage <= 4)
            {
                plantStage += 1;
                
            }
            else
            {
                Debug.Log("Plant is already ready to harvest!");
            }
        }
        else Debug.Log("can't evolve " + gameObject.name + " because it is empty");
        UpdateSprite();
    }
    public void UpdateSprite()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (currentPlantInfo == null && currentPlant !="")
        {
            GetPlantInfo();
            
        }
        if (currentPlantInfo != null && spriteRenderer != null && plantStage > 0)
        {
            spriteRenderer.sprite = currentPlantInfo.stageSprites[plantStage - 1];
        }
    }
    void GetPlantInfo()
    {
        currentPlantInfo = FarmManager.Instance.GetPlantInfo(currentPlant);
    }
    public void Harvest()
    {
 
    }
}