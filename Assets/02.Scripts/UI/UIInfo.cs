using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInfo : MonoBehaviour
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
    [SerializeField] Text _obstacleSellGetTxt = null;

    [Header("EnemyInfo")]
    [SerializeField] GameObject _enemyInfoContainer = null;
    [SerializeField] Text _enemyName = null;
    [SerializeField] Image _enemyIconImage = null;

    bool _view = false;
    Tower _selectTower;
    Enemy _selectEnemy;
    Obstacle _selectObstacle;

    private void Awake()
    {
    }

    public void InstallButtonSetting()
    {
        InstallData[] installTowerDatas = ObjectDataManager.Instance.GetInstallData();
        for (int i = 0; i < installTowerDatas.Length; i++)
        {
            InstallSpawnButton towerSpawnButton = Instantiate(_prefabInstallBtn, _insBtnContainer);
            towerSpawnButton.ButtonDataSetting(installTowerDatas[i]);
            _spawnButtons.Add(towerSpawnButton);
        }
    }

    public void ClickInstallViewBtn()
    {
        InputManager.Instance.UITouch();
        if (InputManager.TouchMode != ETouchMode.Touch)
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

    public void ClickTower(Tower tower)
    {
        if (!_view)
        {
            ViewOn();
        }
        _towerInfoContainer.SetActive(true);
        _obstacleInfoContainer.SetActive(false);
        _enemyInfoContainer.SetActive(false);
        TowerUISetting(tower);
        InputManager.Instance.UITouch();
    }

    public void ClickObstacle(Obstacle obstacle)
    {
        if (!_view)
        {
            ViewOn();
        }
        _towerInfoContainer.SetActive(false);
        _obstacleInfoContainer.SetActive(true);
        _enemyInfoContainer.SetActive(false);
        ObstacleUISetting(obstacle);
        InputManager.Instance.UITouch();
    }

    public void ClickEnemy(Enemy enemy)
    {
        if (!_view)
        {
            ViewOn();
        }
        _towerInfoContainer.SetActive(false);
        _obstacleInfoContainer.SetActive(false);
        _enemyInfoContainer.SetActive(true);
        EnemyUISetting(enemy);
        InputManager.Instance.UITouch();
    }

    void TowerUISetting(Tower tower)
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
        else if (ResourceManager.Instance.TowerPartValue < tower.TowerRepairCost())
            towerRepairCheck = false;
        else
            towerRepairCheck = true;
        _towerRepairBtn.interactable = towerRepairCheck;
        _towerSellGetTxt.text = "Get " + tower.TowerGetSellNumber();
        bool upgradeDEFCheck = ResourceManager.Instance.TowerPartValue >= tower.UpgradeCost(EUpgradeType.Defence);
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

        bool upgradeATKCheck = ResourceManager.Instance.TowerPartValue >= tower.UpgradeCost(EUpgradeType.Attack);
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

        bool upgradeSPCheck = ResourceManager.Instance.TowerPartValue >= tower.UpgradeCost(EUpgradeType.Special);
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

    void ObstacleUISetting(Obstacle obstacle)
    {
        _obstacleName.text = obstacle._gameObstacleData.obstacleNameString;
        _obstacleIconImage.sprite = ObjectDataManager.Instance.GetImage(obstacle._objectName);
        _obstacleSellGetTxt.text = "Get " + obstacle._sellGetCost;
        _selectObstacle = obstacle;
    }

    void EnemyUISetting(Enemy enemy)
    {
        _enemyName.text = enemy._enemyData.enemyFullName;
        _enemyIconImage.sprite = ObjectDataManager.Instance.GetImage(enemy._objectName);
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
        int towerPartValue = ResourceManager.Instance.TowerPartValue;
        if (towerPartValue > _selectTower.TowerRepairCost())
        {
            ResourceManager.Instance.TowerPartValue = -_selectTower.TowerRepairCost();
            _selectTower.TowerRepair();
            _selectTower.TotalCostAdd(_selectTower.TowerRepairCost());
        }
        InputManager.Instance.ObjectSelectClose();
        _selectTower = null;
        InputManager.Instance.UITouch();
    }

    public void ClickTowerSell()
    {
        ResourceManager.Instance.TowerPartValue = _selectTower.TowerGetSellNumber();
        InputManager.Instance.ObjectSelectClose();
        _selectTower.SellTower();
        _selectTower = null;
        InputManager.Instance.UITouch();
    }

    public void ClickObstacleSell()
    {
        ResourceManager.Instance.TowerPartValue = _selectObstacle._sellGetCost;
        InputManager.Instance.ObjectSelectClose();
        _selectObstacle.DestroyObstacle();
        _selectObstacle = null;
        InputManager.Instance.UITouch();
    }

    public void ClickTowerUpgradeDEF()
    {
        ResourceManager.Instance.TowerPartValue = -_selectTower.UpgradeCost(EUpgradeType.Defence);
        _selectTower.TotalCostAdd(_selectTower.UpgradeCost(EUpgradeType.Defence));
        _selectTower._upgradeDEF = ObjectDataManager.Instance.GetUpgradeData(_selectTower._objectName, EUpgradeType.Defence, ++_selectTower._levelDEF);
        _selectTower.TowerUpgrade(EUpgradeType.Defence);
        InputManager.Instance.ObjectSelectClose();
        _selectTower = null;
        InputManager.Instance.UITouch();
    }

    public void ClickTowerUpgradeATK()
    {
        ResourceManager.Instance.TowerPartValue = -_selectTower.UpgradeCost(EUpgradeType.Attack);
        _selectTower.TotalCostAdd(_selectTower.UpgradeCost(EUpgradeType.Attack));
        _selectTower._upgradeATK = ObjectDataManager.Instance.GetUpgradeData(_selectTower._objectName, EUpgradeType.Attack, ++_selectTower._levelATK);
        _selectTower.TowerUpgrade(EUpgradeType.Attack);
        InputManager.Instance.ObjectSelectClose();
        _selectTower = null;
        InputManager.Instance.UITouch();
    }

    public void ClickTowerUpgradeSP()
    {
        ResourceManager.Instance.TowerPartValue = -_selectTower.UpgradeCost(EUpgradeType.Special);
        _selectTower.TotalCostAdd(_selectTower.UpgradeCost(EUpgradeType.Special));
        _selectTower._upgradeSP = ObjectDataManager.Instance.GetUpgradeData(_selectTower._objectName, EUpgradeType.Special, ++_selectTower._levelSP);
        _selectTower.TowerUpgrade(EUpgradeType.Special);
        InputManager.Instance.ObjectSelectClose();
        _selectTower = null;
        InputManager.Instance.UITouch();
    }
}
