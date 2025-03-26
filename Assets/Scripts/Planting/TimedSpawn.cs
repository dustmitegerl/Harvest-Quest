using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedSpawn : MonoBehaviour
{
    public GameObject spawnobj;
    public bool stopSpawning = false;
    public float spawnTime;
    public float spawnDelay;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnObject", spawnTime, spawnDelay);
    }

    public void SpawnObject()
    {
        Instantiate(spawnobj, transform.position, transform.rotation);
        if (stopSpawning)
        {
            CancelInvoke("SpawnObject");
        }
    }
}
