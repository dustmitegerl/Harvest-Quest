using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowthInfo : MonoBehaviour
{
    public PlantingSO[] plantsTypes;

    public PlantingSO GetPlantInfo(string plantName)
    {
        foreach (PlantingSO species in plantsTypes)
        {
            if (species.name.Contains(plantName.ToLower()))
            {
                Debug.Log("retrieved stats for growing " + species.name);
                return species;
            }
        }
        Debug.Log("failed to find stats for growing " + plantName);
        return null;
    }
}
