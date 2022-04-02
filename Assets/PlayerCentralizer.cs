using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCentralizer : MonoBehaviour
{
    [SerializeField]
    private float regenGainPerSecond;
    [SerializeField]
    private float maxRegenSpeed;
    [SerializeField]
    private float regenCooldown;
    [SerializeField]
    private GameHandler game;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private Slide slideAction;

    private float regen = 0;
    private float currentCooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {

        if (positionedLessThanCenter() && running() && !inCooldown())
        {
            regenerate();
            Vector3 playerVelocity = rigidBody.velocity;
            //rb.AddForce(new Vector3(centralizerRegenSpeed, 0, 0));
            rigidBody.velocity = new Vector3(regen, rigidBody.velocity.y, 0);
        }
        else if(!inCooldown() || !running())
        {
            activateCooldown();
        }

        currentCooldown -= Time.deltaTime;
    }

    public void activateCooldown()
    {
        currentCooldown = regenCooldown;
        regen = 0;
    }

    private void regenerate()
    {
        regen += regenGainPerSecond * Time.deltaTime;
        if (regen > maxRegenSpeed)
            regen = maxRegenSpeed;
    }

    private bool positionedLessThanCenter()
    {

        Vector3 playerPos = rigidBody.position;
        return playerPos.x < game.World.center.x;
    }

    private bool running()
    {
        return !slideAction.isSliding() && grounded();
    }

    private bool inCooldown()
    {
        return currentCooldown > 0;
    }

    private bool grounded()
    {
        return Mathf.Approximately(rigidBody.velocity.y, 0);
    }

}
