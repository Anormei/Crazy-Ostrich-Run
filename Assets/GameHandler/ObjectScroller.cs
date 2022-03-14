using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScroller : MonoBehaviour
{

    public GameHandler game;

    private bool ready = false;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if(game != null && rb != null)
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

        rb.MovePosition(new Vector3(rb.position.x + game.ScrollSpeed * GameHandler.SCROLL_DIRECTION * Time.deltaTime, rb.position.y, 0));
    }
}
