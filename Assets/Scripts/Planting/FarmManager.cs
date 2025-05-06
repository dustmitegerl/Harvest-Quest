using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.Examples;
using UnityEngine;

public class FarmManager : MonoBehaviour, IDataPersistence
{
    public PlantingSO[] plantingSOs;
    public List<PlantingSpot> spotData;
    GameObject[] existingSpots;
    #region making it a singleton
    private static FarmManager _instance;
    public static FarmManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        UpdateSpotData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public PlantingSO GetPlantInfo(string plantName)
    {
        foreach (PlantingSO species in plantingSOs)
        {
            if (species.name.ToLower() == plantName.ToLower())
            {
                return species;
            }
            else
            {
                Debug.Log("failed to retrieve plant info");
                return null;
            }
        }
        Debug.Log("couldn't find plantingSOs");
        return null;
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
