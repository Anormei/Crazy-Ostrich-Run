﻿using System.Collections;
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
    [SerializeField]
    private float minGenerationTime;
    [SerializeField]
    private float maxGenerationTime;
    [SerializeField]
    private float edgePadding;
    [SerializeField]
    private float obstaclePadding;
    [SerializeField]
    private Spawner[] spawners;

    private float generationTime;

    private GameObject platformToPlace;
    private float minOffset;
    private float maxOffset;

    private List<GameObject> obstacles = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        generateNewTime();
    }

    // Update is called once per frame
    void Update()
    {
        handleActiveObstacles();
        generationTime -= Time.deltaTime;
        platformToPlace = terrainGenerator.getFrontEnd();
        /*if (platformToPlace != terrainGenerator.getFrontEnd())
        {

            minOffset = 0;
            maxOffset = platformToPlace.width();
        }*/

        if(generationTime <= 0)
        {
            GameObject obstacle = generateObstacle();
            attachToPlatform(obstacle);
            generateNewTime();
            
        }

    }

    public GameObject generateObstacle()
    {
        Spawner spawner = spawners[Random.Range(0, spawners.Length)];
        GameObject obstacle = spawner.createObject((obj) =>
        {
            obj.GetComponent<ObjectScroller>().game = game;
            obj.transform.parent = transform;
            obj.transform.gameObject.SetActive(true);
        });

        obstacles.Add(obstacle);
        return obstacle;
    }

    public void handleActiveObstacles()
    {
        if(obstacles.Count == 0)
            return;
       
        GameObject obstacle = obstacles[0];

        if (leftScreen(obstacle))
        {
            obstacle.GetComponent<SpawnAttacher>().delete();
            obstacles.RemoveAt(0);
        }

    }

    private void attachToPlatform(GameObject obj)
    {
        if (!canFitOnPlatform(obj))
        {
            Debug.Log("Cannot generate obstacle... awaiting...");
            obj.GetComponent<SpawnAttacher>().delete();
            obstacles.Remove(obj);
            return;
        }

        Debug.Log("Generating new obstacle...");

        obj.transform.position = new Vector3(placeUnseen(obj), placeOnTopOfPlatform(obj), 0);
    }

    private void generateNewTime()
    {
        generationTime = Random.Range(minGenerationTime, maxGenerationTime);
    }

    private float placeUnseen(GameObject obj)
    {
        float posX = game.World.xMax;
        float platformLeftBound = bounds(platformToPlace).leftBound();

        if (platformLeftBound > posX)
        {

            posX = platformLeftBound;

            posX += applyOffset(obj, posX);
        }

        if (obstacles.Count > 0)
        {
            GameObject obstacle = obstacles[obstacles.Count - 1];
            float obstacleRightBound = bounds(obstacle).rightBound();

            posX = obstacleRightBound > posX ? obstacleRightBound : posX;
        }


        return posX + bounds(obj).halfWidth();
    }

    // TODO fix
    private float applyOffset(GameObject obj, float startingPosX)
    {
        return Random.Range(0, getAvailableSpace() - bounds(obj).width());
    }

    private float placeOnTopOfPlatform(GameObject obj)
    {
        Vector3 platformPos = platformToPlace.transform.position;

        return platformPos.y + bounds(platformToPlace).halfHeight() + bounds(obj).halfHeight();
    }

    private float getAvailableSpace()
    {
        float furthest = getFurthestPoint();

        return bounds(platformToPlace).rightBound() - furthest;
    }

    private float getFurthestPoint()
    {
        float posX = game.World.xMax;
        float platformLeftBound = bounds(platformToPlace).leftBound();

        posX = platformLeftBound > posX ? platformLeftBound : posX;

        if (obstacles.Count > 0)
        {
            GameObject obstacle = obstacles[obstacles.Count - 1];
            float obstacleRightBound = bounds(obstacle).rightBound();

            posX = obstacleRightBound > posX ? obstacleRightBound : posX;
        }

        return posX;
    }

    private bool canFitOnPlatform(GameObject obj)
    {
        return bounds(obj).width() < getAvailableSpace();
    }

    private bool leftScreen(GameObject obj)
    {
        return bounds(obj).rightBound() < game.World.xMin || bounds(obj).topBound() < game.World.yMin;
    }

    private BoundsCalculator bounds(GameObject obj)
    {
        return obj.GetComponent<BoundsCalculator>();
    }
}
