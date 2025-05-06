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
    FriendGiver friendGiver;

    private void Awake()
    {
        itemGiver = GetComponent<ItemGiver>();
        friendGiver = GetComponent<FriendGiver>();
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
        else if (friendGiver != null && friendGiver.CanBeGiven())
        {
            yield return friendGiver.GiveFriend(initiator.GetComponent<Unit>());
        }
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
