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

	[SerializeField] TestUITWIN _uITWIN = null;
	[SerializeField] TestUIWave _uiWave = null;
	[SerializeField] TestUIResource _uiResource = null;

    private void Awake()
    {
		Instance = this;
    }

	public void GameUISetting()
	{
		_uITWIN.TowerInstallButtonAdd();
	}

    public void TowerInstallClick()
    {
        _uITWIN.TowerViewOff();
    }

	public void ResourceValueChange()
    {
		_uITWIN.UIValueChange();
		_uiResource.UIValueChange();
    }
}
