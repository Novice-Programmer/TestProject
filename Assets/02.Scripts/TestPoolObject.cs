using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestPoolObject : MonoBehaviour
{
    protected int _poolNumber;
    public virtual void ActiveObject(int poolNumber)
    {
        _poolNumber = poolNumber;
        gameObject.SetActive(true);
    }

    public virtual void DisActiveObject()
    {
        gameObject.SetActive(false);
    }
}
