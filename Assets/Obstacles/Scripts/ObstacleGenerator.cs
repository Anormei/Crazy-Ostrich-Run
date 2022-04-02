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
    private float edgeMargin;
    [SerializeField]
    private float obstacleMargin;
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

        if(generationTime <= 0 && !reachedObstacleLimit())
        {
            GameObject obstacle = generateObstacle();
            ObstacleBlocker obstacleBlocker = platformToPlace
                .GetComponent<SpawnAttacher>().Spawner
                .GetComponent<ObstacleBlocker>();

            if (obstacleBlocker != null && obstacleBlocker.isIllegal(obstacle))
            {
                obstacle.GetComponent<SpawnAttacher>().delete();
                obstacles.Remove(obstacle);
                Debug.Log("Obstacle is illegal, removing...");
            }
            else
            {
                attachToPlatform(obstacle);
            }
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

    private bool reachedObstacleLimit()
    {
        return obstacles.Count >= maxObstacles;
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
        float posX = getFurthestPoint();
        
        if(posX > game.World.x)
        {
            posX += applyOffset(obj);
        }

        return posX + bounds(obj).halfWidth();
    }

    private float applyOffset(GameObject obj)
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

        // get rightbound - edgemargin
        return bounds(platformToPlace).rightBound() - edgeMargin - furthest;
    }

    private float getFurthestPoint()
    {

        float worldRightX = game.World.xMax;
        float platformLeftBound = getPlatformToPlaceLeftBound();
        float obstacleRightBound = getFurthestObstacleX();

        float posX = platformLeftBound > worldRightX ? platformLeftBound  : worldRightX;
        posX = posX > obstacleRightBound ? posX : obstacleRightBound;

        if(Mathf.Approximately(posX, worldRightX))
        {
            float distanceToWorldRightX = worldRightX - Mathf.Max(platformLeftBound, obstacleRightBound);
            posX += getRemainderMargin(distanceToWorldRightX, whichMargin(platformLeftBound, obstacleRightBound));
        }

        return posX;
    }

    private float getRemainderMargin(float distance, float margin)
    {
        float difference = margin - distance;
        return difference > 0 ? difference : 0;
    }

    private float whichMargin(float platformLeftBound, float obstacleRightBound)
    {
        return platformLeftBound > obstacleRightBound ? edgeMargin : obstacleMargin;
    }

    private float getPlatformToPlaceLeftBound()
    {
        return bounds(platformToPlace).leftBound() + edgeMargin;
    }

    private GameObject getFurthestObstacle()
    {
        GameObject furthestObstacle = obstacles[0];
        foreach(GameObject obstacle in obstacles)
        {
            float currentObstacleX = furthestObstacle.transform.position.x;
            float nextObstacleX = obstacle.transform.position.x;
            furthestObstacle = nextObstacleX > currentObstacleX ? obstacle : furthestObstacle;
        }

        return furthestObstacle;
    }

    private float getFurthestObstacleX()
    {
        if (obstacles.Count > 0)
        {
            GameObject obstacle = getFurthestObstacle();
            float obstacleRightBound = bounds(obstacle).rightBound() + obstacleMargin;

            return obstacleRightBound;
        }
        return float.MinValue;
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
