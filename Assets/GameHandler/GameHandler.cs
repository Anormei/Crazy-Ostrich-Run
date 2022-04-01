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
    private float finalScrollSpeed;
    private Rect screenDimensions;

    private float speedBoost;
    private float speedBoostDecrease;

    private float speedOffset = 0;

    public float ScrollSpeed
    {
        get { return finalScrollSpeed; }
        set { finalScrollSpeed = value; }
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

    private void FixedUpdate()
    {
        finalScrollSpeed = scrollSpeed + speedOffset;

        if (speedBoost > 0)
        {
            finalScrollSpeed += speedBoost;
            speedBoost -= speedBoostDecrease * Time.deltaTime;
        }
    }

    public void scrollFaster(float speed)
    {
        finalScrollSpeed += speed;
    }

    public void scrollSlower(float speed)
    {
        finalScrollSpeed -= speed;
    }

    public void boostSpeed(float speed, float duration)
    {
        speedBoost = speed;
        speedBoostDecrease = speed / duration;
    }

    public bool hasSpeedBoost()
    {
        return speedBoost > 0;
    }
}
