using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneTargetProjectile : MonoBehaviour
{
    int _atk;
    Transform _target;
    bool _enemyAttack;

    [SerializeField] bool _splash = false;
    [SerializeField] float _speed = 10.0f;
    [SerializeField] float _rotateSpeed = 0.25f;
    [SerializeField] GameObject _boomEffect = null;
    [SerializeField] float _boomEffectTime = 1.0f;
    [SerializeField] float _boomRange = 1.0f;

    public void ProjectileSetting(int atk, Transform target, bool enemyAttack = false)
    {
        _atk = atk;
        _target = target;
        _enemyAttack = enemyAttack;
    }

    private void Update()
    {
        if(_target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = _target.position - transform.position;
        float distanceThisFrame = _speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.forward = Vector3.Lerp(transform.forward, dir.normalized, _rotateSpeed * Time.deltaTime);
    }

    void HitTarget()
    {
        Instantiate(_boomEffect,transform.position,transform.rotation);
        if (_enemyAttack)
        {
            TestTower tower = _target.GetComponent<TestTower>();
        }
        else
        {
            TestEnemy enemy = _target.GetComponent<TestEnemy>();
            enemy.Hit(_atk);
        }
        Destroy(gameObject);
    }
}
