using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectType
{
    None,
    Enemy,
    Tower,
    Obstacle
}

public enum EObjectName
{
    None,
    KW9A,
    P013,
    NMDA,
    FireWall
}

public class ObjectDataManager : MonoBehaviour
{
    public static ObjectDataManager Instance { set; get; }

    [Header("Tower")]
    [SerializeField] TestTowerData[] _towerAllDatas = null;
    [SerializeField] TestTowerUpgradeData[] _upgradeAllData = null;
    [SerializeField] Sprite[] _towerImages = null;
    [SerializeField] TestTower[] _prefabTowers = null;
    [SerializeField] TestGhost[] _prefabGhostTowers = null;

    Dictionary<ETowerType, Dictionary<EUpgradeType, Dictionary<int, TestTowerUpgradeData>>> _towerUpgradeDic
    = new Dictionary<ETowerType, Dictionary<EUpgradeType, Dictionary<int, TestTowerUpgradeData>>>();

    Dictionary<ETowerType, TestTowerGameData> _gameTowerDatas = new Dictionary<ETowerType, TestTowerGameData>();

    [Header("Obstacle")]
    [SerializeField] TestObstacleData[] _obstacleAllDatas = null;
    [SerializeField] Sprite[] _obstacleImages = null;
    [SerializeField] TestObstacle[] _prefabObstacle = null;
    [SerializeField] TestGhostObstacle[] _prefabGhostObstacles = null;

    private void Awake()
    {
        Instance = this;
        TowerDictionarySetting();
    }

    void TowerDictionarySetting()
    {
        for (int i = 0; i < _upgradeAllData.Length; i++)
        {
            Dictionary<EUpgradeType, Dictionary<int, TestTowerUpgradeData>> upgradeType;
            if (_towerUpgradeDic.ContainsKey(_upgradeAllData[i].towerType))
            {
                upgradeType = _towerUpgradeDic[_upgradeAllData[i].towerType];
            }
            else
            {
                upgradeType = new Dictionary<EUpgradeType, Dictionary<int, TestTowerUpgradeData>>();
                _towerUpgradeDic.Add(_upgradeAllData[i].towerType, upgradeType);
            }

            Dictionary<int, TestTowerUpgradeData> levelType;
            if (upgradeType.ContainsKey(_upgradeAllData[i].upgradeType))
            {
                levelType = _towerUpgradeDic[_upgradeAllData[i].towerType][_upgradeAllData[i].upgradeType];
            }
            else
            {
                levelType = new Dictionary<int, TestTowerUpgradeData>();
                upgradeType.Add(_upgradeAllData[i].upgradeType, levelType);
            }

            levelType.Add(_upgradeAllData[i].level, _upgradeAllData[i]);
        }
    }

    public void GameTowerSetting(TestSelectTowerData[] selectTowerDatas)
    {
        for(int i = 0; i < selectTowerDatas.Length; i++)
        {
            TestTowerGameData towerGameData = new TestTowerGameData(GetTowerData(selectTowerDatas[i].towerType));
            towerGameData.ResearchAdd(selectTowerDatas[i].researchDatas);
            _gameTowerDatas.Add(selectTowerDatas[i].towerType, towerGameData);
        }
    }

    public TestTowerData GetTowerData(ETowerType towerType)
    {
        for(int i = 0; i < _towerAllDatas.Length; i++)
        {
            if(towerType == _towerAllDatas[i].towerType)
            {
                return _towerAllDatas[i];
            }
        }
        return null;
    }

    public void ResearchCheck(ETowerType towerType,TestResearchData researchData)
    {
        if (_gameTowerDatas.ContainsKey(towerType))
        {
            ResearchUpdate(towerType,researchData);
        }
        else
        {

        }
    }

    void ResearchUpdate(ETowerType towerType, TestResearchData researchData)
    {
        _gameTowerDatas[towerType].ResearchAdd(researchData);
    }

    public TestTowerUpgradeData GetUpgradeData(ETowerType towerType, EUpgradeType upgradeType, int level)
    {
        return _towerUpgradeDic[towerType][upgradeType][level];
    }

    public void GameTowerDataUpdate(ETowerType towerType, TestTowerGameData towerGameData)
    {
        if (_gameTowerDatas.ContainsKey(towerType))
        {
            _gameTowerDatas[towerType] = towerGameData;
        }
        else
        {
            _gameTowerDatas.Add(towerType, towerGameData);
        }
    }

    public TestTowerGameData GetTowerGameData(ETowerType towerType)
    {
        return _gameTowerDatas[towerType];
    }

    public TestInstallData[] GetInstallData()
    {
        List<TestInstallData> installTowerDatas = new List<TestInstallData>();

        foreach(ETowerType towerType in _gameTowerDatas.Keys)
        {
            TestTowerGameData towerGameData = _gameTowerDatas[towerType];
            TestInstallData installTowerData = new TestInstallData();
            installTowerData.objectType = EObjectType.Tower;
            installTowerData.objectName = ObjectNameCheck(towerGameData.towerType);
            installTowerData.installCost = towerGameData.buildCost;
            installTowerData.objectImage = _towerImages[(int)towerType];
            installTowerDatas.Add(installTowerData);
        }

        return installTowerDatas.ToArray();
    }

    public TestGhost GetBulidGhost(ETowerType towerType)
    {
        for(int i = 0; i < _prefabGhostTowers.Length; i++)
        {
            if(_prefabGhostTowers[i]._towerType == towerType)
            {
                return _prefabGhostTowers[i];
            }
        }
        return null;
    }

    public TestTower GetTower(ETowerType towerType)
    {
        for (int i = 0; i < _prefabTowers.Length; i++)
        {
            if (_prefabTowers[i]._towerType == towerType)
            {
                return _prefabTowers[i];
            }
        }
        return null;
    }

    public Sprite GetTowerImage(EObjectType towerType)
    {
        return _towerImages[(int)towerType];
    }

    EObjectName ObjectNameCheck(ETowerType towerType)
    {
        switch (towerType)
        {
            case ETowerType.KW9A:
                return EObjectName.KW9A;
            case ETowerType.P013:
                return EObjectName.P013;
            case ETowerType.None:
                return EObjectName.None;
            default:
                return EObjectName.None;
        }
    }

    EObjectName ObjectNameCheck(EObstacleType obstacleType)
    {
        switch (obstacleType)
        {
            case EObstacleType.FireWall:
                return EObjectName.FireWall;
            case EObstacleType.None:
                return EObjectName.None;
            default:
                return EObjectName.None;
        }
    }
}
