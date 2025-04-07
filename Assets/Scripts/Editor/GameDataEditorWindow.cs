// thanks to https://docs.unity3d.com/Manual/UIE-HowTo-CreateEditorWindow.html

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class GameDataEditorWindow : EditorWindow
{
    private bool countIntsOpened;

    [MenuItem("Window/Game Data Editor")] // Menu item to open the window
    public static void ShowWindow()
    {
        GetWindow<GameDataEditorWindow>("Game Data Editor");
    }

    void OnGUI()
    {
        GameData.startingHr = EditorGUILayout.IntField("starting hour", GameData.startingHr);
        GameData.timeSpeedModulator = EditorGUILayout.FloatField("time speed modulator", GameData.timeSpeedModulator);
        GameData.secsInMin = EditorGUILayout.IntField("seconds per minute", GameData.secsInMin);
        GameData.minsInHr = EditorGUILayout.IntField("minutes per hour", GameData.minsInHr);
        GameData.hrsInDay = EditorGUILayout.IntField("hours per day", GameData.hrsInDay);
        GameData.daysInWeek = StringArrayField("days of the a week", ref countIntsOpened, GameData.daysInWeek);
        GameData.weeksInMonth = EditorGUILayout.IntField("weeks per month", GameData.weeksInMonth);
        GameData.monthsInYear = StringArrayField("months of the year", ref countIntsOpened, GameData.daysInWeek); ;
    }

    // thanks to https://stackoverflow.com/questions/71980696/unity-custom-editor-with-arrays
    public string[] StringArrayField(string label, ref bool open, string[] array)
    {
        // Create a foldout
        open = EditorGUILayout.Foldout(open, label);
        int newSize = array.Length;

        // Show values if foldout was opened.
        if (open)
        {
            // Int-field to set array size
            newSize = EditorGUILayout.IntField("Size", newSize);
            newSize = newSize < 0 ? 0 : newSize;

            // Creates a spacing between the input for array-size, and the array values.
            EditorGUILayout.Space();

            // Resize if user input a new array length
            if (newSize != array.Length)
            {
                array = ResizeArray(array, newSize);
            }

            // Make multiple text fields based on the length given
            for (var i = 0; i < newSize; ++i)
            {
                array[i] = EditorGUILayout.TextField($"Month {i}:", array[i]);
            }
        }
        return array;
    }
    private static T[] ResizeArray<T>(T[] array, int size)
    {
        T[] newArray = new T[size];

        for (var i = 0; i < size; i++)
        {
            if (i < array.Length)
            {
                newArray[i] = array[i];
            }
        }
        return newArray;
    }
}
