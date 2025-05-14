using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class DialogManager : MonoBehaviour
{
    [SerializeField] GameObject dialogBox;
    [SerializeField] public TMP_Text dialogText;
    [SerializeField] int lettersPerSecond;

    public event Action OnShowDialog;
    public event Action OnCloseDialog;

    #region making it a singleton
    private static DialogManager _instance;
    public static DialogManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    public bool IsShowing { get; private set; }

    public IEnumerator ShowDialogText(string text, bool waitForInput = true, bool autoClose = true)
    {
        OnShowDialog?.Invoke();
        IsShowing = true;
        dialogBox.SetActive(true);

        yield return TypeDialog(text);
        if (waitForInput)
        {
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        if (autoClose)
        {
            CloseDialog();
        }
    }
    
    public void CloseDialog()
    {
        dialogBox.SetActive(false);
        IsShowing = false;
        OnCloseDialog?.Invoke();
    }

    public IEnumerator ShowDialog(Dialog dialog)
    {
        //Show the dialog like it was typing it out
        yield return new WaitForEndOfFrame();

        OnShowDialog?.Invoke();
        IsShowing = true;
        dialogBox.SetActive(true);

        foreach (var line in dialog.Lines)
        {
            yield return TypeDialog(line);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        dialogBox.SetActive(false);
        IsShowing = false;
        OnCloseDialog?.Invoke();
    }

    public void HandleUpdate()
    {
        
    }

    public IEnumerator TypeDialog(string line)
    {
        //Show current dialog
        dialogText.text = "";
        foreach (var letter in line.ToCharArray())
        {
            dialogText.text += letter;
            yield return new WaitForSeconds(1f / lettersPerSecond);
        }
    }
}
