using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EObjectType
{
    None,
    Enemy,
    Tower,
    Obstacle,
    Commander
}

public enum EObjectName
{
    None,
    KW9A,
    P013,
    NMDA,
    FireWall
}

public enum EImageNumber
{
    KW9A,
    FireWall
}

public class ObjectDataManager : MonoBehaviour
{
    public static ObjectDataManager Instance { set; get; }
    [SerializeField] Sprite[] _objectImages = null;
    [SerializeField] TestGhost[] _prefabGhosts = null;

    [Header("Tower")]
    [SerializeField] TestTowerData[] _towerAllDatas = null;
    [SerializeField] TestTowerUpgradeData[] _upgradeAllData = null;
    [SerializeField] TestTower[] _prefabTowers = null;

    Dictionary<ETowerType, Dictionary<EUpgradeType, Dictionary<int, TestTowerUpgradeData>>> _towerUpgradeDic
    = new Dictionary<ETowerType, Dictionary<EUpgradeType, Dictionary<int, TestTowerUpgradeData>>>();

    Dictionary<ETowerType, TestTowerGameData> _gameTowerDatas = new Dictionary<ETowerType, TestTowerGameData>();

    [Header("Obstacle")]
    [SerializeField] TestObstacleData[] _obstacleAllDatas = null;
    [SerializeField] TestObstacle[] _prefabObstacle = null;

    Dictionary<EObstacleType, TestObstacleGameData> _gameObstacleDatas = new Dictionary<EObstacleType, TestObstacleGameData>();

    [Header("Enemy")]
    [SerializeField] TestEnemyData[] _enemyAllDatas;
    [SerializeField] Sprite[] _enemyIconSprites = null;
    [SerializeField] Sprite[] _enemyRankSprites = null;

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

    public void GameInstallSetting(SelectTowerData[] selectTowerDatas, SelectObstacleData[] selectObstacleDatas)
    {
        for (int i = 0; i < selectTowerDatas.Length; i++)
        {
            TestTowerGameData towerGameData = new TestTowerGameData(GetTowerData(selectTowerDatas[i].towerType));
            towerGameData.ResearchAdd(selectTowerDatas[i].researchDatas);
            _gameTowerDatas.Add(selectTowerDatas[i].towerType, towerGameData);
        }

        for (int i = 0; i < selectObstacleDatas.Length; i++)
        {
            TestObstacleGameData obstacleGameData = new TestObstacleGameData(GetObstacleData(selectObstacleDatas[i].obstacleType));
            obstacleGameData.ResearchAdd(selectObstacleDatas[i].researchDatas);
            _gameObstacleDatas.Add(selectObstacleDatas[i].obstacleType, obstacleGameData);
        }
    }

    public TestTowerData GetTowerData(ETowerType towerType)
    {
        for (int i = 0; i < _towerAllDatas.Length; i++)
        {
            if (towerType == _towerAllDatas[i].towerType)
            {
                return _towerAllDatas[i];
            }
        }
        return null;
    }

    public TestTowerData GetTowerData(EObjectName towerName)
    {
        for (int i = 0; i < _towerAllDatas.Length; i++)
        {
            if (towerName == _towerAllDatas[i].objectName)
            {
                return _towerAllDatas[i];
            }
        }
        return null;
    }

    public TestObstacleData GetObstacleData(EObstacleType obstacleType)
    {
        for (int i = 0; i < _obstacleAllDatas.Length; i++)
        {
            if (obstacleType == _obstacleAllDatas[i].obstacleType)
            {
                return _obstacleAllDatas[i];
            }
        }
        return null;
    }

    public void ResearchCheck(ETowerType towerType, TestResearchData researchData)
    {
        if (_gameTowerDatas.ContainsKey(towerType))
        {
            ResearchUpdate(towerType, researchData);
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

    public TestObstacleGameData GetObstacleGameData(EObstacleType obstacleType)
    {
        return _gameObstacleDatas[obstacleType];
    }

    public TestInstallData[] GetInstallData()
    {
        List<TestInstallData> installDatas = new List<TestInstallData>();

        foreach (ETowerType towerType in _gameTowerDatas.Keys)
        {
            TestTowerGameData towerGameData = _gameTowerDatas[towerType];
            TestInstallData installTowerData = new TestInstallData();
            installTowerData.objectType = EObjectType.Tower;
            installTowerData.objectName = ObjectNameCheck(towerGameData.towerType);
            installTowerData.installCost = towerGameData.buildCost;
            installTowerData.objectImage = GetImage(towerGameData.objectName);
            installDatas.Add(installTowerData);
        }

        foreach (EObstacleType obstacleType in _gameObstacleDatas.Keys)
        {
            TestObstacleGameData obstacleGameData = _gameObstacleDatas[obstacleType];
            TestInstallData installObstacleData = new TestInstallData();
            installObstacleData.objectType = EObjectType.Obstacle;
            installObstacleData.objectName = ObjectNameCheck(obstacleGameData.obstacleType);
            installObstacleData.installCost = obstacleGameData.buildCost;
            installObstacleData.objectImage = GetImage(obstacleGameData.objectName);
            installDatas.Add(installObstacleData);
        }

        return installDatas.ToArray();
    }

    public TestGhost GetBuildGhost(EObjectName objectName)
    {
        for (int i = 0; i < _prefabGhosts.Length; i++)
        {
            if (objectName == _prefabGhosts[i]._objectName)
            {
                return _prefabGhosts[i];
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

    public TestTower GetTower(EObjectName objectName)
    {
        for (int i = 0; i < _prefabTowers.Length; i++)
        {
            if (_prefabTowers[i]._objectName == objectName)
            {
                return _prefabTowers[i];
            }
        }
        return null;
    }

    public TestObstacle GetObstacle(EObjectName objectName)
    {
        for (int i = 0; i < _prefabObstacle.Length; i++)
        {
            if (_prefabObstacle[i]._objectName == objectName)
            {
                return _prefabObstacle[i];
            }
        }
        return null;
    }

    public Sprite GetImage(EObjectName objectName)
    {
        int objectImageNumber = 0;
        switch (objectName)
        {
            case EObjectName.KW9A:
                objectImageNumber = 0;
                break;
            case EObjectName.P013:
                break;
            case EObjectName.NMDA:
                break;
            case EObjectName.FireWall:
                objectImageNumber = 1;
                break;
        }
        return _objectImages[objectImageNumber];
    }

    public Sprite GetEnemyImage(EEnemyType enemyType, bool rankImage)
    {
        if (rankImage)
        {
            for (int i = 0; i < _enemyAllDatas.Length; i++)
            {
                if (_enemyAllDatas[i].enemyName == enemyType)
                    return _enemyRankSprites[(int)_enemyAllDatas[i].ratingType];
            }
        }
        else
        {
            return _enemyIconSprites[(int)enemyType];
        }
        return _enemyRankSprites[0];
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
