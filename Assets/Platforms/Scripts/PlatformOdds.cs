using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlatformOdds : ScriptableObject
{

    public Spawner spawner;
    public int odds;
    public int outOf;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Spawner roll()
    {
        int lottery = Random.Range(1, outOf + 1);
        if (lottery <= odds)
            return spawner;

        return null;
    }
}
