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
        BoundsCalculator bounds = platformToPlace.GetComponent<BoundsCalculator>();
        float posX = bounds.leftBound() > game.World.xMax ? bounds.leftBound() : game.World.xMax;

        if(obstacles.Count > 0)
        {
            BoundsCalculator obstacleBounds = obstacles[0].GetComponent<BoundsCalculator>();
            posX = obstacleBounds.rightBound() > posX ? obstacleBounds.rightBound() : posX;
        }
        
        return posX + bounds.halfWidth();
    }

    private float placeOnTopOfPlatform(GameObject obj)
    {
        Vector3 platformPos = platformToPlace.transform.position;
        BoundsCalculator objBounds = obj.GetComponent<BoundsCalculator>();
        BoundsCalculator platformBounds = platformToPlace.GetComponent<BoundsCalculator>();

        return platformPos.y + platformBounds.halfHeight() + objBounds.halfHeight();
    }

    private float getAvailableSpace()
    {
        BoundsCalculator bounds = platformToPlace.GetComponent<BoundsCalculator>();
        return bounds.rightBound() - game.World.xMax;
    }

    private bool canFitOnPlatform(GameObject obj)
    {
        BoundsCalculator bounds = obj.GetComponent<BoundsCalculator>();
        return bounds.width() < getAvailableSpace();
    }

    private bool leftScreen(GameObject obj)
    {
        BoundsCalculator bounds = obj.GetComponent<BoundsCalculator>();
        return bounds.rightBound() < game.World.xMin;
    }
}
