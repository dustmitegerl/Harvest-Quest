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
        }
        Debug.Log("couldn't find " + plantName + " in plantiingSOs");
        return null;
    }
    public void UpdateSpotData()
    {
        
    }
    public void UpdateSpotFromData()
    {
        
    }
    public void LoadData(GameData data)
    {

    }

    public void SaveData(ref GameData data)
    {

    }
}

public class PlantingSpotData { 
    
}
