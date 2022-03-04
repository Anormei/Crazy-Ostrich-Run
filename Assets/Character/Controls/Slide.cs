using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : MonoBehaviour
{
    [SerializeField]
    private GameHandler game;
    [SerializeField]
    private Animator animator;
    [SerializeField]
    private float slideForce;
    [SerializeField]
    private float slideKnockBack;
    [SerializeField]
    private float slideEndLag;
    [SerializeField]
    private Rigidbody2D rb;

    private bool slideHeld = false;
    private bool inEndLag = false;

    private float endLag = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if((slideHeld || inEndLag) && hasNoForwardMotion())
        {
            rb.transform.Translate(Time.deltaTime * game.ScrollSpeed * GameHandler.SCROLL_DIRECTION, 0, 0);
        }

        if (inEndLag)
        {
            endLag += Time.deltaTime;

            if (endLag > slideEndLag)
            {
                inEndLag = false;
                endLag = 0;

                if (!slideHeld)
                {
                    animator.SetBool("slide", false);
                }
            }
        }
    }

    public void holdSlide()
    {
        if (inEndLag)
        {
            return;
        }
        rb.velocity = new Vector2(0.01f, 0);
        rb.AddForce(new Vector2(slideForce, 0));
        slideHeld = true;
        inEndLag = true;
        animator.SetBool("slide", true);
    }

    public void releaseSlide()
    {
        slideHeld = false;

        if (!inEndLag)
        {
            animator.SetBool("slide", false);
        }
    }

    public bool hasNoForwardMotion()
    {
        return rb.velocity.x <= 0;
    }

}
