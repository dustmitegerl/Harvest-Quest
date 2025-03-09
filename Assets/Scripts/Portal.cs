using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    [SerializeField] int nextScene;
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
        SceneManager.LoadScene(nextScene);
    }
}
