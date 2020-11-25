using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestHitZone : MonoBehaviour
{
    int _damage = 0;

    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void HitZoneSetting(int damage)
    {
        _damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Tower"))
        {
            TestTower tower = other.GetComponent<TestTower>();
            tower.Hit(_damage);
        }
    }
}
