using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIWave : MonoBehaviour
{
	[SerializeField] Button _waveStartBtn = null;
	[SerializeField] TestWaveEnemyUI _prefabWaveEnemy = null;
	[SerializeField] Transform _waveEnemyContainer = null;
    [SerializeField] Text _waveNumberTxt = null;
	[SerializeField] Text _waveEnemyNumberTxt = null;
    [SerializeField] Sprite[] _enemyIconSprites = null;
    [SerializeField] Sprite[] _enemyRankSprites = null;

	int _enemyNumber;

    Dictionary<int, List<EEnemyType>> _waveEnemyList = new Dictionary<int, List<EEnemyType>>();
    Dictionary<EEnemyType, List<TestWaveEnemyUI>> _enemyTypeUIDic = new Dictionary<EEnemyType, List<TestWaveEnemyUI>>();

	public void ClickWaveStart()
	{
		_waveStartBtn.gameObject.SetActive(false);
		TestGameManager.Instance.WaveStart();
        TestInputManager.Instance.UITouch();
    }

    public void NextWave(int wave)
    {
        _waveNumberTxt.text = "Wave" + (wave+1).ToString();
        _waveStartBtn.gameObject.SetActive(true);
        WaveEnemyUISetting(wave);
    }

	public void StageEnemyUIInit(TestWave[] waves)
    {
        Dictionary<EEnemyType, int> enemy = new Dictionary<EEnemyType, int>();
        for (int i = 0; i < waves.Length; i++)
        {
            TestWave wave = waves[i];
            List<EEnemyType> enemies = new List<EEnemyType>();
            Dictionary<EEnemyType, int> enemyCheck = new Dictionary<EEnemyType, int>();
            for (int j = 0; j < wave._spawnDatas.Length; j++)
            {
                EEnemyType enemyType = wave._spawnDatas[j]._enemyData.enemyName;
                enemies.Add(enemyType);
                if (enemyCheck.ContainsKey(enemyType))
                {
                    enemyCheck[enemyType]++;
                }
                else
                {
                    enemyCheck.Add(enemyType, 1);
                }
            }
            foreach (EEnemyType enemyType in enemyCheck.Keys)
            {
                if (enemy.ContainsKey(enemyType))
                {
                    if (enemy[enemyType] < enemyCheck[enemyType])
                    {
                        enemy[enemyType] = enemyCheck[enemyType];
                    }
                }
                else
                {
                    enemy.Add(enemyType, enemyCheck[enemyType]);
                }
            }

            _waveEnemyList.Add(i, enemies);
        }

        int enemyNumber = 0;


        foreach (EEnemyType enemyType in enemy.Keys)
        {
            enemyNumber += enemy[enemyType];
        }

        foreach (EEnemyType enemyType in enemy.Keys)
        {
            List<TestWaveEnemyUI> waveEnemyUIList = new List<TestWaveEnemyUI>();
            for(int i = 0; i < enemy[enemyType]; i++)
            {
                TestWaveEnemyUI waveEnemyUI = Instantiate(_prefabWaveEnemy, _waveEnemyContainer);
                waveEnemyUI.WaveEnemyInfo(_enemyIconSprites[(int)enemyType], _enemyRankSprites[(int)enemyType]);
                waveEnemyUIList.Add(waveEnemyUI);
                waveEnemyUI.gameObject.SetActive(false);
            }
            _enemyTypeUIDic.Add(enemyType,waveEnemyUIList);
        }
        WaveEnemyUISetting(0);
    }

	void WaveEnemyUISetting(int wave)
    {
        _enemyNumber = _waveEnemyList[wave].Count;
		_waveEnemyNumberTxt.text = _enemyNumber.ToString();
        for(int i = 0; i < _waveEnemyList[wave].Count; i++)
        {
            WaveEnemyView(_waveEnemyList[wave][i]);
        }
    }

    void WaveEnemyView(EEnemyType enemyType)
    {
        List<TestWaveEnemyUI> typeEnemy = _enemyTypeUIDic[enemyType];
        for(int i = 0; i < typeEnemy.Count; i++)
        {
            if (!typeEnemy[i].gameObject.activeSelf)
            {
                typeEnemy[i].gameObject.SetActive(true);
                break;
            }
        }
    }

    public void WaveEnemyAppearance(EEnemyType enemyType)
    {
        List<TestWaveEnemyUI> typeEnemy = _enemyTypeUIDic[enemyType];
        for(int i = 0; i < typeEnemy.Count; i++)
        {
            if (typeEnemy[i].gameObject.activeSelf)
            {
                typeEnemy[i].gameObject.SetActive(false);
                _enemyNumber--;
                _waveEnemyNumberTxt.text = _enemyNumber.ToString();
                break;
            }
        }
    }
}
