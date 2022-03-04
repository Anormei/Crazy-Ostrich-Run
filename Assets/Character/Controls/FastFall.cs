using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastFall : MonoBehaviour
{

    [SerializeField]
    private Rigidbody2D rb;

    [SerializeField]
    private float fastFallGravityScale;

    private float normalGravityScale;

    private bool heldFastFall = false;

    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        normalGravityScale = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (grounded())
        {
            rb.gravityScale = normalGravityScale;
        }
    }

    public void holdFastFall()
    {
        heldFastFall = true;
        rb.gravityScale = fastFallGravityScale;
    }

    public void releaseFastFall()
    {
        heldFastFall = false;
        rb.gravityScale = normalGravityScale;
    }

    private bool grounded()
    {
        return Mathf.Approximately(rb.velocity.y, 0);
    }
}
