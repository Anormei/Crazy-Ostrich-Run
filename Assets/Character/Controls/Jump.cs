using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float shortHopForce;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float doubleJumpForce;
    [SerializeField]
    private float shortHopReactionTime;

    private float yVelocity;

    private bool airJumped = false;

    private float timeJumpHeld = 0;

    private bool jumpedFromGround;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        yVelocity = rb.velocity.y;

        if (grounded())
        {
            airJumped = false;
        }

        if (jumpedFromGround)
        {
            timeJumpHeld += Time.deltaTime;
        }

    }

    void FixedUpdate()
    {

    }

    public void holdJump()
    {
        timeJumpHeld = 0;
        if (grounded())
        {
            jumpedFromGround = true;
            jump();
        }
        else
        {
            airJump();
        }
    }

    public void releaseJump()
    {
        if (jumpedFromGround && reactedForShortHop())
        {
            shortHop();
        }

        jumpedFromGround = false;

    }

    public void jump()
    {
        applyJumpForce(jumpForce);
    }

    public void shortHop()
    {
        applyJumpForce(shortHopForce);
    }

    public void airJump()
    {
        if (airJumped || grounded())
            return;

        applyJumpForce(doubleJumpForce);
        airJumped = true;
    }

    private bool grounded()
    {
        return Mathf.Approximately(yVelocity, 0);
    }

    private void applyJumpForce(float velocity)
    {
        rb.velocity = new Vector3(0, velocity, 0);

    }

    private bool reactedForShortHop()
    {
        return timeJumpHeld <= shortHopReactionTime;
    }
}
