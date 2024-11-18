using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AntiDuplicateSpawner : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnDefault
    {
        public GameObject uniqueObject;
        public Vector2 defaultLocation;
    }

    public List<SpawnDefault> spawnDefaults;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < spawnDefaults.Count; i++)
        {
            if (GameObject.Find(spawnDefaults[i].uniqueObject.name) == null)
            {
                GameObject newObject = GameObject.Instantiate(spawnDefaults[i].uniqueObject, spawnDefaults[i].defaultLocation, Quaternion.identity);
                newObject.name = spawnDefaults[i].uniqueObject.name; // prevends "(Clone)" from being appended to the name
                DontDestroyOnLoad(newObject); // enables persistence
            } else Debug.Log("Found " + spawnDefaults[i].uniqueObject.name + " in scene. No need to spawn another.");
        }
    }
}
