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

public class GameUI : MonoBehaviour
{
	public static GameUI Instance { set; get; }
	public struct UIPointer
	{
		public PointerInfo pointer;
		public Ray ray;
		public RaycastHit? raycast;
		public bool overUI;
	}

	public EUIState UIState { get; private set; }

	[SerializeField] UIInfo _uIInfo = null;
	[SerializeField] UIWave _uiWave = null;
	[SerializeField] UIResource _uiResource = null;
	[SerializeField] PlayerUI _uiPlayer = null;

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

	public void TowerClick(Tower tower)
    {
		_uIInfo.ClickTower(tower);
		InputManager.Instance.UITouch();
    }

	public void ObstacleClick(Obstacle obstacle)
	{
		_uIInfo.ClickObstacle(obstacle);
		InputManager.Instance.UITouch();
	}

	public void EnemyClick(Enemy enemy)
	{
		_uIInfo.ClickEnemy(enemy);
		InputManager.Instance.UITouch();
	}

	public void StageUIInit(Wave[] waves)
    {
		_uiWave.StageEnemyUIInit(waves);
    }

	public void WaveUISetting(int wave)
    {
		_uiWave.NextWave(wave);
    }

	public void AppearanceEnemy(EObjectName objectName)
    {
		_uiWave.WaveEnemyAppearance(objectName);
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
