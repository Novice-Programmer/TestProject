using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KW9A : TestTower
{
    [Header("KW9A")]
    [SerializeField] Transform _firePointParent = null;
    Transform[] _firePoints;
    GameObject _rocketMissile;

    private void Awake()
    {
        _rocketMissile = Resources.Load("Tower/KW9A_Rocket") as GameObject;
        _firePoints = new Transform[_firePointParent.childCount];
        for (int i = 0; i < _firePoints.Length; i++)
        {
            _firePoints[i] = _firePointParent.GetChild(i);
        }
    }

    protected override void Attack()
    {
        base.Attack();
        StartCoroutine(MissileLaunch());
    }

    IEnumerator MissileLaunch()
    {

        for (int i = 0; i < _atkNumber; i++)
        {
            if (_target == null)
            {
                break;
            }
            for (int j = 0; j < _target.Length; j++)
            {
                GameObject go = Instantiate(_rocketMissile, _firePoints[Random.Range(0, _firePoints.Length)].position, Quaternion.identity);
                OneTargetProjectile missile = go.GetComponent<OneTargetProjectile>();
                missile.ProjectileSetting(_atk, _target[j]);
                yield return new WaitForSeconds(Random.Range(_atkSpd * 0.1f, _atkSpd * 0.2f));
            }
            yield return new WaitForSeconds(Random.Range(_atkSpd * 0.3f, _atkSpd * 0.5f));
        }
    }

    protected override void SpecialAttack()
    {
        base.SpecialAttack();
        StartCoroutine(MissileLaunch());
    }
}
