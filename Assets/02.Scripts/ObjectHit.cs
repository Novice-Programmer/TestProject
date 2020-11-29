using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectHit : MonoBehaviour
{
    public virtual void Hit(int damage, EWeakType weakType)
    {

    }

    public virtual void BadBuff(TestBadBuff badBuff)
    {

    }
}
