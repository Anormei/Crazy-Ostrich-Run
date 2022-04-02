using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    private GameHandler game;
    [SerializeField]
    private Spawner emptySpaceSpawner;
    [SerializeField]
    private GameObject startingPlatform;

    [SerializeField]
    private float minEmptySpace;
    [SerializeField]
    private float maxEmptySpace;

    [SerializeField]
    private float minElevation;
    [SerializeField]
    private float maxElevation;

    [SerializeField]
    private int changeElevationOdds;
    [SerializeField]
    private int changeElevationOddsOf;

    private List<GameObject> terrain = new List<GameObject>();

    private float currentElevation;

    private float terrainY;

    // Start is called before the first frame update
    void Start()
    {
        terrain.Add(startingPlatform);
        terrainY = startingPlatform.transform.position.y + startingPlatform.GetComponent<BoundsCalculator>().halfHeight();
    }

    // Update is called once per frame
    void Update()
    {
        if (terrain.Count > 0)
        {
            handleFrontTerrain();
            handleEndTerrain();
        }
    }

    public GameObject getFrontEnd()
    {
        return terrain[terrain.Count - 1];
    }

    private void handleEndTerrain()
    {
        GameObject land = terrain[0];

        if (rightEdgeOffScreen(land))
        {
            land.GetComponent<SpawnAttacher>().delete();
            terrain.RemoveAt(0);
        }

    }

    private void handleFrontTerrain()
    {
        GameObject land = terrain[terrain.Count - 1];

        if (rightEdgeOnScreen(land))
        {

            generateTerrain(land);
        }
    }

    private void generateTerrain(GameObject land)
    {
        Spawner spawner = getNextSpawner(land);

        float offsetX = 0;

        if(spawner == emptySpaceSpawner)
        {
            if (shouldChangeElevation())
            {
                Debug.Log("Elevation changing...");
                currentElevation = generateNewElevation();
            }
            offsetX = randOffset();
            spawner = spawner.GetComponent<NextPlatformHandler>().getNext();
        }

        terrain.Add(spawner.createObject((obj) =>
        {
            obj.GetComponent<ObjectScroller>().game = game;
            obj.transform.parent = transform;
            obj.transform.gameObject.SetActive(true);

            positionAtEnd(obj, offsetX);

        }));
    }

    private void attachToSpawner(GameObject obj, Spawner spawner)
    {
        obj.AddComponent<SpawnAttacher>();
        obj.GetComponent<SpawnAttacher>().Spawner = spawner;
    }

    private Spawner getNextSpawner(GameObject obj)
    {
        return getNextPlatformHandler(obj).getNext();
    }

    private NextPlatformHandler getNextPlatformHandler(GameObject obj)
    {
        return getSpawner(obj).GetComponent<NextPlatformHandler>();
    }

    private Spawner getSpawner(GameObject obj)
    {
        return obj.GetComponent<SpawnAttacher>().Spawner;
    }

    private void positionAtEnd(GameObject obj, float offset)
    {
        GameObject land = terrain[terrain.Count - 1];
        BoundsCalculator objBounds = obj.GetComponent<BoundsCalculator>();
        BoundsCalculator landBounds = land.GetComponent<BoundsCalculator>();

        obj.transform.position = new Vector3(landBounds.rightBound() + objBounds.halfWidth() + offset, getElevation(obj), 0);
    }

    private bool rightEdgeOnScreen(GameObject obj)
    {
        BoundsCalculator bounds = obj.GetComponent<BoundsCalculator>();
        return bounds.rightBound() < game.World.xMax;
    }

    private bool rightEdgeOffScreen(GameObject obj)
    {
        BoundsCalculator bounds = obj.GetComponent<BoundsCalculator>();
        return bounds.rightBound() < game.World.xMin;
    }

    private float randOffset()
    {
        return Random.Range(minEmptySpace, maxEmptySpace);
    }

    private bool hasPhysicalBody(GameObject obj)
    {
        return obj.GetComponent<Rigidbody2D>() != null;
    }

    private float generateNewElevation()
    {
        return Random.Range(minElevation, maxElevation);
    }

    private bool shouldChangeElevation()
    {

        return Random.Range(0, changeElevationOddsOf) <= changeElevationOdds;
    }

    private float getElevation(GameObject obj)
    {
        BoundsCalculator objBounds = obj.GetComponent<BoundsCalculator>();
        return terrainY - objBounds.halfHeight() + currentElevation;
    }


}
