using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    // reference to script: https://pavcreations.com/turn-based-battle-and-transition-from-a-game-world-unity/#preserving-world-state-data
    // reference to script: https://www.youtube.com/watch?v=CE9VOZivb3I
    //[SerializeField]
    public Animator transition;
    //public AudioSource transitionAudio;
    [SerializeField]
    float transitionTime = 1f;
    #region making it a singleton
    private static LevelLoader _instance;
    public static LevelLoader Instance { get { return _instance; } }
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
    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadNamedLevel(levelName));
    }

    IEnumerator LoadNamedLevel(string levelName)
    {
        //transitionAudio.Play();

        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        // temporary solution, needs "End" trigger set up
        SceneManager.LoadScene(levelName);
        transition.SetTrigger("End");
    }

}


