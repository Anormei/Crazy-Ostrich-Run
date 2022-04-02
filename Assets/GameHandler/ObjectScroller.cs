using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScroller : MonoBehaviour
{

    public enum ScrollerType
    {
        Dynamic,
        Kinematic,
        Static
    }

    public GameHandler game;

    private bool ready = false;

    private Action<GameHandler, MonoBehaviour> scroller;

    [SerializeField]
    private ScrollerType scrollerType;

    private Action<GameHandler, MonoBehaviour> kinematicScroller = (game, obj) =>
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.MovePosition(new Vector3(rb.position.x + game.ScrollSpeed * GameHandler.SCROLL_DIRECTION * Time.deltaTime, rb.position.y, 0));
    };

    private Action<GameHandler, MonoBehaviour> staticScroller = (game, obj) =>
    {
        obj.transform.Translate(new Vector3(game.ScrollSpeed * GameHandler.SCROLL_DIRECTION * Time.deltaTime, 0, 0));
    };

    private Action<GameHandler, MonoBehaviour> dynamicScroller = (game, obj) =>
    {
        Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector3(game.ScrollSpeed * GameHandler.SCROLL_DIRECTION, rb.velocity.y, 0);
    };

    // Start is called before the first frame update
    void Awake()
    {

        if (scrollerType == ScrollerType.Dynamic)
            scroller = dynamicScroller;
        else if (scrollerType == ScrollerType.Kinematic)
            scroller = kinematicScroller;
        else
            scroller = staticScroller;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (game == null)
            return;


        scroller(game, this);
    }
}
