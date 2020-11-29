using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWaveManager : MonoBehaviour
{
    public static TestWaveManager Instance { set; get; }
    [SerializeField] TestWave[] _waves = null;
    public Transform _startPoint = null;
 
    int _waveNumber = 0;
    int _nowWaveEnemyNumber = 0;
    bool _stageClearCheck = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TestPool.Instance.WaveEnemyInit(_waves);
        TestGameUI.Instance.StageUIInit(_waves);
    }

    public void WaveStart(int waveNumber)
    {
        _waveNumber = waveNumber;
        _nowWaveEnemyNumber = _waves[waveNumber]._spawnDatas.Length;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        TestWave nowWave = _waves[_waveNumber];
        yield return new WaitForSeconds(nowWave._waveDelay);
        for (int i = 0; i < nowWave._spawnDatas.Length; i++)
        {
            yield return new WaitForSeconds(nowWave._spawnDatas[i]._delayTime);
            GameObject enemy = TestPool.Instance.PoolGetObject(nowWave._spawnDatas[i]._enemyData.fileName);
            enemy.transform.position = _startPoint.position;
            enemy.GetComponent<TestEnemy>().Active();
            TestGameUI.Instance.AppearanceEnemy(nowWave._spawnDatas[i]._enemyData.enemy);
        }
        yield return null;

    }

    public void WaveEnemyDie()
    {
        _nowWaveEnemyNumber--;
        if (_nowWaveEnemyNumber == 0)
        {
            WaveClear();
        }
    }

    void WaveClear()
    {
        _stageClearCheck = _waveNumber >= _waves.Length - 1;
        TestGameManager.Instance.WaveEnd(_stageClearCheck);
    }
}
