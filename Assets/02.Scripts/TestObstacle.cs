using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EAttackAble
{
    None,
    AttackAble,
    AttackDisable
}

public enum EObstacleType
{
    None,
    FireWall,
}

public class TestObstacle : ObjectHit
{
    public EObstacleType _obstacleName = EObstacleType.None;
    public EAttackAble _obstacleType = EAttackAble.None;
    [SerializeField] HitPad _hitPad = null;
    [SerializeField] protected int _durability;
    [SerializeField] protected int _reduceValue;
    [SerializeField] protected float[] _values;

    private void Start()
    {
        _hitPad.HitPadSetting("Enemy", _values);
    }

    public override void Hit(int damage, EWeakType weakType)
    {
        _durability -= damage;
        DurabilityCheck();
    }

    protected void DurabilityCheck()
    {
        if (_durability <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        _durability -= _reduceValue;
        DurabilityCheck();
    }
}
