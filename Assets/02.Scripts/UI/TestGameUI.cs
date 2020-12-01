using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum EUIState
{
	Normal,
	Building,
	Paused,
	GameOver,
	BuildingWithDrag
}

public class TestGameUI : MonoBehaviour
{
	public static TestGameUI Instance { set; get; }
	public struct UIPointer
	{
		public TestPointerInfo pointer;
		public Ray ray;
		public RaycastHit? raycast;
		public bool overUI;
	}

	public EUIState UIState { get; private set; }

	[SerializeField] TestUIInfo _uIInfo = null;
	[SerializeField] TestUIWave _uiWave = null;
	[SerializeField] TestUIResource _uiResource = null;
	[SerializeField] TestPlayerUI _uiPlayer = null;

    private void Awake()
    {
		Instance = this;
    }

	public void GameUISetting()
	{
		_uIInfo.InstallButtonSetting();
	}

    public void ViewUIOff()
    {
        _uIInfo.ViewOff();
    }

	public void ResourceValueChange()
    {
		_uIInfo.UIValueChange();
		_uiResource.UIValueChange();
    }

	public void TowerClick(TestTower tower)
    {
		_uIInfo.ClickTower(tower);
		TestInputManager.Instance.UITouch();
    }

	public void ObstacleClick(TestObstacle obstacle)
	{
		_uIInfo.ClickObstacle(obstacle);
		TestInputManager.Instance.UITouch();
	}

	public void EnemyClick(TestEnemy enemy)
	{
		_uIInfo.ClickEnemy(enemy);
		TestInputManager.Instance.UITouch();
	}

	public void StageUIInit(TestWave[] waves)
    {
		_uiWave.StageEnemyUIInit(waves);
    }

	public void WaveUISetting(int wave)
    {
		_uiWave.NextWave(wave);
    }

	public void AppearanceEnemy(EEnemyType enemyType)
    {
		_uiWave.WaveEnemyAppearance(enemyType);
    }

	public void WaveClear(int wave)
    {
		_uiWave.NextWave(wave);
    }

	public void StageClear()
    {

    }

	public void CommanderSetting(int maxHP)
    {
		_uiPlayer.HPSetting(maxHP);
    }

	public void CommanderHit(int hp)
    {
		_uiPlayer.ChangeHP(hp);
    }

	public void InfoViewOff()
    {
		_uIInfo.ViewOff();
    }
}
