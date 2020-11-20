using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalProjectile : MonoBehaviour
{
    int _atk;
    Transform _target;
    bool _enemyAttack;

    [SerializeField] float _speed = 10.0f;
    [SerializeField] float _rotateSpeed = 0.25f;
    [SerializeField] GameObject _boomEffect = null;
    [SerializeField] float _boomEffectTime = 1.0f;

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
        //transform.forward = Vector3.Lerp(transform.forward, dir.normalized, _rotateSpeed * Time.deltaTime);
        float distanceThisFrame = _speed * Time.deltaTime;

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(_target);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_enemyAttack)
        {

        }
        else
        {
            if(other.CompareTag("Enemy"))
            {
                GameObject go = Instantiate(_boomEffect);
                go.transform.position = transform.position;
                Destroy(go, _boomEffectTime);
                TestEnemy enemy = _target.GetComponent<TestEnemy>();
                enemy.Hit(_atk);
                Destroy(gameObject);
            }
        }
    }
}
