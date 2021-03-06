using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnAttacher : MonoBehaviour
{
    [SerializeField]
    private Spawner spawner;

    public Spawner Spawner
    {
        get { return spawner; }
        set { spawner = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isSpawnedBy(Spawner spawner)
    {
        return spawner == this.spawner;
    }

    public void delete()
    {
        gameObject.SetActive(false);
        spawner.deleteObject(gameObject);
    }
}
