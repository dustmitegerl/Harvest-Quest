using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine. UI;
public class GlobalAchievements : MonoBehaviour
{

    // reference to script: https://www.youtube.com/watch?v=-XuzwxkZ2Wk


    //General Points

    public GameObject achNotes;

    public AudioSource achSound;

    public bool achActive = false;

    public GameObject ach01Title;

    public GameObject ach01Desc;

    // Setting Up First Achievement 

    public GameObject ach01Image;

   

    public static int ach01Count;

    public int ach01Trigger = 5;

    public int ach01Code;


    // Update is called once per frame

    void Update()

    {

        ach01Code = PlayerPrefs.GetInt("Ach01");

        if (ach01Count == ach01Trigger && ach01Code != 12345)

        {   

            StartCoroutine(Trigger01Ach());

        }

    }


    IEnumerator Trigger01Ach()

    {   

        achActive = true;

        ach01Code = 12345;

        PlayerPrefs.SetInt("Ach01", ach01Code);

        achSound.Play();

        ach01Image.SetActive(true);

        ach01Title.GetComponent<Text>().text = "Start Here!";

        ach01Desc.GetComponent<Text>().text = "Starting to play Harvest Quest!";

        achNotes.SetActive(true);

        yield return new WaitForSeconds(7);


        //Resetting UI

        achNotes.SetActive(false);

        ach01Image.SetActive(false);

        ach01Title.GetComponent<Text>().text = "";

        ach01Desc.GetComponent<Text>().text = "";

        achActive = false;

    }
}
