using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PlayerCameraController : MonoBehaviour
{
    [SerializeField]
    GameObject mainCam;
    [SerializeField]
    string overWorldName;
    [SerializeField]
    string battleArenaName;
    [SerializeField]
    string currentSceneName;
    // Start is called before the first frame update
    void Start()
    {
        currentSceneName = SceneManager.GetActiveScene().name;
        mainCam = transform.Find("Main Camera").gameObject;

        if (currentSceneName == battleArenaName)
        {
            mainCam.SetActive(false);
        }
        else if (currentSceneName == overWorldName)
        {
            mainCam.SetActive(true);
        }
    }
}
