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
        for(int i = 0; i < _firePoints.Length; i++)
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
        for (int i = 0; i < 3; i++)
        {
            GameObject go = Instantiate(_rocketMissile, _firePoints[Random.Range(0, _firePoints.Length)].position,Quaternion.identity);
            NormalProjectile missile = go.GetComponent<NormalProjectile>();
            missile.ProjectileSetting(_atk / 3, _target);
            yield return new WaitForSeconds(Random.Range(0.1f, 0.3f));
        }
    }

    protected override void SpecialAttack()
    {
        base.SpecialAttack();
        StartCoroutine(MissileLaunch());
    }
}
