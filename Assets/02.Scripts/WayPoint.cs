using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WayPoint : MonoBehaviour
{
    int _wayPointNumber;

    public void WayPointSetting(int number)
    {
        _wayPointNumber = number;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy._state == EStateEnemy.Move)
                enemy.GetNextWayPoint(_wayPointNumber);
            else if(enemy._state == EStateEnemy.AttackSearch && enemy._target==null)
                enemy.GetNextWayPoint(_wayPointNumber);
        }
    }
}
