using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLibrary : MonoBehaviour
{
    [System.Serializable]
    public struct SpawnDefault
    {
        public GameObject uniqueObject;
        public Vector2 defaultLocation;
    }

    public List<SpawnDefault> spawnDefaults;
}