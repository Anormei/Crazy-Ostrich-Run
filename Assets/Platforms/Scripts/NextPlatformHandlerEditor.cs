using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(NextPlatformHandler))]
public class NextPlatformHandlerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        NextPlatformHandler script = (NextPlatformHandler)target;

        GUILayout.BeginVertical("Odds", "Window");

        for(int i = 0; i < script.platformOdds.Count; i++)
        {
            PlatformOdds odds = script.platformOdds[i];


            GUILayout.BeginVertical("", "Window");
            EditorGUI.BeginChangeCheck();
            Spawner spawner = (Spawner)EditorGUILayout.ObjectField("Spawner", odds.spawner, typeof(Spawner), true);
            int oddsValue = EditorGUILayout.IntField("Odds", odds.odds);
            int outOf = EditorGUILayout.IntField("Out of", odds.outOf);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(script, "ModifyOdds");
                odds.spawner = spawner;
                odds.odds = oddsValue;
                odds.outOf = outOf;
            }
            if (GUILayout.Button("Delete"))
            {
                Undo.RecordObject(script, "DeleteOdds");
                script.platformOdds.Remove(odds);
            }
            GUILayout.EndVertical();
        }

        if(GUILayout.Button("Create Odds"))
        {
            Undo.RecordObject(script, "CreateOdds");
            script.createOdds();
        }

        GUILayout.EndVertical();
    }
}
