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
    FireWall,
    Commander,
    Tower,
    Obstacle,
    Resource,
    ALL,
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
    [SerializeField] Sprite[] _objectBackgroundImage = null;
    [SerializeField] Ghost[] _prefabGhosts = null;

    [Header("Tower")]
    [SerializeField] TowerData[] _towerAllDatas = null;
    [SerializeField] TowerUpgradeData[] _upgradeAllData = null;
    [SerializeField] Tower[] _prefabTowers = null;

    Dictionary<EObjectName, Dictionary<EUpgradeType, Dictionary<int, TowerUpgradeData>>> _towerUpgradeDic
    = new Dictionary<EObjectName, Dictionary<EUpgradeType, Dictionary<int, TowerUpgradeData>>>();

    Dictionary<EObjectName, TowerGameData> _gameTowerDatas = new Dictionary<EObjectName, TowerGameData>();

    [Header("Obstacle")]
    [SerializeField] ObstacleData[] _obstacleAllDatas = null;
    [SerializeField] Obstacle[] _prefabObstacle = null;

    Dictionary<EObjectName, ObstacleGameData> _gameObstacleDatas = new Dictionary<EObjectName, ObstacleGameData>();

    [Header("Enemy")]
    [SerializeField] EnemyData[] _enemyAllDatas = null;
    [SerializeField] Sprite[] _enemyIconSprites = null;
    [SerializeField] Sprite[] _enemyRankSprites = null;

    [Header("Mark")]
    [SerializeField] Transform _markerContainer = null;
    [SerializeField] Marker _prefabMarker = null;
    [SerializeField] Sprite[] _markIconSprites = null;
    [SerializeField] Sprite[] _markBackgroundSprites = null;

    [Header("Status")]
    [SerializeField] Transform _statusContainer = null;
    [SerializeField] WorldStatusUI _prefabStatusUI = null;

    private void Awake()
    {
        Instance = this;
        TowerDictionarySetting();
    }

    void TowerDictionarySetting()
    {
        for (int i = 0; i < _upgradeAllData.Length; i++)
        {
            Dictionary<EUpgradeType, Dictionary<int, TowerUpgradeData>> upgradeType;
            if (_towerUpgradeDic.ContainsKey(_upgradeAllData[i].objectName))
            {
                upgradeType = _towerUpgradeDic[_upgradeAllData[i].objectName];
            }
            else
            {
                upgradeType = new Dictionary<EUpgradeType, Dictionary<int, TowerUpgradeData>>();
                _towerUpgradeDic.Add(_upgradeAllData[i].objectName, upgradeType);
            }

            Dictionary<int, TowerUpgradeData> levelType;
            if (upgradeType.ContainsKey(_upgradeAllData[i].upgradeType))
            {
                levelType = _towerUpgradeDic[_upgradeAllData[i].objectName][_upgradeAllData[i].upgradeType];
            }
            else
            {
                levelType = new Dictionary<int, TowerUpgradeData>();
                upgradeType.Add(_upgradeAllData[i].upgradeType, levelType);
            }

            levelType.Add(_upgradeAllData[i].level, _upgradeAllData[i]);
        }
    }

    public void GameInstallSetting(SelectData[] selectDatas)
    {
        for (int i = 0; i < selectDatas.Length; i++)
        {
            if (selectDatas[i].objectType == EObjectType.Tower)
            {
                TowerGameData towerGameData = new TowerGameData(GetTowerData(selectDatas[i].objectName));
                towerGameData.ResearchAdd(selectDatas[i].researchDatas);
                _gameTowerDatas.Add(selectDatas[i].objectName, towerGameData);
            }
            else
            {
                ObstacleGameData obstacleGameData = new ObstacleGameData(GetObstacleData(selectDatas[i].objectName));
                obstacleGameData.ResearchAdd(selectDatas[i].researchDatas);
                _gameObstacleDatas.Add(selectDatas[i].objectName, obstacleGameData);
            }
        }
    }

    public TowerData GetTowerData(EObjectName towerName)
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

    public ObstacleData GetObstacleData(EObjectName obstacleName)
    {
        for (int i = 0; i < _obstacleAllDatas.Length; i++)
        {
            if (obstacleName == _obstacleAllDatas[i].objectName)
            {
                return _obstacleAllDatas[i];
            }
        }
        return null;
    }

    public void ResearchCheck(EObjectName objectName, ResearchData researchData)
    {
        if (_gameTowerDatas.ContainsKey(objectName))
        {
            ResearchUpdate(objectName, researchData);
        }
    }

    void ResearchUpdate(EObjectName objectName, ResearchData researchData)
    {
        _gameTowerDatas[objectName].ResearchAdd(researchData);
    }

    public TowerUpgradeData GetUpgradeData(EObjectName objectName, EUpgradeType upgradeType, int level)
    {
        return _towerUpgradeDic[objectName][upgradeType][level];
    }

    public void GameTowerDataUpdate(EObjectName objectName, TowerGameData towerGameData)
    {
        if (_gameTowerDatas.ContainsKey(objectName))
        {
            _gameTowerDatas[objectName] = towerGameData;
        }
        else
        {
            _gameTowerDatas.Add(objectName, towerGameData);
        }
    }

    public TowerGameData GetTowerGameData(EObjectName objectName)
    {
        return _gameTowerDatas[objectName];
    }

    public ObstacleGameData GetObstacleGameData(EObjectName objectName)
    {
        return _gameObstacleDatas[objectName];
    }

    public InstallData[] GetInstallData()
    {
        List<InstallData> installDatas = new List<InstallData>();

        foreach (EObjectName objectName in _gameTowerDatas.Keys)
        {
            TowerGameData towerGameData = _gameTowerDatas[objectName];
            InstallData installTowerData = new InstallData();
            installTowerData.objectType = EObjectType.Tower;
            installTowerData.installCost = towerGameData.buildCost;
            installTowerData.objectImage = GetImage(objectName);
            installDatas.Add(installTowerData);
        }

        foreach (EObjectName objectName in _gameObstacleDatas.Keys)
        {
            ObstacleGameData obstacleGameData = _gameObstacleDatas[objectName];
            InstallData installObstacleData = new InstallData();
            installObstacleData.objectType = EObjectType.Obstacle;
            installObstacleData.installCost = obstacleGameData.buildCost;
            installObstacleData.objectImage = GetImage(objectName);
            installDatas.Add(installObstacleData);
        }

        return installDatas.ToArray();
    }

    public Ghost GetBuildGhost(EObjectName objectName)
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

    public Tower GetTower(EObjectName objectName)
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

    public Obstacle GetObstacle(EObjectName objectName)
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

    public Sprite GetImage(EObjectName objectName, bool backgroundImage = false)
    {
        int objectImageNumber = 0;
        switch (objectName)
        {
            case EObjectName.KW9A:
                break;
            case EObjectName.P013:
                break;
            case EObjectName.NMDA:
                break;
            case EObjectName.FireWall:
                break;
        }
        if (backgroundImage)
            return _objectImages[objectImageNumber];
        else
            return _objectImages[objectImageNumber];
    }

    public void MarkerSetting(Transform target, EObjectName objectName)
    {
        Marker marker = Instantiate(_prefabMarker, _markerContainer);
        Sprite icon = _markIconSprites[0];
        Sprite background = _markBackgroundSprites[0];
        switch (objectName)
        {
            case EObjectName.KW9A:
                icon = _markIconSprites[0];
                background = _markBackgroundSprites[0];
                break;
            case EObjectName.NMDA:
                icon = _markIconSprites[1];
                background = _markBackgroundSprites[1];
                break;
            case EObjectName.FireWall:
                icon = _markIconSprites[2];
                background = _markBackgroundSprites[2];
                break;
            case EObjectName.Commander:
                icon = _markIconSprites[3];
                background = _markBackgroundSprites[3];
                break;
        }

        marker.MarkerSetting(target, icon, background);
    }

    public WorldStatusUI StatusInit()
    {
        return Instantiate(_prefabStatusUI, _statusContainer);
    }
}
