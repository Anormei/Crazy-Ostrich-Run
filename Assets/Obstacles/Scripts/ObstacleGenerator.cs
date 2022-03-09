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

        if (platformToPlace != terrainGenerator.getFrontEnd())
        {
            platformToPlace = terrainGenerator.getFrontEnd();
            minOffset = 0;
            maxOffset = platformToPlace.width();
        }

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
            obj.GetComponent<SpawnAttacher>().delete();
            obstacles.Remove(obj);
            return;
        }

        obj.transform.position = new Vector3(placeUnseen(obj), placeOnTopOfPlatform(obj), 0);
    }

    private void generateNewTime()
    {
        generationTime = Random.Range(minGenerationTime, maxGenerationTime);
    }

    private float placeUnseen(GameObject obj)
    {
        float posX = bounds(platformToPlace).leftBound() > game.World.xMax ? bounds(platformToPlace).leftBound() : game.World.xMax;

        if(obstacles.Count > 0)
        {
            GameObject obstacle = obstacles[0];
            posX = bounds(obstacle).rightBound() > posX ? bounds(obstacle).rightBound() : posX;
        }
        
        return posX + bounds(obj).halfWidth();
    }

    private float placeOnTopOfPlatform(GameObject obj)
    {
        Vector3 platformPos = platformToPlace.transform.position;

        return platformPos.y + bounds(platformToPlace).halfHeight() + bounds(obj).halfHeight();
    }

    private float getAvailableSpace()
    {
        float closest = bounds(platformToPlace).rightBound();
        if(obstacles.Count > 0)
        {
            GameObject lastObstacle = obstacles[obstacles.Count - 1];
            closest = bounds(lastObstacle).rightBound() > closest ? bounds(lastObstacle).rightBound() : closest;
        }
        return closest - game.World.xMax;
    }

    private bool canFitOnPlatform(GameObject obj)
    {
        return bounds(obj).width() < getAvailableSpace();
    }

    private bool leftScreen(GameObject obj)
    {
        return bounds(obj).rightBound() < game.World.xMin;
    }

    private BoundsCalculator bounds(GameObject obj)
    {
        return obj.GetComponent<BoundsCalculator>();
    }
}
