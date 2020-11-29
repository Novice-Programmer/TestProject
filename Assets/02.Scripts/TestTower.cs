using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETowerState
{
    Search,
    Attack,
    Delay,
    Stun,
    Breakdown
}

public abstract class TestTower : ObjectHit
{
    [Header("TowerInfo")]
    public ETowerType _towerType = ETowerType.None;
    public ETowerState _towerState = ETowerState.Search;
    public int _level;
    public int _hp;
    public int _maxHP;
    [SerializeField] protected int _ep;
    [SerializeField] protected int _chargeEP;
    [SerializeField] protected int _atk;
    [SerializeField] protected int _def;
    [SerializeField] protected float _atkSpd;
    [SerializeField] protected float _atkRange;
    [SerializeField] protected int _atkNumber;
    [SerializeField] protected int _targetNumber;
    public int _levelATK;
    public int _levelDEF;
    public int _levelSP;
    [SerializeField] protected float[] _spValue;
    [SerializeField] protected TestIntVector2 _dimensions;
    [SerializeField] protected TestWorldStatusUI _statusUI;

    [Header("Setup")]
    [SerializeField] Transform _partToRotate = null;
    [SerializeField] float _turnSpeed = 10.0f;
    [SerializeField] bool _rotateCheck = true;
    [SerializeField] GameObject _rangeObject = null;
    [SerializeField] MeshRenderer[] _materials = null;
    [SerializeField] List<Material> _activeMaterials = null;
    [SerializeField] Material _breakDownMaterial = null;
    Vector3 _rangeSize;

    public TestTowerGameData _gameTowerData;
    public TestTowerUpgradeData _upgradeATK = null;
    public TestTowerUpgradeData _upgradeDEF = null;
    public TestTowerUpgradeData _upgradeSP = null;

    string _enemyTag = "Enemy";
    protected Transform[] _target;

    float _attackCountdown = 0;
    int _totalCost = 0;

    bool _towerSelect = false;
    public bool _towerBulidSuccess = false;

    TestTile _parentTile;
    TestIntVector2 _gridPosition;

    private void Start()
    {
        _rangeSize = _rangeObject.transform.localScale;
        DataSetting();
        StartCoroutine(BulidSuccess());
        InvokeRepeating("UpdateTarget", 0.0f, 0.5f);
        _materials = GetComponentsInChildren<MeshRenderer>();
        for(int i = 0; i < _materials.Length; i++)
        {
            _activeMaterials.Add(_materials[i].material);
        }
    }

