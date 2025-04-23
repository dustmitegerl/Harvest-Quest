using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// thanks to https://www.youtube.com/watch?v=aUi9aijvpgs&t=1211s
public interface IDataPersistence
{
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}