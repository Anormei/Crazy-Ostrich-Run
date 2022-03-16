using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticScroller : MonoBehaviour
{
    public GameHandler game;
    private bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        if (game != null)
        {
            ready = true;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!ready)
        {
            return;
        }

        transform.Translate(new Vector3(game.ScrollSpeed * GameHandler.SCROLL_DIRECTION * Time.deltaTime, 0, 0));
    }
}
