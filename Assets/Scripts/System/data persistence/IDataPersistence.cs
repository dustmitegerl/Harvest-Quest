using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// thanks to https://www.youtube.com/watch?v=aUi9aijvpgs&t=1211s
public interface IDataPersistence
{
    void LoadData(GameData data);

    // The 'ref' keyword was removed from here as it is not needed.
    // In C#, non-primitive types are automatically passed by reference.
    void SaveData(GameData data);
}