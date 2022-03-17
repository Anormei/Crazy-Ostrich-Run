using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCentralizer : MonoBehaviour
{

    [SerializeField]
    private float regenSpeed;
    [SerializeField]
    private GameHandler game;
    [SerializeField]
    private Rigidbody2D rigidBody;
    [SerializeField]
    private Slide slideAction;

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

        Vector3 playerPos = rigidBody.position;
        if (playerPos.x < game.World.center.x && !slideAction.isSliding() && grounded())
        {

            Vector3 playerVelocity = rigidBody.velocity;
            //rb.AddForce(new Vector3(centralizerRegenSpeed, 0, 0));
            rigidBody.velocity = new Vector3(regenSpeed, rigidBody.velocity.y, 0);
        }
    }

    private bool grounded()
    {
        return Mathf.Approximately(rigidBody.velocity.y, 0);
    }

}
