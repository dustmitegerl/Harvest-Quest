using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest
{
    public QuestBase Base { get; private set; }
    public QuestStatus Status { get; private set; }

    public Quest(QuestBase _base)
    {
        Base = _base;
    }

    public IEnumerator StartQuest()
    {
        Status = QuestStatus.Started;

        yield return DialogManager.Instance.ShowDialog(Base.StartDialogue);

        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);
    }

    public IEnumerator CompleteQuest(Transform player)
    {
        Status = QuestStatus.Completed;

        yield return DialogManager.Instance.ShowDialog(Base.CompleteDialogue);

        var inventory = Inventory.GetInventory();
        if (Base.RequiredItem != null)
        {
            //inventory.RemoveItem(Base.RequiredItem);
        }
        if (Base.RewardItem != null)
        {
            
            //inventory.AddItem(Base.RewardItem);

            yield return DialogManager.Instance.ShowDialogText($"Player recieved {Base.RewardItem.name}");
        }

        var questList = QuestList.GetQuestList();
        questList.AddQuest(this);
    }

    public bool CanBeCompleted()
    {
        var inventory = Inventory.GetInventory();
        if(Base.RequiredItem != null)
        {
            //if (!inventory.HasItem(Base.RequiredItem))
                //return false;
        }
        return true;
    }
}

public enum QuestStatus { None, Started, Completed }
