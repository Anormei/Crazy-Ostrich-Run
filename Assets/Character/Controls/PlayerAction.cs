using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private UnityEvent holdSlide;
    [SerializeField]
    private UnityEvent releaseSlide;
    [SerializeField]
    private UnityEvent holdFastFall;
    [SerializeField]
    private UnityEvent releaseFastFall;



    private float yVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        yVelocity = rb.velocity.y;
    }

    public void holdDown()
    {
        if (grounded())
        {
            holdSlide.Invoke();
        }
        else
        {
            holdFastFall.Invoke();
        }
    }

    public void release()
    {
        if (grounded())
        {
            releaseSlide.Invoke();
        }
        else
        {
            releaseFastFall.Invoke();
        }
    }

    private bool grounded()
    {
        return Mathf.Approximately(yVelocity, 0);
    }
}
