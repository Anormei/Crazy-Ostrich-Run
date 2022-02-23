using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator : MonoBehaviour
{
    [SerializeField]
    private GameHandler game;
    [SerializeField]
    private GameObject startingPlatform;
    [SerializeField]
    private Spawner flatPlatformSpawner;
    private Spawner cliffSpawner;
    private Spawner rockTowerSpawner;


    private List<GameObject> terrain = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        terrain.Add(startingPlatform);
        flatPlatformSpawner.addObjectToSpawner(startingPlatform);
        attachToSpawner(startingPlatform, flatPlatformSpawner);
    }

    // Update is called once per frame
    void Update()
    {
        handleFrontTerrain();
        handleEndTerrain();
    }

    private void handleEndTerrain()
    {
        GameObject land = terrain[0];

        if (land.rightBound() < game.World.xMin)
        {
            land.GetComponent<SpawnAttacher>().delete();
            terrain.RemoveAt(0);
        }

    }

    private void handleFrontTerrain()
    {
        GameObject land = terrain[terrain.Count - 1];

        if (land.rightBound() < game.World.xMax)
        {

            terrain.Add(flatPlatformSpawner.createObject((obj) =>
            {
                obj.GetComponent<ObjectScroller>().game = game;
                obj.transform.parent = transform;
                obj.transform.gameObject.SetActive(true);
                obj.transform.position = new Vector3(land.rightBound() + obj.halfWidth(), land.transform.position.y, 0);
            }));
        }
    }

    private void attachToSpawner(GameObject obj, Spawner spawner)
    {
        obj.AddComponent<SpawnAttacher>();
        obj.GetComponent<SpawnAttacher>().Spawner = spawner;
    }

}
