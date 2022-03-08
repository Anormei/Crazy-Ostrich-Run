using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    [SerializeField]
    private GameHandler game;
    [SerializeField]
    private TerrainGenerator terrainGenerator;
    [SerializeField]
    private float maxObstacles;

    private GameObject platformToPlace;
    private float minOffset;
    private float offsetMax;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (platformToPlace != terrainGenerator.getFrontEnd())
        {
            platformToPlace = terrainGenerator.getFrontEnd();
            minOffset = 0;
            offsetMax = platformToPlace.width();
        }
    }

    public void attachToPlatform(GameObject obj)
    {
        

        obj.transform.position = new Vector3(placeAtPlatformLeftEdge(obj) + randOffset(), placeOnTopOfPlatform(obj), 0);
        minOffset = obj.width();
    }

    private float placeAtPlatformLeftEdge(GameObject obj)
    {
        return platformToPlace.leftBound() + obj.halfWidth();
    }

    private float placeOnTopOfPlatform(GameObject obj)
    {
        Vector3 platformPos = platformToPlace.transform.position;
        return platformPos.y - (obj.halfHeight() - platformToPlace.halfHeight());
    }

    private float randOffset()
    {
        return Random.Range(minOffset, offsetMax);
    }

}
