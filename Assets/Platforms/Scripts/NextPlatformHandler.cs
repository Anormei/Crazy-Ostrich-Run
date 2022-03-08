using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextPlatformHandler
    : MonoBehaviour
{

    public Spawner defaultToSpawnNext;
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

    public void createOdds()
    {
        platformOdds.Add(ScriptableObject.CreateInstance<PlatformOdds>());
    }

    public Spawner getNext()
    {
        Spawner spawner = null;
        foreach (PlatformOdds odds in platformOdds)
        {
            spawner = odds.roll();
        }

        return spawner != null ? spawner : defaultToSpawnNext;
    }
}
