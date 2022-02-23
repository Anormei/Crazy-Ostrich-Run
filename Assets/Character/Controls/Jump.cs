using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour
{
    private Rigidbody2D rb;

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float fullJumpForce;
    [SerializeField]
    private float doubleJumpForce;
    [SerializeField]
    private float activateLongJumpTimer;

    private float yVelocity;

    private bool fullJumped = false;
    private bool airJumped = false;

    private float timePressed = 0;

    private bool isPressed;

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

        Debug.Log(rb.velocity);
    }

    void FixedUpdate()
    {
        if (isPressed)
        {
            timePressed += Time.deltaTime;
            jump();
        }
    }

    public void onDown()
    {
        if (grounded())
        {
            isPressed = true;
        }
        else
        {
            airJump();
        }
        //Debug.Log("onDown");
    }

    public void onUp()
    {
        isPressed = false;
        timePressed = 0;
        //Debug.Log("onUp");
    }

    public void jump()
    {
        if (grounded())
        {
            shortHop();
        }

        if (timePressed >= activateLongJumpTimer)
        {
            fullJump();
        }
    }

    public void shortHop()
    {
        fullJumped = false;
        applyJumpForce(jumpForce);
    }

    public void fullJump()
    {
        if (fullJumped || airJumped)
            return;

        fullJumped = true;
        applyJumpForce(fullJumpForce);
 
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
}
