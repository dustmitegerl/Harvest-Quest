using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // reference to script: https://pavcreations.com/turn-based-battle-and-transition-from-a-game-world-unity/#preserving-world-state-data

    public static LevelLoader instance;
    public Animator transition;
    public float transitionTime = 1f;

    public LoadLevel(string levelName) 
    { 
        StartCoroutine(LoadNameLevel(levelName));
    }

    IEnumerator LoadNamedLevel(string levelName) 
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);

        transition.SetTrigger("End");
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
