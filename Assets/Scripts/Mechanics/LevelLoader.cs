using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // reference to script: https://pavcreations.com/turn-based-battle-and-transition-from-a-game-world-unity/#preserving-world-state-data
    // reference to script: https://www.youtube.com/watch?v=CE9VOZivb3I
    public static LevelLoader instance;
    [SerializeField]
    string farmScene = "Farm";
    //[SerializeField]
    public Animator transition;
    //public AudioSource transitionAudio;
    [SerializeField]
    float transitionTime = 1f;
    [SerializeField]
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
        //transitionAudio.Play();

        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);

        //transition.SetTrigger("End");
    }

    void Awake()
    {
        instance = this;
        
    }

    public void EndBattle() 
    { 
      LevelLoader.instance.LoadLevel(farmScene);
    }

}


