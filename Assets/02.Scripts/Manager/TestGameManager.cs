using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public static TestGameManager Instance { set; get; }

    int _wave = 0;
    float _timeCheck = 0;
    bool _waveStart = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ObjectDataManager.Instance.GameInstallSetting(PlayerDataManager.Instance.GetPlayerSelectTower(), PlayerDataManager.Instance.GetPlayerSelectObstacle());
        TestResourceManager.Instance.GameResourceSetting(PlayerDataManager.Instance.GetResourceResearchData());
        TestGameUI.Instance.GameUISetting();
        TestResourceManager.Instance.TowerPartPayment(EPaymentType.Initial, _wave);
    }

    private void Update()
    {
        if (_waveStart)
        {
            _timeCheck += Time.deltaTime;
            if (_timeCheck >= 10.0f)
            {
                _timeCheck = 0;
                TestResourceManager.Instance.TowerPartPayment(EPaymentType.Occasional, _wave - 1);
            }
        }
    }

    public void WaveStart()
    {
        _waveStart = true;
        TestWaveManager.Instance.WaveStart(_wave);
    }

    public void WaveEnd(bool stageClear)
    {
        _timeCheck = 0;
        _waveStart = false;
        _wave++;
        TestResourceManager.Instance.WaveClear(_wave);
        if (stageClear)
        {
            Debug.Log("스테이지 클리어");
            TestGameUI.Instance.StageClear();
        }
        else
        {
            TestResourceManager.Instance.TowerPartPayment(EPaymentType.Regular, _wave);
            TestGameUI.Instance.WaveClear(_wave);
        }
    }

    public void Install(EObjectType objectType, EObjectName objectName, int installCost)
    {
        TestInputManager.Instance.Install(objectType, objectName, installCost);
    }

    public void TowerBuild(TestGhost ghostData)
    {
        TestResourceManager.Instance.TowerPartValue = -ghostData._installCost;
        TestTower tower = Instantiate(ObjectDataManager.Instance.GetTower(ghostData._objectName), ghostData._fitPos, Quaternion.identity);
        tower.BuildingTower(ghostData);
    }

    public void ObstacleBuild(TestGhost ghostData)
    {
        TestResourceManager.Instance.TowerPartValue = -ghostData._installCost;
        TestObstacle obstacle = Instantiate(ObjectDataManager.Instance.GetObstacle(ghostData._objectName), ghostData._fitPos, Quaternion.identity);
        obstacle.BuildingObstacle(ghostData);
    }

    public void GameOver()
    {
        Debug.Log("GaveOver");
    }
}
