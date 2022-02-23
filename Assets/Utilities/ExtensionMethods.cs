using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    

    //  Extension methods for GameObject

    public static float leftBound(this GameObject obj)
    {
        Vector3 pos = obj.transform.localPosition;
        Vector3 size = obj.GetComponent<Renderer>().bounds.size;
        
        return pos.x - size.x / 2.0f;
    }

    public static float topBound(this GameObject obj)
    {
        Vector3 pos = obj.transform.localPosition;
        Vector3 size = obj.GetComponent<Renderer>().bounds.size;

        return pos.y - size.y / 2.0f;
    }

    public static float rightBound(this GameObject obj)
    {
        Vector3 pos = obj.transform.localPosition;
        Vector3 size = obj.GetComponent<Renderer>().bounds.size;

        return pos.x + size.x / 2.0f;
    }

    public static float bottomBound(this GameObject obj)
    {
        Vector3 pos = obj.transform.localPosition;
        Vector3 size = obj.GetComponent<Renderer>().bounds.size;

        return pos.x + size.y / 2.0f;
    }

    public static float width(this GameObject obj)
    {
        return obj.GetComponent<Renderer>().bounds.size.x;
    }

    public static float height(this GameObject obj)
    {
        return obj.GetComponent<Renderer>().bounds.size.y;
    }
    public static float halfWidth(this GameObject obj)
    {
        return obj.GetComponent<Renderer>().bounds.size.x / 2.0f;
    }

    public static float halfHeight(this GameObject obj)
    {
        return obj.GetComponent<Renderer>().bounds.size.y / 2.0f;
    }
}
