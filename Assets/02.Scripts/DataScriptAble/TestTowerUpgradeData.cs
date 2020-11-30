using UnityEngine;


public enum EUpgradeType
{
    Attack,
    Defence,
    Special
}

[CreateAssetMenu(fileName = "TestData", menuName = "Test/TowerTowerUpgradeData", order = 3)]
public class TestTowerUpgradeData : ScriptableObject
{
    public ETowerType towerType; // 타워 종류
    public EUpgradeType upgradeType; // 업그레이드 종류
    public int level; // 업그레이드 레벨
    public float[] addValue; // 증가 능력치

    public int nextCost; // 다음 비용
}