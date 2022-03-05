using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject objectToSpawn;
    public int maxPoolSize;

    private PoolFactory<GameObject> poolFactory;
    private List<GameObject> objects = new List<GameObject>();


    void Awake()
    {
        poolFactory = new PoolFactory<GameObject>(() =>
        {
            return Instantiate(objectToSpawn);
        }, maxPoolSize);
    }

    public GameObject createObject()
    {
        GameObject obj = poolFactory.newObject();
        addObjectToSpawner(obj);

        return obj;
    }

    public GameObject createObject(Action<GameObject> applyBeforeCreation)
    {
        GameObject obj = poolFactory.newObject();
        addObjectToSpawner(obj);
        applyBeforeCreation(obj);
        objects.Add(obj);

        return obj;
    }

    public void deleteObject(GameObject obj)
    {
        objects.Remove(obj);
        poolFactory.free(obj);
    }

    public void deleteObject(GameObject obj, Action<GameObject> applyOnDeletion)
    {
        applyOnDeletion(obj);
        objects.Remove(obj);
        poolFactory.free(obj);

    }

    public void addObjectToSpawner(GameObject obj)
    {
        objects.Add(obj);
        obj.AddComponent<SpawnAttacher>();
        obj.GetComponent<SpawnAttacher>().Spawner = this;
    }
 
}
