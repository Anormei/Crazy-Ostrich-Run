using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolFactory<T>
{

    private Func<T> createObject;
    private Queue<T> objects = new Queue<T>();

    private int max = 5;

    public PoolFactory(Func<T> factory)
    {
        this.createObject = factory;
    }

    public PoolFactory(Func<T> factory, int max) : this(factory)
    {
        this.max = max;
    }

    public T newObject()
    {
        return objects.Count > 0 ? objects.Dequeue() : createObject();

    }

    public void free(T gameObject)
    {
        if(objects.Count >= max)
        {
            return;
        }

        objects.Enqueue(gameObject);
    }
}
