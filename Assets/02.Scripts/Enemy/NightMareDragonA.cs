using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightMareDragonA : TestEnemy
{
    [SerializeField] Transform _specialAttackPos = null;
    [SerializeField] OneTargetProjectile _specialAttack = null;
    [SerializeField] TestHitZone _attackZone = null;
    [SerializeField] float _attackAnimTime = 1.0f;
    [SerializeField] float _specialAnimTime = 0.8f;
    protected override void TargetAttack()
    {
        base.TargetAttack();
        StartCoroutine(AttackAction());
    }

    IEnumerator AttackAction()
    {
        _attackZone.HitZoneSetting(_atk);
        _attackZone.gameObject.SetActive(true);
        yield return new WaitForSeconds(_attackAnimTime);
        _attackZone.gameObject.SetActive(false);
    }

    protected override void TargetSpecialAttack()
    {
        base.TargetSpecialAttack();
        StartCoroutine(SpecialAttackAction());
    }

    IEnumerator SpecialAttackAction()
    {
        yield return new WaitForSeconds(_specialAnimTime);
        OneTargetProjectile attack = Instantiate(_specialAttack, _specialAttackPos.position, _specialAttackPos.rotation);
        attack.ProjectileSetting(_atk*2, _targetTower.transform, true);
        yield return null;
    }
}
