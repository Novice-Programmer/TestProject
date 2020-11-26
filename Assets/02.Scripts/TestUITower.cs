using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUITower : MonoBehaviour
{
    [SerializeField] TestTowerSpawnButton _prefabTowerInsBtn = null;
    [SerializeField] Transform _insBtnContainer = null;
    TestTowerSpawnButton[] _spawnButtons;
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

    bool _towerView = false;
    TestTower _selectTower;

    private void Awake()
    {
    }

    public void TowerInstallButtonAdd()
    {
        TestInstallTowerData[] installTowerDatas = TestTowerDataManager.Instance.GetInstallTower();
        _spawnButtons = new TestTowerSpawnButton[installTowerDatas.Length];
        for (int i = 0; i < installTowerDatas.Length; i++)
        {
            TestTowerSpawnButton towerSpawnButton = Instantiate(_prefabTowerInsBtn, _insBtnContainer);
            towerSpawnButton.TowerButtonSetting(installTowerDatas[i]);
            _spawnButtons[i] = towerSpawnButton;
        }
    }

    public void ClickTowerViewBtn()
    {
        _towerInfoContainer.gameObject.SetActive(false);
        if (_towerView)
        {
            TowerViewOff();
        }
        else
        {
            TowerViewOn();
        }
    }

    public void ClickTower(TestTower tower)
    {
        if (!_towerView)
        {
            TowerViewOn();
        }
        _towerInfoContainer.SetActive(true);
        TowerUISetting(tower);
    }

    void TowerUISetting(TestTower tower)
    {
        _towerName.text = tower._gameTowerData.towerName;
        _towerIconImage.sprite = TestTowerDataManager.Instance.GetTowerImage(tower._towerType);
        _towerRepairBtn.interactable = tower._maxHP - tower._hp > (tower._hp * 0.1f) && TestResourceManager.Instance.TowerPartValue >= tower.TowerRepairCost();
        if (_towerRepairBtn.interactable)
        {
            _towerRepairCostTxt.text = "Cost " + tower.TowerRepairCost();
        }
        else
            _towerRepairCostTxt.text = "수리 불가";
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

    public void UIValueChange()
    {
        for (int i = 0; i < _spawnButtons.Length; i++)
        {
            _spawnButtons[i].TowerMoneyCheck();
        }
    }

    public void TowerViewOn()
    {
        _towerView = true;
        _viewAnimator.SetBool("View", true);
    }

    public void TowerViewOff()
    {
        _towerView = false;
        _viewAnimator.SetBool("View", false);
    }

    public void ClickTowerRepair()
    {
        int towerPartValue = TestResourceManager.Instance.TowerPartValue;
        if (towerPartValue > _selectTower.TowerRepairCost())
        {
            TestResourceManager.Instance.TowerPartValue = -_selectTower.TowerRepairCost();
            _selectTower.TowerRepair();
        }
        TestInputManager.Instance.TowerSelectClose();
        _selectTower = null;
    }

    public void ClickTowerSell()
    {
        TestResourceManager.Instance.TowerPartValue = _selectTower.TowerGetSellNumber();
        TestInputManager.Instance.TowerSelectClose();
        _selectTower.SellTower();
        _selectTower = null;
    }

    public void ClickTowerUpgradeDEF()
    {
        TestResourceManager.Instance.TowerPartValue = -_selectTower.UpgradeCost(EUpgradeType.Defence);
        _selectTower._upgradeDEF = TestTowerDataManager.Instance.GetUpgradeData(_selectTower._towerType, EUpgradeType.Defence, ++_selectTower._levelDEF);
        _selectTower.StatusCheck();
        TestInputManager.Instance.TowerSelectClose();
        _selectTower = null;
    }

    public void ClickTowerUpgradeATK()
    {
        TestResourceManager.Instance.TowerPartValue = -_selectTower.UpgradeCost(EUpgradeType.Attack);
        _selectTower._upgradeATK = TestTowerDataManager.Instance.GetUpgradeData(_selectTower._towerType, EUpgradeType.Attack, ++_selectTower._levelATK);
        _selectTower.StatusCheck();
        TestInputManager.Instance.TowerSelectClose();
        _selectTower = null;
    }

    public void ClickTowerUpgradeSP()
    {
        TestResourceManager.Instance.TowerPartValue = -_selectTower.UpgradeCost(EUpgradeType.Special);
        _selectTower._upgradeSP = TestTowerDataManager.Instance.GetUpgradeData(_selectTower._towerType, EUpgradeType.Special, ++_selectTower._levelSP);
        _selectTower.StatusCheck();
        TestInputManager.Instance.TowerSelectClose();
        _selectTower = null;
    }
}
