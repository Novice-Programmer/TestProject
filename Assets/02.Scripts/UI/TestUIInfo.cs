using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIInfo : MonoBehaviour
{
    [SerializeField] InstallSpawnButton _prefabInstallBtn = null;
    [SerializeField] Transform _insBtnContainer = null;
    List<InstallSpawnButton> _spawnButtons = new List<InstallSpawnButton>();
    [SerializeField] Animator _viewAnimator = null;
    [Header("TowerInfo")]
    [SerializeField] GameObject _towerInfoContainer = null;
    [SerializeField] Text _towerName = null;
    [SerializeField] Image _towerIconImage = null;
    [SerializeField] Button _towerRepairBtn = null;
    [SerializeField] Text _towerRepairCostTxt = null;
    [SerializeField] Text _towerSellGetTxt = null;
    [SerializeField] Button _towerUpgradeDEFBtn = null;
    [SerializeField] Text _towerDEFTxt = null;
    [SerializeField] Button _towerUpgradeATKBtn = null;
    [SerializeField] Text _towerATKTxt = null;
    [SerializeField] Button _towerUpgradeSPBtn = null;
    [SerializeField] Text _towerSPTxt = null;

    [Header("ObstacleInfo")]
    [SerializeField] GameObject _obstacleInfoContainer = null;
    [SerializeField] Text _obstacleName = null;
    [SerializeField] Image _obstacleIconImage = null;
    [SerializeField] Button _obstacleSellBtn = null;
    [SerializeField] Text _obstacleSellGetTxt = null;

    [Header("EnemyInfo")]
    [SerializeField] GameObject _enemyInfoContainer = null;
    [SerializeField] Text _enemyName = null;
    [SerializeField] Image _enemyIconImage = null;

    bool _view = false;
    TestTower _selectTower;
    TestEnemy _selectEnemy;
    TestObstacle _selectObstacle;

    private void Awake()
    {
    }

    public void InstallButtonSetting()
    {
        TestInstallData[] installTowerDatas = ObjectDataManager.Instance.GetInstallData();
        for (int i = 0; i < installTowerDatas.Length; i++)
        {
            InstallSpawnButton towerSpawnButton = Instantiate(_prefabInstallBtn, _insBtnContainer);
            towerSpawnButton.ButtonDataSetting(installTowerDatas[i]);
            _spawnButtons.Add(towerSpawnButton);
        }
    }

    public void ClickInstallViewBtn()
    {
        TestInputManager.Instance.UITouch();
        if (TestInputManager.TouchMode != ETouchMode.Touch)
        {
            return;
        }
        _towerInfoContainer.SetActive(false);
        _obstacleInfoContainer.SetActive(false);
        _enemyInfoContainer.SetActive(false);
        if (_view)
        {
            ViewOff();
        }
        else
        {
            ViewOn();
        }
    }

    public void ClickTower(TestTower tower)
    {
        if (!_view)
        {
            ViewOn();
        }
        _towerInfoContainer.SetActive(true);
        _obstacleInfoContainer.SetActive(false);
        _enemyInfoContainer.SetActive(false);
        TowerUISetting(tower);
        TestInputManager.Instance.UITouch();
    }

    public void ClickObstacle(TestObstacle obstacle)
    {
        if (!_view)
        {
            ViewOn();
        }
        _towerInfoContainer.SetActive(false);
        _obstacleInfoContainer.SetActive(true);
        _enemyInfoContainer.SetActive(false);
        ObstacleUISetting(obstacle);
        TestInputManager.Instance.UITouch();
    }

    public void ClickEnemy(TestEnemy enemy)
    {
        if (!_view)
        {
            ViewOn();
        }
        _towerInfoContainer.SetActive(false);
        _obstacleInfoContainer.SetActive(false);
        _enemyInfoContainer.SetActive(true);
        EnemyUISetting(enemy);
        TestInputManager.Instance.UITouch();
    }

    void TowerUISetting(TestTower tower)
    {
        _towerName.text = tower._gameTowerData.towerName;
        _towerIconImage.sprite = ObjectDataManager.Instance.GetImage(tower._objectName);
        bool towerRepairCheck;
        _towerRepairCostTxt.text = "Cost " + tower.TowerRepairCost();
        if (tower._hp > (tower._maxHP * 0.9f))
        {
            towerRepairCheck = false;
            _towerRepairCostTxt.text = "내구도 충분";
        }
        else if (TestResourceManager.Instance.TowerPartValue < tower.TowerRepairCost())
            towerRepairCheck = false;
        else
            towerRepairCheck = true;
        _towerRepairBtn.interactable = towerRepairCheck;
        _towerSellGetTxt.text = "Get " + tower.TowerGetSellNumber();
        bool upgradeDEFCheck = TestResourceManager.Instance.TowerPartValue >= tower.UpgradeCost(EUpgradeType.Defence);
        if (tower._upgradeDEF != null)
        {
            upgradeDEFCheck &= tower._gameTowerData.maxUpgrade >= tower._upgradeDEF.level;
            if (tower._gameTowerData.maxUpgrade >= tower._upgradeDEF.level)
                _towerDEFTxt.text = "DEF Upgrade " + tower.UpgradeCost(EUpgradeType.Defence);
            else
                _towerDEFTxt.text = "Max DEF Upgrade";
        }

        else
        {
            _towerDEFTxt.text = "DEF Upgrade " + tower.UpgradeCost(EUpgradeType.Defence);
        }
        _towerUpgradeDEFBtn.interactable = upgradeDEFCheck;

        bool upgradeATKCheck = TestResourceManager.Instance.TowerPartValue >= tower.UpgradeCost(EUpgradeType.Attack);
        if (tower._upgradeATK != null)
        {
            upgradeATKCheck &= tower._gameTowerData.maxUpgrade >= tower._upgradeATK.level;
            if (tower._gameTowerData.maxUpgrade >= tower._upgradeATK.level)
                _towerATKTxt.text = "ATK Upgrade " + tower.UpgradeCost(EUpgradeType.Attack);
            else
                _towerATKTxt.text = "Max ATK Upgrade";
        }
        else
        {
            _towerATKTxt.text = "ATK Upgrade " + tower.UpgradeCost(EUpgradeType.Attack);
        }
        _towerUpgradeATKBtn.interactable = upgradeATKCheck;

        bool upgradeSPCheck = TestResourceManager.Instance.TowerPartValue >= tower.UpgradeCost(EUpgradeType.Special);
        if (tower._upgradeSP != null)
        {
            upgradeSPCheck &= tower._gameTowerData.spMaxUpgrade >= tower._upgradeSP.level;
            if (tower._gameTowerData.spMaxUpgrade >= tower._upgradeSP.level)
                _towerSPTxt.text = "SP Upgrade " + tower.UpgradeCost(EUpgradeType.Special);
            else
                _towerSPTxt.text = "Max SP Upgrade";
        }
        else
        {
            _towerSPTxt.text = "SP Upgrade " + tower.UpgradeCost(EUpgradeType.Special);
        }
        _towerUpgradeSPBtn.interactable = upgradeSPCheck;
        _selectTower = tower;
    }

    void ObstacleUISetting(TestObstacle obstacle)
    {
        _obstacleName.text = obstacle._gameObstacleData.obstacleNameString;
        _obstacleIconImage.sprite = ObjectDataManager.Instance.GetImage(obstacle._objectName);
        _obstacleSellGetTxt.text = "Get " + obstacle._sellGetCost;
        _selectObstacle = obstacle;
    }

    void EnemyUISetting(TestEnemy enemy)
    {
        _enemyName.text = enemy._enemyData.enemyFullName;
        _enemyIconImage.sprite = ObjectDataManager.Instance.GetEnemyImage(enemy._enemyData.enemyName, false);
    }

    public void UIValueChange()
    {
        for (int i = 0; i < _spawnButtons.Count; i++)
        {
            _spawnButtons[i].InstallMoneyCheck();
        }
    }

    public void ViewOn()
    {
        _view = true;
        _viewAnimator.SetBool("View", true);
    }

    public void ViewOff()
    {
        _view = false;
        _viewAnimator.SetBool("View", false);
    }

    public void ClickTowerRepair()
    {
        int towerPartValue = TestResourceManager.Instance.TowerPartValue;
        if (towerPartValue > _selectTower.TowerRepairCost())
        {
            TestResourceManager.Instance.TowerPartValue = -_selectTower.TowerRepairCost();
            _selectTower.TowerRepair();
            _selectTower.TotalCostAdd(_selectTower.TowerRepairCost());
        }
        TestInputManager.Instance.ObjectSelectClose();
        _selectTower = null;
        TestInputManager.Instance.UITouch();
    }

    public void ClickTowerSell()
    {
        TestResourceManager.Instance.TowerPartValue = _selectTower.TowerGetSellNumber();
        TestInputManager.Instance.ObjectSelectClose();
        _selectTower.SellTower();
        _selectTower = null;
        TestInputManager.Instance.UITouch();
    }

    public void ClickObstacleSell()
    {
        TestResourceManager.Instance.TowerPartValue = _selectObstacle._sellGetCost;
        TestInputManager.Instance.ObjectSelectClose();
        _selectObstacle.DestroyObstacle();
        _selectObstacle = null;
        TestInputManager.Instance.UITouch();
    }

    public void ClickTowerUpgradeDEF()
    {
        TestResourceManager.Instance.TowerPartValue = -_selectTower.UpgradeCost(EUpgradeType.Defence);
        _selectTower.TotalCostAdd(_selectTower.UpgradeCost(EUpgradeType.Defence));
        _selectTower._upgradeDEF = ObjectDataManager.Instance.GetUpgradeData(_selectTower._towerType, EUpgradeType.Defence, ++_selectTower._levelDEF);
        _selectTower.TowerUpgrade(EUpgradeType.Defence);
        TestInputManager.Instance.ObjectSelectClose();
        _selectTower = null;
        TestInputManager.Instance.UITouch();
    }

    public void ClickTowerUpgradeATK()
    {
        TestResourceManager.Instance.TowerPartValue = -_selectTower.UpgradeCost(EUpgradeType.Attack);
        _selectTower.TotalCostAdd(_selectTower.UpgradeCost(EUpgradeType.Attack));
        _selectTower._upgradeATK = ObjectDataManager.Instance.GetUpgradeData(_selectTower._towerType, EUpgradeType.Attack, ++_selectTower._levelATK);
        _selectTower.TowerUpgrade(EUpgradeType.Attack);
        TestInputManager.Instance.ObjectSelectClose();
        _selectTower = null;
        TestInputManager.Instance.UITouch();
    }

    public void ClickTowerUpgradeSP()
    {
        TestResourceManager.Instance.TowerPartValue = -_selectTower.UpgradeCost(EUpgradeType.Special);
        _selectTower.TotalCostAdd(_selectTower.UpgradeCost(EUpgradeType.Special));
        _selectTower._upgradeSP = ObjectDataManager.Instance.GetUpgradeData(_selectTower._towerType, EUpgradeType.Special, ++_selectTower._levelSP);
        _selectTower.TowerUpgrade(EUpgradeType.Special);
        TestInputManager.Instance.ObjectSelectClose();
        _selectTower = null;
        TestInputManager.Instance.UITouch();
    }
}
