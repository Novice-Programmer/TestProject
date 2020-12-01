using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassEnemyInfo : ObjectGame
{
    [SerializeField] TestEnemy _passData = null;

    public override void Select(bool selectOff = true)
    {
        _passData.Select(selectOff);
    }
}
