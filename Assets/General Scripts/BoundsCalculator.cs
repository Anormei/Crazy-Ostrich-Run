using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCalculator : MonoBehaviour
{
    Bounds bounds;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Renderer>() != null)
        {
            bounds = GetComponent<Renderer>().bounds;
        }
        else
        {
            bounds = new Bounds(this.transform.position, Vector3.zero);
            foreach(Renderer renderer in GetComponentsInChildren<Renderer>())
            {
                bounds.Encapsulate(renderer.bounds);
            }

            Vector3 localCenter = bounds.center - this.transform.position;
            bounds.center = localCenter;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float leftBound()
    {
        Vector3 pos = transform.localPosition;
        Vector3 size = bounds.size;

        return pos.x - size.x / 2.0f;
    }

    public float topBound()
    {
        Vector3 pos = transform.localPosition;
        Vector3 size = bounds.size;

        return pos.y - size.y / 2.0f;
    }

    public float rightBound()
    {
        Vector3 pos = transform.localPosition;
        Vector3 size = bounds.size;

        return pos.x + size.x / 2.0f;
    }

    public float bottomBound()
    {
        Vector3 pos = transform.localPosition;
        Vector3 size = bounds.size;

        return pos.x + size.y / 2.0f;
    }

    public float width()
    {
        return bounds.size.x;
    }

    public float height()
    {
        return bounds.size.y;
    }
    public float halfWidth()
    {
        return bounds.size.x / 2.0f;
    }

    public float halfHeight()
    {
        return bounds.size.y / 2.0f;
    }
}
