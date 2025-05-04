using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, Interactable
{
    [SerializeField] Dialog dialog;

    [SerializeField] QuestBase questToStart;
    [SerializeField] QuestBase questToComplete;

    Quest activeQuest;

    ItemGiver itemGiver;
    //FrienshipGiver friendshipGiver;

    private void Awake()
    {
        itemGiver = GetComponent<ItemGiver>();
        //friendshipGiver = GetComponent<FrienshipGiver>();
    }

    public IEnumerator Interact(Transform initiator)
    {
        if(questToComplete != null)
        {
            var quest = new Quest(questToComplete);
            yield return quest.CompleteQuest(initiator);
            questToComplete = null;

            Debug.Log($"{quest.Base.Name} completed");
        }

        if (itemGiver != null && itemGiver.CanBeGiven())
        {
            yield return itemGiver.GiveItem(initiator.GetComponent<Unit>());
        }
        /*else if (friendshipGiver != null && friendshipGiver.CanBeGiven())
        {
            /yield return friendshipGiver.GiveFriendship(initiator.GetComponent<Unit>());
        }*/
        else if (questToStart != null)
        {
            activeQuest = new Quest(questToStart);
            yield return activeQuest.StartQuest();
            questToStart = null;

            if (activeQuest.CanBeCompleted())
            {
                yield return activeQuest.CompleteQuest(initiator);
                activeQuest = null;
            }
        }
        else if (activeQuest != null)
        {
            if (activeQuest.CanBeCompleted())
            {
                yield return activeQuest.CompleteQuest(initiator);
                activeQuest = null;
            }
            else
            {
                yield return DialogManager.Instance.ShowDialog(activeQuest.Base.InProgressDialogue);
            }
        }
        else
        {
            yield return DialogManager.Instance.ShowDialog(dialog);
        }
    }
}

public enum NPCState { Dialog}
