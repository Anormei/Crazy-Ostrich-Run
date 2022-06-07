using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMargin : MonoBehaviour
{

    public float leftMargin;
    public float rightMargin;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public float rightMarginX()
    {
        return GetComponent<BoundsCalculator>().rightBound() + rightMargin;
    }

    public float leftMarginX()
    {
        return GetComponent<BoundsCalculator>().leftBound() + leftMargin;
    }
}
