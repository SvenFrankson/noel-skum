using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.Collections.Generic;

public class Test : EditorWindow
{
    [MenuItem("Window/Test")]
    static void Open()
    {
        EditorWindow.GetWindow<Test>();
    }

    public void OnGUI()
    {

        if (GUILayout.Button("Test Evaluate BiCubic"))
        {
            TestEvaluateBiCubic();
        }
    }

    public void TestEvaluateBiCubic()
    {
        RandomSeed r = new RandomSeed(DateTime.Now.Second);
        float[] randResults = new float[200];
        for (int i = 0; i < 200; i++)
        {
            randResults[i] = PlanetDataGenerator.EvaluateBiCubic(i * i * 43, i * 231, r);
        }

        List<float> randResultsList = new List<float>(randResults);
        randResultsList.Sort();

        string output = " | ";
        for (int i = 0; i < 200; i+=10)
        {
            output += randResultsList[i].ToString("0.000") + " | ";
        }
        output += randResultsList[199].ToString("0.000") + " | ";

        Debug.Log(output);
    }
}
