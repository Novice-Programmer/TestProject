using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPassType
{
    Enemy,
}

public class TestPoolManager : MonoBehaviour
{
    static TestPoolManager _uniqueInstance;
    public static TestPoolManager Instance { get { return _uniqueInstance; } }

    Dictionary<string, List<GameObject>> _poolObjects = new Dictionary<string, List<GameObject>>();

    private void Awake()
    {
        _uniqueInstance = this;
    }

    /// <summary>
    /// 스테이지의 웨이브 정보를 받아와서 적의 이름 수의 최대만큼 게임 오브젝트를 미리 만듬
    /// </summary>
    /// <param name="waves">스테이지의 웨이브 정보</param>
    public void WaveEnemyInit(TestWave[] waves)
    {
        Dictionary<string, int> enemyCreateName = new Dictionary<string, int>();
        for (int i = 0; i < waves.Length; i++)
        {
            TestWave wave = waves[i];
            Dictionary<string, int> enemyNameCheck = new Dictionary<string, int>();
            for (int j = 0; j < wave._spawnDatas.Length; j++)
            {
                string enemyFileName = wave._spawnDatas[j]._enemyData.fileName;
                if (enemyNameCheck.ContainsKey(enemyFileName))
                {
                    enemyNameCheck[enemyFileName]++;
                }
                else
                {
                    enemyNameCheck.Add(enemyFileName, 1);
                }
            }
            foreach(string enemyFileName in enemyNameCheck.Keys)
            {
                if (enemyCreateName.ContainsKey(enemyFileName))
                {
                    if (enemyCreateName[enemyFileName] < enemyNameCheck[enemyFileName])
                    {
                        enemyCreateName[enemyFileName] = enemyNameCheck[enemyFileName];
                    }
                }
                else
                {
                    enemyCreateName.Add(enemyFileName, enemyNameCheck[enemyFileName]);
                }
            }
        }
        foreach (string enemyFileName in enemyCreateName.Keys)
        {
            CreatePoolObject(enemyFileName, enemyCreateName[enemyFileName], EPassType.Enemy);
        }
    }

    /// <summary>
    /// Resources 폴더 안에 path에 있는 objectName을 받아와서 number만큼 오브젝트를 만든 뒤에 _poolObjects에 저장
    /// </summary>
    /// <param name="objectName">파일명</param>
    /// <param name="number">만들 오브젝트의 숫자</param>
    /// <param name="path">위치</param>
    void CreatePoolObject(string objectName, int number, EPassType path)
    {
        List<GameObject> gameObjectList = new List<GameObject>();
        GameObject go = Resources.Load(path.ToString() + "/" + objectName) as GameObject;
        for (int i = 0; i < number; i++)
        {
            GameObject obj = Instantiate(go, TestWaveManager.Instance._startPoint.position, Quaternion.identity, transform);
            obj.SetActive(false);
            gameObjectList.Add(obj);
        }
        _poolObjects.Add(objectName, gameObjectList);
    }

    /// <summary>
    /// _poolObjects에 있는 objectName에 일치하는 이름 중에 비활성화 된 게임 오브젝트를 갖고온다.
    /// </summary>
    /// <param name="objectName">파일명</param>
    /// <returns></returns>
    public GameObject PoolGetObject(string objectName)
    {
        GameObject go = _poolObjects[objectName][0];
        for(int i = 0; i < _poolObjects[objectName].Count; i++)
        {
            if (!_poolObjects[objectName][i].activeSelf)
            {
                go = _poolObjects[objectName][i];
                break;
            }
        }
        return go;
    }
}
