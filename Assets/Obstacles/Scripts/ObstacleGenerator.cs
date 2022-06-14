using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleGenerator : MonoBehaviour
{
    public static int MAX_POINTS = 100;

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
    private Spawner crateObstacleSpawner;
    [SerializeField]
    private float multiCrateMargin;
    [SerializeField]
    private ObstacleSpawner[] obstacleSpawners;

    private float spawnPointX;

    private float generationTime;

    private GameObject platformToPlace;
    private List<GameObject> platformsToPlace = new List<GameObject>();
    private float minOffset;
    private float maxOffset;

    private List<GameObject> obstacles = new List<GameObject>();
    private Queue<GameObject> obstacleQueue = new Queue<GameObject>();

    private List<GameObject> platformsOnSpawnPointX = new List<GameObject>();

    void Awake()
    {
        spawnPointX = game.World.xMax;
    }

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

        if(generationTime <= 0 && !reachedObstacleLimit() && obstacleQueue.Count == 0 && getFurthestObstacleX() < spawnPointX)
        {
            // replace: queue a bunch of objects
            ObstacleSpawner spawner = getRandomObstacleSpawner();

            /*
             * 1. Spawn initial obstacle (if valid)
             * if valid:
             *      2. Generate random points
             *      3. Spawn more obstacles from random points and random spawners ? or odds-based generation?
             * if invalid:
             *      skip
             */

            GameObject obstacle = spawner.generateObstacle();
            obstacle.SetActive(false);
            obstacleQueue.Enqueue(obstacle);

            int points = generateRandomPoints();
            bool canAfford = true;

            do{
                ObstacleGenerationFeedback feedback = spawner.buyObstacle(points);
                points = feedback.remainingPoints;

                if(feedback.obstacleGenerated != null)
                {
                    feedback.obstacleGenerated.SetActive(false);
                    obstacleQueue.Enqueue(feedback.obstacleGenerated);
                }

                canAfford = points > 0;
            } while (canAfford);

                generateNewTime();
            
        } // else still generate new time?

        if (spawnPointIsFree())
        {
            GameObject obstacle = obstacleQueue.Dequeue();

            platformsToPlace.Clear();
            terrainGenerator.findIntersectingEdges(spawnPointX, spawnPointX + bounds(obstacle).width(), platformsToPlace);

            if (!isObstacleIllegal(obstacle) && spawnIsInBound())
            {
                attachToPlatform(obstacle);
                obstacle.SetActive(true);
            }
            else
            {
                obstacleQueue.Enqueue(obstacle);
            }
            /*else
            {
                obstacle.GetComponent<SpawnAttacher>().delete();
                foreach (GameObject nextObstacle in obstacleQueue)
                {
                    nextObstacle.GetComponent<SpawnAttacher>().delete();
                }
                obstacleQueue.Clear();
            }*/
        }

    }

    public int generateRandomPoints()
    {
        return Random.Range(0, MAX_POINTS);
    }

    private bool spawnPointIsFree()
    {

        GameObject furthestObstacle = getFurthestObstacle();
        if (furthestObstacle == null)
        {

            return obstacleQueue.Count > 0;
        }

        float obstacleRightMost = furthestObstacle.GetComponent<ObstacleMargin>().rightMarginX();
        return obstacleRightMost < spawnPointX && obstacleQueue.Count > 0;
    }

    private bool spawnIsInBound()
    {
        if (platformsToPlace.Count == 0)
            return false;

        GameObject left = platformsToPlace[0];
        GameObject right = platformsToPlace[platformsToPlace.Count - 1];

        float leftBoundary = bounds(left).leftBound() + edgeMargin;
        float rightBoundary = bounds(right).rightBound() - edgeMargin;

        return spawnPointX > leftBoundary && spawnPointX < rightBoundary;
    }

    private ObstacleSpawner getRandomObstacleSpawner()
    {
        return obstacleSpawners[Random.Range(0, obstacleSpawners.Length)];
    }

    public bool isObstacleIllegal(GameObject obstacle)
    {

        bool banned = false;

        foreach (GameObject platform in platformsToPlace)
        {
            ObstacleBlocker obstacleBlocker = platformToPlace
                .GetComponent<SpawnAttacher>().Spawner
                .GetComponent<ObstacleBlocker>();

            if(obstacleBlocker != null && obstacleBlocker.isIllegal(obstacle))
                banned = true;
        }

        bool isIllegal = banned || !canFitOnPlatform(obstacle);

        return isIllegal;
    }

    private bool hasLeftMarginSpace(GameObject obstacle)
    {
        GameObject furthestObstacle = getFurthestObstacle();
        if (furthestObstacle == null)
        {

            return true;
        }

        float obstacleRightMost = furthestObstacle.GetComponent<ObstacleMargin>().rightMarginX();
        float nextObstacleLeftMost = obstacle.GetComponent<ObstacleMargin>().leftMarginX();

        return nextObstacleLeftMost > obstacleRightMost;

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
            removeObstacle(obstacle);
        }

    }

    public void removeObstacle(GameObject obstacle)
    {
        obstacle.GetComponent<SpawnAttacher>().delete();
        obstacles.Remove(obstacle);
    }

    private void attachToPlatform(GameObject obstacle)
    {

        obstacles.Add(obstacle);
        obstacle.transform.position = new Vector3(placeAtSpawnPointX(obstacle), placeOnTopOfPlatform(obstacle), 0);
    }

    private void generateNewTime()
    {
        generationTime = Random.Range(minGenerationTime, maxGenerationTime);
    }

    private float placeAtSpawnPointX(GameObject obj)
    {
        return spawnPointX + bounds(obj).halfWidth();
    }

    private float generateOffset(GameObject obj)
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

        Debug.Log("platformsToPlacer = " + platformsToPlace.Count);
        if (platformsToPlace.Count == 0)
            return 0;

        float length = bounds(platformsToPlace[platformsToPlace.Count - 1]).rightBound();

        return length - spawnPointX - edgeMargin;
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
        return bounds(platformToPlace).leftBound();
    }

    private GameObject getFurthestObstacle()
    {
        if (obstacles.Count == 0)
            return null;

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
        if (obstacles.Count == 0)
            return float.MinValue;

            GameObject obstacle = getFurthestObstacle();
            float obstacleRightBound = bounds(obstacle).rightBound() + obstacleMargin;

            return obstacleRightBound;
        

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
