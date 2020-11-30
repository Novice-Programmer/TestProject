using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InstallSpawnButton : MonoBehaviour
{
    [SerializeField] TestInstallData _buttonData;
    [SerializeField] GameObject _lockBtn = null;
    [SerializeField] Image _towerImage = null;
    [SerializeField] Text _partCostTxt = null;

    int _installCost = 0;

    public void ButtonDataSetting(TestInstallData buttonData)
    {
        _buttonData = buttonData;
        _towerImage.sprite = _buttonData.objectImage;
        _installCost = _buttonData.installCost;
        _partCostTxt.text = _buttonData.installCost.ToString();
    }

    public void InstallMoneyCheck()
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

    public void InstallClick()
    {
        TestGameUI.Instance.ViewUIOff();
        TestGameManager.Instance.Install(_buttonData.objectType, _buttonData.objectName, _buttonData.installCost);
        TestInputManager.Instance.UITouch();
    }
}
