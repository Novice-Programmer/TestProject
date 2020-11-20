using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestTowerUpgrade : MonoBehaviour
{
    public static TestTowerUpgrade Instance { set; get; }

    [SerializeField] TestTowerUpgradeData[] _upgradeTower = null;
    Dictionary<ETowerType, Dictionary<EUpgradeType, Dictionary<int, TestTowerUpgradeData>>> _towerTypeUpgrade 
        = new Dictionary<ETowerType, Dictionary<EUpgradeType, Dictionary<int, TestTowerUpgradeData>>>();

    private void Awake()
    {
        Instance = this;
        TowerDictionarySetting();
    }

    void TowerDictionarySetting()
    {
        for (int i = 0; i < _upgradeTower.Length; i++)
        {
            Dictionary<EUpgradeType, Dictionary<int, TestTowerUpgradeData>> upgradeType;
            if (_towerTypeUpgrade.ContainsKey(_upgradeTower[i].towerType))
            {
                upgradeType = _towerTypeUpgrade[_upgradeTower[i].towerType];
            }
            else
            {
                upgradeType = new Dictionary<EUpgradeType, Dictionary<int, TestTowerUpgradeData>>();
                _towerTypeUpgrade.Add(_upgradeTower[i].towerType, upgradeType);
            }

            Dictionary<int, TestTowerUpgradeData> levelType;
            if (upgradeType.ContainsKey(_upgradeTower[i].upgradeType))
            {
                levelType = _towerTypeUpgrade[_upgradeTower[i].towerType][_upgradeTower[i].upgradeType];
            }
            else
            {
                levelType = new Dictionary<int, TestTowerUpgradeData>();
                upgradeType.Add(_upgradeTower[i].upgradeType, levelType);
            }

            levelType.Add(_upgradeTower[i].level, _upgradeTower[i]);
        }
    }

    public TestTowerUpgradeData TowerGetUpgradeData(ETowerType towerType, EUpgradeType upgradeType, int level)
    {
        return _towerTypeUpgrade[towerType][upgradeType][level];
    }
}
