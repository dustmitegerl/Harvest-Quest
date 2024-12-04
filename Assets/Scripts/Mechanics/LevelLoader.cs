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

    GameTime gameTime;
    

    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadNamedLevel(levelName));
        if (gameTime == null)
        {
            gameTime = GameObject.FindGameObjectWithTag("Game Manager").GetComponent<GameTime>();
        }
    }

    IEnumerator LoadNamedLevel(string levelName)
    {
        //transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
        if (levelName == "BattleArena")
        {
            gameTime.Pause();
        } else gameTime.UnPause();

        //transition.SetTrigger("End");
    }


    void Awake()
    {
        instance = this;
        
    }

    public void EndBattle() 
    { 
      LevelLoader.instance.LoadLevel(mainLevelName);
    }

}