    private void Update()
    {
        if(_towerState == ETowerState.Breakdown)
        {
            return;
        }
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
                Vector3 dir = Vector3.zero;
                for(int i = 0; i < _target.Length; i++)
                {
                    dir = _target[i].position - transform.position;
                }
                Quaternion lookRotation = Quaternion.LookRotation(dir/_target.Length);
                Vector3 rotateValue = Quaternion.Lerp(_partToRotate.rotation, lookRotation, Time.deltaTime * _turnSpeed).eulerAngles;
                _partToRotate.rotation = Quaternion.Euler(_partToRotate.rotation.x, rotateValue.y, _partToRotate.rotation.z);

                if (_attackCountdown <= 0)
                {
                    if (_ep == 100)
                    {
                        SpecialAttack();
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
        _statusUI.MPChange(_ep);
    }

    protected virtual void SpecialAttack()
    {
        _ep = 0;
        _statusUI.MPChange(_ep);
    }

    public override void Hit(int damage, EWeakType weakType)
    {
        if (_hp <= 0)
        {
            return;
        }

        _hp -= damage;
        if (_hp <= 0)
        {
            _hp = 0;
            Braekdown();
        }
        _statusUI.HPChange(_hp);
    }

    void Braekdown()
    {
        _towerState = ETowerState.Breakdown;
        for(int i = 0; i < _materials.Length; i++)
        {
            _materials[i].material = _breakDownMaterial;
        }
    }

    void UpdateTarget()
    {
        if (_towerState == ETowerState.Search || _towerState == ETowerState.Attack)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(_enemyTag);
            List<TestEnemy> enemiesRank = new List<TestEnemy>();

            for (int i = 0; i < enemies.Length; i++)
            {
                if (Vector3.Distance(transform.position, enemies[i].transform.position) < _atkRange)
                {
                    TestEnemy enemy = enemies[i].GetComponent<TestEnemy>();
                    if (enemy._state != EStateEnemy.Die)
                        enemiesRank.Add(enemy);
                }
            }

            if (enemiesRank.Count > 0)
            {
                _target = new Transform[_targetNumber];
                for (int i = 0; i < enemiesRank.Count-1; i++)
                {
                    for (int j = i + 1; j < enemiesRank.Count; j++)
                    {
                        if (enemiesRank[i]._rating < enemiesRank[j]._rating)
                        {
                            TestEnemy temp = enemiesRank[i];
                            enemiesRank[i] = enemiesRank[j];
                            enemiesRank[j] = temp;
                        }
                        else if (enemiesRank[i]._rating == enemiesRank[j]._rating)
                        {
                            float distanceToEnemyI = Vector3.Distance(transform.position, enemiesRank[i].transform.position);
                            float distanceToEnemyJ = Vector3.Distance(transform.position, enemiesRank[j].transform.position);
                            if (distanceToEnemyI > distanceToEnemyJ)
                            {
                                TestEnemy temp = enemiesRank[i];
                                enemiesRank[i] = enemiesRank[j];
                                enemiesRank[j] = temp;
                            }
                        }
                    }
                }

                _towerState = ETowerState.Attack;
                if (_target.Length <= enemiesRank.Count)
                {
                    for (int i = 0; i < _target.Length; i++)
                    {
                        _target[i] = enemiesRank[i].transform;
                    }
                }
                else
                {
                    for(int i = 0; i < _target.Length; i+=enemiesRank.Count)
                    {
                        for(int j = 0; j < enemiesRank.Count; j++)
                        {
                            if(i+j >= _target.Length)
                            {
                                break;
                            }
                            _target[i+j] = enemiesRank[j].transform;
                        }
                    }
                }
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
        if (_towerSelect)
        {
            TestGameUI.Instance.TowerClick(this);
        }
        else
        {
            TestGameUI.Instance.TowerViewUIOff();
        }
        _rangeObject.SetActive(_towerSelect);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, _atkRange);
    }

    public void BuildingTower(TestGhostTower ghostTower)
    {
        _parentTile = ghostTower.parentTile;
        _gridPosition = ghostTower._gridPos;
    }

    public void TowerRepair()
    {
        _hp = _maxHP;
        _statusUI.HPChange(_hp);
        if (_towerState == ETowerState.Breakdown)
        {
            _towerState = ETowerState.Search;
            for(int i = 0; i < _materials.Length; i++)
            {
                _materials[i].material = _activeMaterials[i];
            }
        }
    }

    public void SellTower()
    {
        _parentTile.Clear(_gridPosition, _dimensions);
        Destroy(gameObject);
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
        _statusUI.StatusSetting(_maxHP);
        _statusUI.HPChange(_hp);
        _totalCost = _gameTowerData.buildCost;
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

        _spValue = new float[_gameTowerData.spValue.Length];
        for (int i = 0; i < _spValue.Length; i++)
        {
            if (_upgradeSP != null)
            {
                _spValue[i] = _gameTowerData.spValue[i] + _upgradeSP.addValue[i];
            }
            else
            {
                _spValue[i] = _gameTowerData.spValue[i];
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

    public int UpgradeCost(EUpgradeType upgradeType)
    {
        int cost = 0;

        switch (upgradeType)
        {
            case EUpgradeType.Attack:
                if (_upgradeATK != null)
                {
                    cost = _upgradeATK.nextCost + _gameTowerData.atkUpgradeCost;
                }
                else
                {
                    cost = _gameTowerData.atkUpgradeCost;
                }
                break;
            case EUpgradeType.Defence:
                if (_upgradeDEF != null)
                {
                    cost = _upgradeDEF.nextCost + _gameTowerData.defUpgradeCost;
                }
                else
                {
                    cost = _gameTowerData.defUpgradeCost;
                }
                break;
            case EUpgradeType.Special:
                if (_upgradeSP != null)
                {
                    cost = _upgradeSP.nextCost + _gameTowerData.spUpgradeCost;
                }
                else
                {
                    cost = _gameTowerData.spUpgradeCost;
                }
                break;
        }

        return cost;
    }

    public int TowerRepairCost()
    {
        int cost;
        float hpRate =  1 - (float)_hp / _maxHP;
        cost = (int)(hpRate * _totalCost * 0.3f);
        return cost;
    }

    public int TowerGetSellNumber()
    {
        int sellNumber = (int)(_totalCost * 0.5f);
        return sellNumber;
    }

    public void TowerUpgrade(EUpgradeType upgradeType)
    {
        int maxHP = _maxHP;
        StatusCheck();
        if(upgradeType == EUpgradeType.Defence)
        {
            _statusUI.StatusSetting(_maxHP);
            int addHP = _maxHP - maxHP;
            _hp += addHP;
            _statusUI.HPChange(_hp);
        }
    }

    public void TotalCostAdd(int cost)
    {
        _totalCost += cost;
    }
    #endregion

}
