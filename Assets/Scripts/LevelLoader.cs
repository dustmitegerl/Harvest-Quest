using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // reference to script: https://pavcreations.com/turn-based-battle-and-transition-from-a-game-world-unity/#preserving-world-state-data

    public static LevelLoader instance;
    public string mainLevelName;
    [SerializeField]
    //Animator transition;
    //[SerializeField]
    float transitionTime = 1f;

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadNamedLevel(levelName));

    }

    IEnumerator LoadNamedLevel(string levelName)
    {
        //transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);


        //transition.SetTrigger("End");
    }


    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this.gameObject);
        //GameObject[] levelLoaders = GameObject.FindGameObjectsWithTag("Level Loader");
        //if (levelLoaders.Length > 1)
        //{
        //    for (int i = 0; i < levelLoaders.Length; i++)
        //    {
        //        if (levelLoaders[i] != this)
        //        {
        //            Destroy(levelLoaders[i]);
        //        }
        //    }
        //}
    }

    public void EndBattle() 
    { 
      LevelLoader.instance.LoadLevel(mainLevelName);
    }

}
