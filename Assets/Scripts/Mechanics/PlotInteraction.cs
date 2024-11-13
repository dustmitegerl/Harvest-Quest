using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlotInteraction : MonoBehaviour
{
    public Plant[] plants;
    [SerializeField]
    LevelLoader levelLoader;
    GameObject levelLoaderPrefab;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            levelLoader.LoadLevel("BattleArena");
        }
    }

    void Start()
    {
            levelLoaderPrefab = GameObject.Find("LevelLoader");
            levelLoader = levelLoaderPrefab.GetComponent<LevelLoader>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
