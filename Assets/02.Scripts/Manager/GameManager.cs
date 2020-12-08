using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { set; get; }
    int _wave = 0;
    float _timeCheck = 0;
    bool _waveStart = false;
    bool _gameEnd = false;

    List<GameObject> _installObjects = new List<GameObject>();

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 플레이어가 선택한 오브젝트 가져오기
        GameUI.Instance.GameUISetting();
        ResourceManager.Instance.TowerPartPayment(EPaymentType.Initial, _wave);
    }

    private void Update()
    {
        if (_gameEnd)
            return;

        if (_waveStart)
        {
            _timeCheck += Time.deltaTime;
            if (_timeCheck >= 10.0f)
            {
                _timeCheck = 0;
                ResourceManager.Instance.TowerPartPayment(EPaymentType.Occasional, _wave - 1);
            }
        }
    }

    public void WaveStart()
    {
        _waveStart = true;
        WaveManager.Instance.WaveStart(_wave);
    }

    public void WaveEnd(bool stageClear)
    {
        _timeCheck = 0;
        _waveStart = false;
        _wave++;
        ResourceManager.Instance.WaveClear(_wave);
        if (stageClear)
        {
            Debug.Log("스테이지 클리어");
            GameUI.Instance.StageClear();
        }
        else
        {
            ResourceManager.Instance.TowerPartPayment(EPaymentType.Regular, _wave);
            GameUI.Instance.WaveClear(_wave);
        }
    }

    public void Install(EObjectType objectType, EObjectName objectName, int installCost)
    {
        InputManager.Instance.Install(objectType, objectName, installCost);
    }

    public void TowerBuild(Ghost ghostData)
    {
        ResourceManager.Instance.TowerPartValue = -ghostData._installCost;
        Tower tower = Instantiate(ObjectDataManager.Instance.GetTower(ghostData._objectName), ghostData._fitPos, Quaternion.identity);
        tower.BuildingTower(ghostData);
        _installObjects.Add(tower.gameObject);
    }

    public void ObstacleBuild(Ghost ghostData)
    {
        ResourceManager.Instance.TowerPartValue = -ghostData._installCost;
        Obstacle obstacle = Instantiate(ObjectDataManager.Instance.GetObstacle(ghostData._objectName), ghostData._fitPos, Quaternion.identity);
        obstacle.BuildingObstacle(ghostData);
        _installObjects.Add(obstacle.gameObject);
    }

    public void GameEnd()
    {
        _gameEnd = true;
        PoolManager.Instance.GameEnd();
        for(int i = 0; i < _installObjects.Count; i++)
        {
            if (_installObjects[i] != null)
            {
                _installObjects[i].SetActive(false);
            }
        }
    }
}
