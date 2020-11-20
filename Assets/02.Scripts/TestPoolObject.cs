using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TestPoolObject : MonoBehaviour
{
    public virtual void ActiveObject()
    {
        gameObject.SetActive(true);
    }

    public virtual void DisActiveObject()
    {
        gameObject.SetActive(false);
    }
}
