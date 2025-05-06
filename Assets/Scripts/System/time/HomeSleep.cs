using System.Collections;
using System.Collections.Generic;
using TMPro.Examples;
using UnityEngine;

public class HomeSleep : MonoBehaviour
{
    [SerializeField]
    Interaction interaction;
    [SerializeField]
    bool inRange;
    [HeaderAttribute("messages to insert into dialog. \n&actionkey gets replaced with GameController.actionKey.")]
    [SerializeField]
    string[] napMessage;
    [SerializeField]
    string[] sleepMessage;
    enum Time { NAPTIME, SLEEPTIME};
    [SerializeField]
    Time currentTime;

    void Awake()
    {
        interaction = GetComponent<Interaction>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("player detected at " + gameObject.name);
            inRange = true;
            currentTime = GetTime();
            UpdateDialog();
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        inRange = false;
    }
    #region setup
    void UpdateDialog()
    {
        Dialog dialog = interaction.dialog;
        string[] message;
        dialog.Lines.Clear();
        if (currentTime == Time.SLEEPTIME)
        {
            message = sleepMessage;
        }
        else if (currentTime == Time.NAPTIME) {
            message = napMessage;
        }
        else
        {
            message = null;
        }
        foreach (string line in message) {
            if (line.Contains("&actionkey"))
            {
                dialog.Lines.Add(line.Replace("&actionkey", GameController.Instance.actionKey.ToString()));
            }
            else dialog.Lines.Add(line);
        }
    }
    Time GetTime()
    {
        if (GameTime.hrs >= GameTime.Instance.bedTime)
        {
            return Time.SLEEPTIME;
        }
        else return Time.NAPTIME;
    }
    #endregion
    #region action

    void HandleInput()
    {
        if (inRange && Input.GetKeyDown(GameController.Instance.actionKey))
        {
            if (currentTime == Time.SLEEPTIME)
            {
                GameTime.Instance.Sleep();
            }
            else if (currentTime == Time.NAPTIME)
            {
                GameTime.Instance.Nap();
            }
            else
            {
                Debug.LogWarning("issue getting time in " + gameObject.name);
            }
        }
    }

    void Update()
    {
        HandleInput();
    }
    #endregion

}
