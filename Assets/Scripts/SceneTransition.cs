using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] int goingToScene;
    [SerializeField] float delayTime;

    private void OnTriggerEnter(Collider other)
    {
        var hitTag = other.gameObject.tag;

        if (hitTag == "Player")
        {
            StartCoroutine(LoadDelay());
        }
    }

    IEnumerator LoadDelay()
    {
        yield return new WaitForSeconds(delayTime);
        SceneManager.LoadScene(goingToScene);
    }
}
