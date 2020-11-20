using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EStatusType
{
    HP,
    EP,
    ATK,
    DEF,
    SP,
    ATKSPD,
    ATKRANGE,
    ATKNUMBER,
    TARGETNUMBER,
    LEVELATK,
    LEVELDEF,
    LEVELSP
}

public enum ERotateAxis
{
    X,
    Y,
    Z
}

public enum ETowerState
{
    Search,
    Attack,
    Delay,
    Stun,
    Breakdown
}

public abstract class TestTower : MonoBehaviour
{
    [Header("TowerInfo")]
    public ETowerType _towerType = ETowerType.None;
    [SerializeField] protected ETowerState _towerState = ETowerState.Search;
    [SerializeField] protected int _level;
    [SerializeField] protected int _hp;
    [SerializeField] protected int _maxHP;
    [SerializeField] protected int _ep;
    [SerializeField] protected int _chargeEP;
    [SerializeField] protected int _atk;
    [SerializeField] protected int _def;
    [SerializeField] protected float _atkSpd;
    [SerializeField] protected float _atkRange;
    [SerializeField] protected int _atkNumber;
    [SerializeField] protected int _targetNumber;
    [SerializeField] protected int _levelATK;
    [SerializeField] protected int _levelDEF;
    [SerializeField] protected int _levelSP;
    [SerializeField] protected float[] spValue;
    [SerializeField] protected TestIntVector2 _dimensions;

    [Header("Setup")]

    [SerializeField] Transform _partToRotate = null;
    [SerializeField] float _turnSpeed = 10.0f;
    [SerializeField] bool _rotateCheck = true;
    [SerializeField] GameObject _rangeObject = null;
    Vector3 _rangeSize;

    TestTowerGameData _gameTowerData;
    TestTowerUpgradeData _upgradeATK = null;
    TestTowerUpgradeData _upgradeDEF = null;
    TestTowerUpgradeData _upgradeSP = null;

    string _enemyTag = "Enemy";
    protected Transform _target;

    float _attackCountdown = 0;

    bool _towerSelect = false;
    public bool _towerBulidSuccess = false;

    public TestIntVector2 gridPosition { get; private set; }

    private void Start()
    {
        _rangeSize = _rangeObject.transform.localScale;
        DataSetting();
        StartCoroutine(BulidSuccess());
        InvokeRepeating("UpdateTarget", 0.0f, 0.5f);
    }

    private void Update()
    {
        if (_target == null)
        {
            if (_towerState == ETowerState.Attack)
                _towerState = ETowerState.Search;
            else if (_towerState == ETowerState.Search)
            {
                _partToRotate.Rotate(_partToRotate.rotation.x, _turnSpeed * Time.deltaTime, _partToRotate.rotation.z);
            }
        }

        else
        {
            if (_towerState == ETowerState.Attack)
            {
                Vector3 dir = _target.position - transform.position;
                Quaternion lookRotation = Quaternion.LookRotation(dir);
                Vector3 rotateValue = Quaternion.Lerp(_partToRotate.rotation, lookRotation, Time.deltaTime * _turnSpeed).eulerAngles;
                _partToRotate.rotation = Quaternion.Euler(_partToRotate.rotation.x, rotateValue.y, _partToRotate.rotation.z);

                if (_attackCountdown <= 0)
                {
                    if (_ep == 100)
                    {
                        SpecialAttack();
                        _ep = 0;
                    }
                    else
                    {
                        Attack();
                    }
                    _attackCountdown = 1 / _atkSpd;
                }
            }
        }
        _attackCountdown -= Time.deltaTime;
        if (_attackCountdown <= 0)
        {
            _attackCountdown = 0;
        }
    }

    protected virtual void Attack()
    {
        _ep += _chargeEP;
        if (_ep > 100)
        {
            _ep = 100;
        }
    }

    protected virtual void SpecialAttack()
    {

    }

    void UpdateTarget()
    {
        if (_towerState == ETowerState.Search || _towerState == ETowerState.Attack)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(_enemyTag);
            float shortestDistance = _atkRange;
            GameObject nearestEnemy = null;
            foreach (GameObject enemy in enemies)
            {
                if (enemy.GetComponent<TestEnemy>()._state == EStateEnemy.Die)
                    continue;
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy < shortestDistance)
                {
                    if (nearestEnemy != null)
                    {
                        if ((int)nearestEnemy.GetComponent<TestEnemy>()._rating >= (int)enemy.GetComponent<TestEnemy>()._rating)
                        {
                            shortestDistance = distanceToEnemy;
                            nearestEnemy = enemy;
                        }
                    }
                    else
                    {
                        shortestDistance = distanceToEnemy;
                        nearestEnemy = enemy;
                    }

                }
            }

