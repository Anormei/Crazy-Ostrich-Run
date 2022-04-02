using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCrateInteraction : MonoBehaviour
{

    [SerializeField]
    private float crashForce;

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private PlayerCentralizer playerCentralizer;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Crate")
        {
            playerCentralizer.activateCooldown();
            rb.AddForce(new Vector3(-crashForce, 0, 0));
        }
    }
}
