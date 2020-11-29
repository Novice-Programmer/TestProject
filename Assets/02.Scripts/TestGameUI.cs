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

	[SerializeField] TestUITower _uITower = null;
	[SerializeField] TestUIWave _uiWave = null;
	[SerializeField] TestUIResource _uiResource = null;
	[SerializeField] TestPlayerUI _uiPlayer = null;

    private void Awake()
    {
		Instance = this;
    }

	public void GameUISetting()
	{
		_uITower.TowerInstallButtonAdd();
	}

    public void TowerViewUIOff()
    {
        _uITower.TowerViewOff();
    }

	public void ResourceValueChange()
    {
		_uITower.UIValueChange();
		_uiResource.UIValueChange();
    }

	public void TowerClick(TestTower tower)
    {
		_uITower.ClickTower(tower);
    }

	public void StageUIInit(TestWave[] waves)
    {
		_uiWave.StageEnemyUIInit(waves);
    }

	public void WaveUISetting(int wave)
    {
		_uiWave.NextWave(wave);
    }

	public void AppearanceEnemy(EEnemy enemyType)
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
}
