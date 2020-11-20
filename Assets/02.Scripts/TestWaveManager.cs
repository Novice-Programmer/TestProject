using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestWaveManager : MonoBehaviour
{
    public static TestWaveManager Instance { set; get; }
    [SerializeField] TestWave[] _waves = null;
    public Transform _startPoint = null;
 
    int _waveNumber = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TestPool.Instance.WaveEnemyInit(_waves);
    }

    public void WaveStart(int waveNumber)
    {
        _waveNumber = waveNumber;
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        TestWave nowWave = _waves[_waveNumber];
        yield return new WaitForSeconds(nowWave._waveDelay);
        for (int i = 0; i < nowWave._spawnDatas.Length; i++)
        {
            yield return new WaitForSeconds(nowWave._spawnDatas[i]._delayTime);
            TestEnemy enemy = TestPool.Instance.GetEnemy(nowWave._spawnDatas[i]._enemyData.fileName);
            enemy.transform.position = _startPoint.position;
            enemy.gameObject.SetActive(true);
        }
        yield return null;
    }
}
