using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrienshipGiver : MonoBehaviour
{
    [SerializeField] ProgressBar friendshipToGive;
    [SerializeField] Dialog dialog;

    bool used = false;

    public IEnumerator GiveFriendship(Unit player)
    {
        yield return DialogManager.Instance.ShowDialog(dialog);

        //player.GetComponent<ExperienceManager>().AddExperience(friendshipToGive);

        used = true;

        //string dialogText = $"Player recieved {friendshipToGive.Base.Name}";

        //yield return DialogManager.Instance.ShowDialogText(dialogText);
    }

    public bool CanBeGiven()
    {
        return friendshipToGive != null && !used;
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
