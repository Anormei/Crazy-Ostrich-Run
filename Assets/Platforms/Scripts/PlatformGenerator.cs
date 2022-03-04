using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{

    public enum TerrainType
    {
        Flat,
        Cliff,
        End,
        Tower,
        Scenario
    }

    public TerrainType terrainType;
    [HideInInspector]
    public List<PlatformOdds> platformOdds = new List<PlatformOdds>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
