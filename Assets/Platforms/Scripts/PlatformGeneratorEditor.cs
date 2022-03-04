using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static PlatformGenerator;

[CustomEditor(typeof(PlatformGenerator))]
public class PlatformGeneratorEditor : Editor
{



    private void OnEnable()
    {
        
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        PlatformGenerator platformGenerator = (PlatformGenerator)target;

        GUILayout.BeginVertical("Odds to generate next terrain", "Window");

        for(int i = 0; i < platformGenerator.platformOdds.Count; i++)
        {
            PlatformOdds odds = platformGenerator.platformOdds[i];
            GUILayout.BeginVertical("", "Window");
            EditorGUI.BeginChangeCheck();
            TerrainType terrainType = (TerrainType)EditorGUILayout.EnumPopup("Terrain Type", odds.terrainType);
            int fieldOdds = EditorGUILayout.IntField("Odds", odds.odds);
            int fieldOutOf = EditorGUILayout.IntField("Out of", odds.outOf);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(platformGenerator, "Odds updated");
                odds.terrainType = terrainType;
                odds.odds = fieldOdds;
                odds.outOf = fieldOutOf;

            }

            if (GUILayout.Button("Delete"))
            {
                platformGenerator.platformOdds.Remove(odds);
            }
            GUILayout.EndVertical();
        }

        if (GUILayout.Button("Create odds"))
        {

            Undo.RecordObject(platformGenerator, "Added odds");
            platformGenerator.platformOdds.Add(new PlatformOdds());
        }

        GUILayout.EndVertical();


    }
}
