using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriendGiver : MonoBehaviour
{
    [SerializeField] PartyManager friendToHelp;
    [SerializeField] Dialog dialog;

    bool used = false;

    public IEnumerator GiveFriend(Unit player)
    {
        yield return DialogManager.Instance.ShowDialog(dialog);

        //player.GetComponent<PartyManager>().AddMemberToPartyByName(friendToHelp);

        used = true;

        string dialogText = $"Player recieved friend";

        yield return DialogManager.Instance.ShowDialogText(dialogText);
    }

    public bool CanBeGiven()
    {
        return friendToHelp != null && !used;
    }

    public object CaptureState()
    {
        return used;
    }

    public void RestoreState(object state)
    {
        used = (bool)state;
    }
}
