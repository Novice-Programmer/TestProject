using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    public static TestGameManager Instance { set; get; }

    int _wave = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        TestTowerDataManager.Instance.GameTowerSetting(TestPlayerDataManager.Instance.GetPlayerSelectTower());
        TestResourceManager.Instance.GameResourceSetting(TestPlayerDataManager.Instance.GetResourceResearchData());
        TestGameUI.Instance.GameUISetting();
        TestResourceManager.Instance.TowerPartPayment(EPaymentType.Initial, _wave);
    }

    public void WaveStart()
    {
        TestWaveManager.Instance.WaveStart(_wave);
        _wave++;
    }

    public void WaveEnd()
    {

    }

    public void TowerInstall(ETowerType towerType,int installCost)
    {
        TestInputManager.Instance.InstallTower(towerType,installCost);
    }

    public void TowerBulid(TestGhostTower ghostTowerData)
    {
        TestResourceManager.Instance.TowerPartValue = -ghostTowerData.installCost;
        TestTower tower = Instantiate(TestTowerDataManager.Instance.GetTower(ghostTowerData._towerType), ghostTowerData.fitPos, Quaternion.identity);
    }
}