            if (nearestEnemy != null)
            {
                _towerState = ETowerState.Attack;
                _target = nearestEnemy.transform;
            }
            else
            {
                _target = null;
            }
        }
    }

    IEnumerator BulidSuccess()
    {
        yield return new WaitForSeconds(0.5f);
        _towerBulidSuccess = true;
    }

    public void TowerSelect(bool towerSelectOff = true)
    {
        _towerSelect = !_towerSelect;
        if (!towerSelectOff)
        {
            _towerSelect = false;
        }
        _rangeObject.SetActive(_towerSelect);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _atkRange);
    }

    #region 데이터


    void DataSetting()
    {
        _gameTowerData = TestTowerDataManager.Instance.GetTowerGameData(_towerType);
        _ep = 0;
        _levelATK = 0;
        _levelDEF = 0;
        _levelSP = 0;
        _level = 0;
        ResearchCheck();
        StatusCheck();

        _hp = _maxHP;
    }

    void StatusCheck()
    {

        if (_upgradeATK != null)
        {
            _atk = (int)(_gameTowerData.atk + _upgradeATK.addValue[0] + (_gameTowerData.atk + _upgradeATK.addValue[0]) * _gameTowerData.researchResult.atkAddRate * 0.01f);
            _atkSpd = _gameTowerData.atkSpd + _upgradeATK.addValue[1] + (_gameTowerData.atkSpd + _upgradeATK.addValue[1]) * _gameTowerData.researchResult.atkSpdRate * 0.01f;
            _atkRange = _gameTowerData.atkRange + _upgradeATK.addValue[2] + (_gameTowerData.atkRange + _upgradeATK.addValue[2]) * _gameTowerData.researchResult.atkRangeRate * 0.01f;
            _atkNumber = _gameTowerData.atkNumber + (int)_upgradeATK.addValue[3];
            _targetNumber = _gameTowerData.atkNumber + (int)_upgradeATK.addValue[4];
        }
        else
        {
            _atk = _gameTowerData.atk + (int)(_gameTowerData.atk * _gameTowerData.researchResult.atkAddRate * 0.01f);
            _atkSpd = _gameTowerData.atkSpd + _gameTowerData.atkSpd * _gameTowerData.researchResult.atkSpdRate * 0.01f;
            _atkRange = _gameTowerData.atkRange + _gameTowerData.atkRange * _gameTowerData.researchResult.atkRangeRate * 0.01f;
            _atkNumber = _gameTowerData.atkNumber;
            _targetNumber = _gameTowerData.atkNumber;
        }
        if (_upgradeDEF != null)
        {
            _maxHP = (int)(_gameTowerData.hp + _upgradeDEF.addValue[0] + (_gameTowerData.hp + _upgradeDEF.addValue[0]) * _gameTowerData.researchResult.hpAddRate * 0.01f);
            _def = (int)(_gameTowerData.def + _upgradeDEF.addValue[1] + (_gameTowerData.def + _upgradeDEF.addValue[1]) * _gameTowerData.researchResult.defAddRate * 0.01f);
            _chargeEP = (int)(_gameTowerData.ep + _upgradeDEF.addValue[2] + (_gameTowerData.ep + _upgradeDEF.addValue[2]) * _gameTowerData.researchResult.epAddRate * 0.01f);
        }
        else
        {
            _maxHP = _gameTowerData.hp + (int)(_gameTowerData.hp * _gameTowerData.researchResult.hpAddRate * 0.01f);
            _def = _gameTowerData.def + (int)(_gameTowerData.def * _gameTowerData.researchResult.defAddRate * 0.01f);
            _chargeEP = _gameTowerData.ep + (int)(_gameTowerData.ep * _gameTowerData.researchResult.epAddRate * 0.01f);
        }

        spValue = _gameTowerData.spValue;
        if (_upgradeSP != null)
        {
            for (int i = 0; i < spValue.Length; i++)
            {
                spValue[i] = _gameTowerData.spValue[i] + _upgradeSP.addValue[i];
            }
        }

        _rangeObject.transform.localScale = _rangeSize * _atkRange;
    }

    void ResearchCheck()
    {
        for (int i = 0; i < _gameTowerData.researchs.Count; i++)
        {
            switch (_gameTowerData.researchs[i])
            {
                case EResearch.AdvancedAITechnology:
                    _levelSP++;
                    _upgradeSP = TestTowerDataManager.Instance.GetUpgradeData(_towerType, EUpgradeType.Special, 1);
                    break;
            }
        }
    }

    #endregion

}
