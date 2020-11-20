using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUITWIN : MonoBehaviour
{
    [SerializeField] TestTowerSpawnButton _prefabTowerInsBtn = null;
    [SerializeField] Transform _insBtnContainer = null;
    TestTowerSpawnButton[] _spawnButtons;
    [SerializeField] Animator _viewAnimator = null;

	bool _towerView = false;

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
        if (_towerView)
        {
            TowerViewOff();
        }
        else
        {
            TowerViewOn();
        }
    }

    public void UIValueChange()
    {
        for(int i = 0; i < _spawnButtons.Length; i++)
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
}
