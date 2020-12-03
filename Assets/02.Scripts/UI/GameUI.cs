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

	[SerializeField] UIResource _uiResource = null;
	[SerializeField] UIInfo _uiInfo = null;
	[SerializeField] UIMap _uiMap = null;
	[SerializeField] UIWave _uiWave = null;
	[SerializeField] UIPlayer _uiPlayer = null;

    private void Awake()
    {
		Instance = this;
    }

	public void GameUISetting()
	{
		_uiInfo.InstallButtonSetting();
	}

    public void ViewUIOff()
    {
        _uiInfo.ViewOff();
    }

	public void ResourceValueChange()
    {
		_uiInfo.UIValueChange();
		_uiResource.UIValueChange();
    }

	public void TowerClick(Tower tower)
    {
		_uiInfo.ClickTower(tower);
		InputManager.Instance.UITouch();
    }

	public void ObstacleClick(Obstacle obstacle)
	{
		_uiInfo.ClickObstacle(obstacle);
		InputManager.Instance.UITouch();
	}

	public void EnemyClick(Enemy enemy)
	{
		_uiInfo.ClickEnemy(enemy);
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
		_uiMap.WaveSetting(wave);
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
		_uiInfo.ViewOff();
    }

	public void EnemyDie()
    {
		_uiWave.WaveEnemyDie();
    }
}
