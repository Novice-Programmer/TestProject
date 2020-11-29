using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightMareDragonA : TestEnemy
{
    [SerializeField] Transform _specialAttackPos = null;
    [SerializeField] GameObject _attackEffect = null;
    [SerializeField] Projectile _specialAttack = null;
    [SerializeField] TestHitZone _attackZone = null;
    public override void TargetAttack()
    {
        base.TargetAttack();
        if (_target == null)
            return;
        _attackZone.HitZoneSetting(_atk, _target.tag);
        _attackZone.gameObject.SetActive(true);
        Vector3 effectPos = new Vector3(_target.position.x, _target.position.y + 1f, _target.position.z);
        Instantiate(_attackEffect, effectPos, Quaternion.identity);
    }

    public override void AttackEnd()
    {
        base.AttackEnd();
        _attackZone.gameObject.SetActive(false);
    }

    public override void TargetSpecialAttack()
    {
        base.TargetSpecialAttack();
        Projectile attack = Instantiate(_specialAttack, _specialAttackPos.position, _specialAttackPos.rotation);
        attack.ProjectileSetting(_target.position, _target.tag, (int)(_atk * 2.5f));
    }

    public override void SkillEnd()
    {
        base.SkillEnd();
    }
}
