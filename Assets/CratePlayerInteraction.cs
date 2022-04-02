using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CratePlayerInteraction : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField]
    private float hitForce;
    [SerializeField]
    private float hitSpinForce;

    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<ObjectScroller>().enabled = true;
        transform.localRotation = Quaternion.identity;
    }

    void OnDisable()
    {
        
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<ObjectScroller>().enabled = false;
            rb.AddForce(new Vector3(hitForce, hitForce, 0));
            rb.AddTorque(-hitSpinForce);
        }
    }
}
