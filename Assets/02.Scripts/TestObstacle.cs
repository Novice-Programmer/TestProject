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
    public EObjectName _objectName = EObjectName.None;
    public EObstacleType _obstacleName = EObstacleType.None;
    public EAttackAble _obstacleType = EAttackAble.None;
    [SerializeField] HitPad _hitPad = null;
    [SerializeField] int _durability;
    [SerializeField] int _reduceValue;
    [SerializeField] float[] _values;
    [SerializeField] TestWorldStatusUI _duraBar = null;
    TestTile _parentTile;
    TestIntVector2 _gridPosition;
    TestIntVector2 _dimensions;
    EFitType _fitType;

    private void Start()
    {
        _hitPad.HitPadSetting("Enemy", _values);
        _duraBar.StatusSetting(_durability);
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
            _durability = 0;
            Destroy(gameObject);
        }
        _duraBar.HPChange(_durability);
    }

    private void OnTriggerEnter(Collider other)
    {
        _durability -= _reduceValue;
        DurabilityCheck();
    }

    public void BuildingObstacle(TestGhost ghost)
    {
        _parentTile = ghost._parentTile;
        _gridPosition = ghost._gridPos;
        _dimensions = ghost._demision;
        _fitType = ghost._fitType;
        switch (ghost._rotateType)
        {
            case ERotateType.degree0:
                break;
            case ERotateType.degree90:
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                break;
            case ERotateType.degree180:
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                break;
            case ERotateType.degree270:
                transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                break;
        }
    }

    public void SellObstacle()
    {
        _parentTile.Clear(_gridPosition, _dimensions, _fitType);
        Destroy(gameObject);
    }
}
