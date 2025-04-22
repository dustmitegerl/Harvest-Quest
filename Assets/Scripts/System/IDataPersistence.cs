using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence 
{ //Reference: https://m.youtube.com/watch?v=aUi9aijvpgs&pp=0gcJCdgAo7VqN5tD
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
