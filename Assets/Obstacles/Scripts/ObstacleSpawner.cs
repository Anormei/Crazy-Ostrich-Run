using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{

    public static int MAX_POINTS = 100;

    [SerializeField]
    private GameObject objectToSpawn;
    [SerializeField]
    private GameHandler game;
    [SerializeField]
    private Spawner spawner;
    [SerializeField]
    private int cost;

    private List<GameObject> objects;

    void Awake()
    {

    }

    public ObstacleGenerationFeedback buyObstacle(int pointsLeft)
    {

        int pointsRemaining = pointsLeft - cost;
        ObstacleGenerationFeedback feedback = new ObstacleGenerationFeedback();
        if (pointsRemaining > 0)
        {
            feedback.remainingPoints = pointsRemaining;
            feedback.obstacleGenerated = generateObstacle();
        }

        return feedback;
    }

    public GameObject generateObstacle()
    {
        GameObject obstacle = spawner.createObject((obj) =>
        {
            obj.GetComponent<ObjectScroller>().game = game;
            obj.transform.parent = transform;
            obj.transform.gameObject.SetActive(true);
        });

        return obstacle;

    }

}
