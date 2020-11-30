using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EHitType
{
    None,
    Enemy,
    Tower,
    Commander,
    Obstacle
}

public abstract class ObjectHit : MonoBehaviour
{
    public EHitType _hitType = EHitType.None;
    public virtual void Hit(int damage, EWeakType weakType)
    {

    }

    public virtual void ReduceMP(int mp)
    {

    }

    public virtual void BadBuff(TestBadBuff badBuff)
    {

    }

    public virtual bool BuffCheck(EBadBuff buffType)
    {
        return false;
    }

    public virtual void BadBuffUpdate(TestBadBuff badBuff)
    {
    }
}
