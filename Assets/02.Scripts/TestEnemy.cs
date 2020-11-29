using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum EStateEnemy
{
    Idle,
    AttackSearch,
    Attack,
    Move,
    AttackCommander,
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

public abstract class TestEnemy : ObjectHit
{
    [SerializeField] protected TestEnemyData _enemyData;

    public ERatingEnemy _rating = ERatingEnemy.Normal;
    public EStateEnemy _state = EStateEnemy.Idle;
    protected int _wavePointIndex = 0;
    protected bool _action = false;
    protected bool _isDead = false;

    protected EWeakType _weakType;
    [SerializeField] protected int _hp;
    [SerializeField] protected int _mp;
    [SerializeField] protected int _atk; // 공격력
    [SerializeField] protected int _def; // 방어력
    [SerializeField] protected float _atkSpd; // 공격 속도
    [SerializeField] protected float _movSpd; // 이동 속도
    [SerializeField] protected float _moveCheckSize; // 이동 확인 범위
    [SerializeField] protected TestWorldStatusUI _statusUI = null;

    [SerializeField] List<TestBadBuff> _badBuffs;

    NavMeshAgent _enemyAI;
    Animator _enemyAnim;
    BoxCollider _enemyCllider;
    public Transform _target;

    string _towerTag = "Tower";
    [SerializeField] BoxCollider _blockCollider = null;
    float _timeCheck = 0;
    float _attackTime = 0;

    private void Awake()
    {
        _enemyAI = GetComponent<NavMeshAgent>();
        _enemyAnim = GetComponent<Animator>();
        _enemyCllider = GetComponent<BoxCollider>();
    }

    public void Active()
    {
        gameObject.SetActive(true);
        _enemyAI.enabled = true;
        _enemyCllider.enabled = true;
        _blockCollider.enabled = true;
        _wavePointIndex = 0;
        _enemyAI.destination = WayPointContainer._wayPoints[_wavePointIndex].position;
        _badBuffs = new List<TestBadBuff>();
        _state = EStateEnemy.Move;
        _action = false;
        _timeCheck = 0;
        _attackTime = 0;
        _hp = _enemyData.hp;
        _statusUI.StatusSetting(_enemyData.hp);
        _statusUI.HPChange(_hp);
        _mp = 0;
        StatusCheck();
        StartCoroutine(BadBuffCheck());
    }

    public void Disactive()
    {
        TestWaveManager.Instance.WaveEnemyDie();
        gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_action)
        {
            return;
        }
        else
        {
            _timeCheck += Time.deltaTime;
            switch (_state)
            {
                case EStateEnemy.AttackSearch:
                    AttackSearch();
                    break;
                case EStateEnemy.Attack:
                    Attack();
                    break;
                case EStateEnemy.Move:
                    Move();
                    break;
                case EStateEnemy.AttackCommander:
                    AttackCommander();
                    break;
                case EStateEnemy.None:
                    break;
                case EStateEnemy.Stun:
                    break;
            }
        }
    }

    IEnumerator BadBuffCheck()
    {
        while (!_isDead)
        {
            if (_badBuffs.Count > 0)
            {
                StatusCheck();
            }
            yield return null;
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
                    if (_badBuffs[i].hp > 0)
                    {
                        Hit(_badBuffs[i].hp, EWeakType.None);
                    }
                    _mp -= _badBuffs[i].mp;
                    atkBadValue += _badBuffs[i].atk;
                    defBadValue += _badBuffs[i].def;
                    atkSpdBadValue += _badBuffs[i].atkSpd;
                    movSpdBadValue += _badBuffs[i].movSpd;
                    _statusUI.MPChange(_mp);
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
            if (_movSpd < 0.1f)
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

        if (_enemyAI.speed > 0)
            _enemyAI.speed = _movSpd;
        _enemyAI.acceleration = _movSpd * 2.0f;
    }

    #region 액션

    void Move()
    {
        _enemyAI.speed = _movSpd;
        _enemyAnim.SetBool("Move", true);
        if (_timeCheck >= _enemyData.checkTime)
        {
            _timeCheck = 0;
            float checkPer = Random.Range(0.0f, 100.0f);
            if (checkPer <= _enemyData.atkRate)
            {
                _state = EStateEnemy.AttackSearch;
                return;
            }
        }
    }

    void AttackSearch()
    {
        _enemyAI.speed = _movSpd;
        _enemyAnim.SetBool("Move", true);
        if (_timeCheck >= _enemyData.atkTime)
        {
            _timeCheck = 0;
            GetNextWayPoint(--_wavePointIndex);
            _state = EStateEnemy.Move;
            return;
        }
        GameObject[] towers = GameObject.FindGameObjectsWithTag(_towerTag);
        float shortestDistance = _enemyData.sightRange;
        GameObject nearestTower = null;
        foreach (GameObject tower in towers)
        {
            if (tower.GetComponent<TestTower>()._towerState == ETowerState.Breakdown)
                continue;
            float distanceToTower = Vector3.Distance(transform.position, tower.transform.position);
            if (distanceToTower < shortestDistance)
            {

                shortestDistance = distanceToTower;
                nearestTower = tower;
            }
        }

        if (nearestTower != null)
        {
            _state = EStateEnemy.Attack;
            _attackTime = 0;
            _target = nearestTower.GetComponent<TestTower>().transform;
            _enemyAI.destination = nearestTower.transform.position;
        }
    }

    void Attack()
    {
        if (_timeCheck >= _enemyData.atkTime)
        {
            _timeCheck = 0;
            GetNextWayPoint(--_wavePointIndex);
            _state = EStateEnemy.Move;
            return;
        }

        if (_target == null)
        {
            GetNextWayPoint(--_wavePointIndex);
            _state = EStateEnemy.AttackSearch;
            return;
        }
        else if (_target != null && _target.GetComponent<TestTower>()._towerState == ETowerState.Breakdown)
        {
            _target = null;
            GetNextWayPoint(--_wavePointIndex);
            _state = EStateEnemy.AttackSearch;
            return;
        }
        if (Vector3.Distance(transform.position, _target.position) < _enemyData.atkRange)
        {
            _enemyAI.speed = 0;
            _enemyAnim.SetBool("Move", false);
            Quaternion lookRotation = Quaternion.LookRotation(_target.position - transform.position);
            Vector3 rotateValue = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            transform.rotation = Quaternion.Euler(transform.rotation.x, rotateValue.y, transform.rotation.z);
            if (_attackTime <= 0.0f)
            {
                if (Quaternion.Angle(transform.rotation, lookRotation) < 10.0f)
                {
                    _attackTime = 9999;
                    if (_mp < 100)
                    {
                        _enemyAnim.SetTrigger("Attack");
                    }
                    else
                    {
                        _enemyAnim.SetTrigger("Skill");
                    }
                }
            }
        }
        else
        {
            _enemyAI.speed = _movSpd;
            _enemyAnim.SetBool("Move", true);
        }


        _attackTime -= Time.deltaTime;
    }

    void AttackCommander()
    {
        if (Vector3.Distance(transform.position, _target.position) < _enemyData.atkRange + 1f)
        {
            _enemyAI.speed = 0;
            _enemyAnim.SetBool("Move", false);
            Quaternion lookRotation = Quaternion.LookRotation(_target.position - transform.position);
            Vector3 rotateValue = Quaternion.Lerp(transform.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
            transform.rotation = Quaternion.Euler(transform.rotation.x, rotateValue.y, transform.rotation.z);
            if (_attackTime <= 0.0f)
            {
                _attackTime = 9999;
                if (_mp < 100)
                {
                    _enemyAnim.SetTrigger("Attack");
                }
                else
                {
                    _enemyAnim.SetTrigger("Skill");
                }
            }
        }
        else
        {
            _enemyAI.speed = _movSpd;
            _enemyAnim.SetBool("Move", true);
        }

        _attackTime -= Time.deltaTime;
    }

    public virtual void TargetAttack()
    {
    }

    public virtual void AttackEnd()
    {
        _mp += _enemyData.mp;
        _statusUI.MPChange(_mp);
        _attackTime = 1 / _atkSpd;
    }

    public virtual void TargetSpecialAttack()
    {
    }

    public virtual void SkillEnd()
    {
        _mp = 0;
        _statusUI.MPChange(_mp);
        _attackTime = 1 / _atkSpd;
    }

    void Die()
    {
        _state = EStateEnemy.Die;
        _enemyAI.enabled = false;
        _enemyCllider.enabled = false;
        _blockCollider.enabled = false;
        if (gameObject.activeSelf)
        {
            StopCoroutine(BadBuffCheck());
            _enemyAnim.SetTrigger("Die");
        }
    }

    public void DieEnd()
    {
        _action = true;
        Disactive();
    }

    #endregion

    public void GetNextWayPoint(int wayPointNumber)
    {
        _wavePointIndex = wayPointNumber;
        if (_wavePointIndex >= WayPointContainer._wayPoints.Length - 1)
        {
            _state = EStateEnemy.AttackCommander;
            _target = GameObject.FindGameObjectWithTag("Commander").transform;
            _enemyAI.destination = _target.transform.position;
            return;
        }
        _wavePointIndex++;
        _enemyAI.destination = WayPointContainer._wayPoints[_wavePointIndex].position;
    }

    public override void Hit(int damage, EWeakType weakType)
    {
        if (_state == EStateEnemy.Die)
            return;
        int resultDamage = damage - _def;
        if (resultDamage <= 0)
        {
            resultDamage = 1;
        }
        _hp -= resultDamage;
        if (_hp <= 0)
        {
            _hp = 0;
        }
        _statusUI.HPChange(_hp);
        if (_hp == 0)
        {
            Die();
        }
    }

    public override void BadBuff(TestBadBuff badBuff)
    {
        _badBuffs.Add(badBuff);
    }
}
