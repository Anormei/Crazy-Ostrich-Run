using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleBlocker : MonoBehaviour
{

    [SerializeField]
    private Spawner[] spawners;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isIllegal(GameObject obj)
    {
        SpawnAttacher spawnAttacher = obj.GetComponent<SpawnAttacher>();
        if(spawnAttacher == null)
            return false;


        foreach (Spawner spawner in spawners)
        {
            if (spawnAttacher.isSpawnedBy(spawner))
                return true;
        }

        return false;
    }
}
