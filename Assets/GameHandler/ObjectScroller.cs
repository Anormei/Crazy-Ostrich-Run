using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScroller : MonoBehaviour
{

    public GameHandler game;

    private bool ready = false;

    // Start is called before the first frame update
    void Start()
    {
        if(game != null)
        {
            ready = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!ready)
        {
            return;
        }

        transform.Translate(Time.deltaTime * game.ScrollSpeed * GameHandler.SCROLL_DIRECTION, 0, 0);
    }
}
