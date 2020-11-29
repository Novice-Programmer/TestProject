using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NightMareDragonA : TestEnemy
{
    [SerializeField] Transform _specialAttackPos = null;
    [SerializeField] Projectile _specialAttack = null;
    [SerializeField] TestHitZone _attackZone = null;
    public override void TargetAttack()
    {
        base.TargetAttack();
        _attackZone.HitZoneSetting(_atk, _target.tag);
        _attackZone.gameObject.SetActive(true);
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
