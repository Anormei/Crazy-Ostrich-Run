using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCentralizer : MonoBehaviour
{

    [SerializeField]
    private float regenSpeed;
    [SerializeField]
    private float regenCooldown;
    [SerializeField]
    private GameHandler game;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private Slide slideAction;

    private float currentCooldown = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {

        if (positionedCenter() && running() && !inCooldown())
        {

            Vector3 playerVelocity = rigidBody.velocity;
            //rb.AddForce(new Vector3(centralizerRegenSpeed, 0, 0));
            rigidBody.velocity = new Vector3(regenSpeed, rigidBody.velocity.y, 0);
        }
        else if(!inCooldown())
        {
            currentCooldown = regenCooldown;
        }

        currentCooldown -= Time.deltaTime;
    }

    private bool positionedCenter()
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
