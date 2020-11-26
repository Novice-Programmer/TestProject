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

    Dictionary<int, List<EEnemy>> _waveEnemyList = new Dictionary<int, List<EEnemy>>();
    Dictionary<EEnemy, List<TestWaveEnemyUI>> _enemyTypeUIDic = new Dictionary<EEnemy, List<TestWaveEnemyUI>>();

	public void ClickWaveStart()
	{
		_waveStartBtn.gameObject.SetActive(false);
		TestGameManager.Instance.WaveStart();
	}

    public void NextWave(int wave)
    {
        _waveNumberTxt.text = "Wave" + (wave+1).ToString();
        _waveStartBtn.gameObject.SetActive(true);
        WaveEnemyUISetting(wave);
    }

	public void StageEnemyUIInit(TestWave[] waves)
    {
        Dictionary<EEnemy, int> enemy = new Dictionary<EEnemy, int>();
        for (int i = 0; i < waves.Length; i++)
        {
            TestWave wave = waves[i];
            List<EEnemy> enemies = new List<EEnemy>();
            Dictionary<EEnemy, int> enemyCheck = new Dictionary<EEnemy, int>();
            for (int j = 0; j < wave._spawnDatas.Length; j++)
            {
                EEnemy enemyType = wave._spawnDatas[j]._enemyData.enemy;
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
            foreach (EEnemy enemyType in enemyCheck.Keys)
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


        foreach (EEnemy enemyType in enemy.Keys)
        {
            enemyNumber += enemy[enemyType];
        }

        foreach (EEnemy enemyType in enemy.Keys)
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

    void WaveEnemyView(EEnemy enemyType)
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

    public void WaveEnemyAppearance(EEnemy enemyType)
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
