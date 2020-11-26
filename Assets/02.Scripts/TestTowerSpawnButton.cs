using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TestTowerSpawnButton : MonoBehaviour
{
    [SerializeField] TestInstallTowerData _towerState;
    [SerializeField] GameObject _lockBtn = null;
    [SerializeField] Image _towerImage = null;
    [SerializeField] Text _partCostTxt = null;

    int _installCost = 0;

    public void TowerButtonSetting(TestInstallTowerData towerState)
    {
        _towerState = towerState;
        _towerImage.sprite = _towerState.towerImage;
        _installCost = _towerState.towerInstallCost;
        _partCostTxt.text = _towerState.towerInstallCost.ToString();
    }

    public void TowerMoneyCheck()
    {
        if (TestResourceManager.Instance.TowerPartValue > _installCost)
        {
            _partCostTxt.color = Color.black;
            _lockBtn.SetActive(false);
        }
        else
        {
            _partCostTxt.color = Color.red;
            _lockBtn.SetActive(true);
        }
    }

    public void TowerClick()
    {
        TestGameUI.Instance.TowerViewUIOff();
        TestGameManager.Instance.TowerInstall(_towerState.towerType, _towerState.towerInstallCost);
    }
}
