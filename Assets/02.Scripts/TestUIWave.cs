using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIWave : MonoBehaviour
{
	[SerializeField] Button _waveStartBtn = null;
	public void ClickWaveStart()
	{
		_waveStartBtn.gameObject.SetActive(false);
		TestGameManager.Instance.WaveStart();
	}
}
