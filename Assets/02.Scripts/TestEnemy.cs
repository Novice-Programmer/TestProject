using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EStateEnemy
{
    AttackTowerSearch,
    AttackCommander,
    Attack,
    Move,
    Special,
    Stun,
    Die,

    None
}

public enum ERatingEnemy
{
    Normal,
    SpecialEnemy,
    MineralEnemy,
    Boss
}

public abstract class TestEnemy : MonoBehaviour
{
    [SerializeField] protected TestEnemyData _enemyData;
    
    public ERatingEnemy _rating = ERatingEnemy.Normal;
    public EStateEnemy _state = EStateEnemy.Move;
    protected int _wavePointIndex = 0;
    protected bool _action = false;
    protected bool _isDead = false;

    protected EWeakType _weakType;
    protected int _hp;
    protected int _mp;
    protected int _atk; // 공격력
    protected int _def; // 방어력
    protected float _atkSpd; // 공격 속도
    protected float _movSpd; // 이동 속도

    List<TestBadBuff> _badBuffs;

    NavMeshAgent _enemyAI;

    float _attackTime = 0;
    float _timeCheck = 0;

    private void Awake()
    {
        _enemyAI = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        _enemyAI.destination = TestWayPoint._wayPoints[_wavePointIndex].position;
        _badBuffs = new List<TestBadBuff>();
        _state = EStateEnemy.Move;
        _hp = _enemyData.hp;
        _mp = 0;
        StatusCheck();
        StartCoroutine(EnemyAction());
    }

    private void Update()
    {
        if (_badBuffs.Count > 0)
        {
            StatusCheck();
        }
    }

    void StatusCheck()
    {
        if (_badBuffs.Count > 0)
        {
            int atkBadValue = 0;
            int defBadValue = 0;
            float atkSpdBadValue = 0;
            float movSpdBadValue = 0;

            for (int i = 0; i < _badBuffs.Count; i++)
            {
                TestBadBuff badBuff = _badBuffs[i];
                badBuff.checkTime += Time.deltaTime;
                badBuff.remainTime -= Time.deltaTime;

                if (badBuff.checkTime >= badBuff.dotTime)
                {
                    _hp -= _badBuffs[i].hp;
                    _mp -= _badBuffs[i].mp;
                    atkBadValue += _badBuffs[i].atk;
                    defBadValue += _badBuffs[i].def;
                    atkSpdBadValue += _badBuffs[i].atkSpd;
                    movSpdBadValue += _badBuffs[i].movSpd;
                    badBuff.checkTime = 0;
                }

                if (badBuff.remainTime <= 0)
                {
                    _badBuffs.RemoveAt(i);
                    break;
                }
            }
            _atk = _enemyData.atk - atkBadValue;
            if (_atk <= 0)
            {
                _atk = 1;
            }
            _def = _enemyData.def - defBadValue;
            _atkSpd = _enemyData.atk - atkSpdBadValue;
            if (_atkSpd < 0.01f)
            {
                _atkSpd = 0.01f; 
            }
            _movSpd = _enemyData.movSpd - movSpdBadValue;
            if(_movSpd < 0.1f)
            {
                _movSpd = 0.1f;
            }
        }
        else
        {
            _atk = _enemyData.atk;
            _def = _enemyData.def;
            _atkSpd = _enemyData.atkSpd;
            _movSpd = _enemyData.movSpd;
        }


        _enemyAI.speed = _movSpd;
        _enemyAI.acceleration = _movSpd * 2.0f;
    }

    IEnumerator EnemyAction()
    {
        while (!_isDead)
        {
            if (_action)
            {
                yield return null;
            }
            else
            {
                switch (_state)
                {
                    case EStateEnemy.AttackTowerSearch:
                        break;
                    case EStateEnemy.AttackCommander:
                        break;
                    case EStateEnemy.Attack:
                        break;
                    case EStateEnemy.Move:
                        Move();
                        break;
                    case EStateEnemy.Special:
                        break;
                    case EStateEnemy.None:
                        break;
                    case EStateEnemy.Stun:
                        break;
                    case EStateEnemy.Die:
                        break;
                }
            }
            yield return null;
        }
    }

    void Move()
    {
        if (Vector3.Distance(transform.position, _enemyAI.destination) <= 0.2f)
        {
            GetNextWayPoint();
        }
    }

    void GetNextWayPoint()
    {
        if (_wavePointIndex >= TestWayPoint._wayPoints.Length -1)
        {
            gameObject.SetActive(false);
            return;
        }
        _wavePointIndex++;
        _enemyAI.destination = TestWayPoint._wayPoints[_wavePointIndex].position;
    }

    public virtual void Hit(int damage)
    {
        _hp = damage - _def;
        if (_hp <= 0)
        {
            _hp = 0;
            _state = EStateEnemy.Die;
        }
    }
}
