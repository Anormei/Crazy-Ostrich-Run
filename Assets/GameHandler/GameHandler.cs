using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    // Start is called before the first frame update

    public static class Orientation
    {
        public const float LEFT = -1.0f;
        public const float RIGHT = 1.0f;
    }

    public const float SCROLL_DIRECTION = Orientation.LEFT;
    public const float DEFAULT_SCROLL_SPEED = 10.0f;

    private float scrollSpeed;
    private Rect screenDimensions;

    public float ScrollSpeed
    {
        get { return scrollSpeed; }
        set { scrollSpeed = value; }
    }

    public Rect World
    {
        get { return screenDimensions; }
    }

    void Awake()
    {
        scrollSpeed = DEFAULT_SCROLL_SPEED;

        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 screenSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        screenDimensions = new Rect(cameraPos.x - screenSize.x, cameraPos.y - screenSize.y, screenSize.x * 2.0f, screenSize.y * 2.0f);
        Debug.Log(screenDimensions.xMax);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
