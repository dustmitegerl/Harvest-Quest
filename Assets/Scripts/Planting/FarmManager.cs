using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FarmManager : MonoBehaviour, IDataPersistence
{
    public List<PlantingSpot> spotData;
    GameObject[] existingSpots;

    // Start is called before the first frame update
    void Start()
    {
        UpdateSpotData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateSpotData()
    {
        existingSpots = GameObject.FindGameObjectsWithTag("Planting Spot");
        foreach (GameObject spot in existingSpots)
        {
            PlantingSpot thisSpot = spot.GetComponent<PlantingSpot>();
            PlantingSpot spotDatum = gameObject.AddComponent<PlantingSpot>();
            spotDatum.id = thisSpot.id;
            spotDatum.currentPlant = thisSpot.currentPlant;
            spotDatum.plantStage = thisSpot.plantStage;
            spotDatum.level = thisSpot.level;
            spotDatum.isGrowing = thisSpot.isGrowing;
            spotData.Add(spotDatum);
        }
    }
    public void UpdateSpotFromData()
    {
        existingSpots = GameObject.FindGameObjectsWithTag("Planting Spot");
        foreach (GameObject spot in existingSpots)
        {
            PlantingSpot thisSpot = spot.GetComponent<PlantingSpot>();
            foreach (PlantingSpot spotDatum in spotData)
            {
                if (thisSpot.id == spotDatum.id)
                {
                    thisSpot.currentPlant = spotDatum.currentPlant;
                    thisSpot.plantStage = spotDatum.plantStage;
                    thisSpot.level = spotDatum.level;
                    thisSpot.isGrowing = spotDatum.isGrowing;
                }
            }
        }
    }
    public void LoadData(GameData data)
    {

    }

    public void SaveData(ref GameData data)
    {

    }
}
