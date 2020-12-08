using System;
using System.Collections.Generic;

[Serializable]
public struct TowerGameData
{
    public EObjectName objectName;
    public string towerName;
    public string description;
    public int hp; // 체력
    public int ep; // 전력
    public int atk; // 공격력
    public int def; // 방어력
    public float atkSpd; // 공격 속도
    public float atkRange; // 공격 범위
    public int atkNumber; // 공격 횟수
    public int targetNumber; // 타겟 수
    public float[] spValue; // 특수 능력치

    public int buildCost; // 설치 비용
    public int maxUpgrade; // 일반 최대 업그레이드 레벨
    public int defUpgradeCost; // 방어 업그레이드 비용
    public int atkUpgradeCost; // 공격 업그레이드 비용
    public int spUpgradeCost; // 특수 업그레이드 비용
    public int spMaxUpgrade; // 특수 최대 업그레이드
    public ESpecialResearch specialResearch; // 특수 업그레이드

    public TowerGameData(TowerData towerData)
    {
        objectName = towerData.objectName;
        towerName = towerData.towerName;
        description = towerData.description;
        hp = towerData.hp;
        ep = towerData.ep;
        atk = towerData.atk;
        def = towerData.def;
        atkSpd = towerData.atkSpd;
        atkRange = towerData.atkRange;
        atkNumber = towerData.atkNumber;
        targetNumber = towerData.targetNumber;
        spValue = new float[towerData.spValue.Length];
        for (int i = 0; i < spValue.Length; i++)
        {
            spValue[i] = towerData.spValue[i];
        }
        buildCost = towerData.buildCost;
        maxUpgrade = towerData.maxUpgrade;
        spMaxUpgrade = towerData.maxSpUpgrade;
        defUpgradeCost = towerData.defUpgradeCost;
        atkUpgradeCost = towerData.atkUpgradeCost;
        spUpgradeCost = towerData.spUpgradeCost;
        specialResearch = ESpecialResearch.None;
    }

    public void CostCheck()
    {
        int reduceCost = (int)(buildCost * ResearchManager.Instance.GameResearchData.towerCostReduceRate * 0.01f);
        if (buildCost + reduceCost > 0)
        {
            buildCost += reduceCost;
        }
        reduceCost = (int)(defUpgradeCost * ResearchManager.Instance.GameResearchData.towerCostReduceRate * 0.01f);
        if (defUpgradeCost + reduceCost > 0)
        {
            defUpgradeCost += reduceCost;
        }
        reduceCost = (int)(atkUpgradeCost * ResearchManager.Instance.GameResearchData.towerCostReduceRate * 0.01f);
        if (atkUpgradeCost + reduceCost > 0)
        {
            atkUpgradeCost += reduceCost;
        }
        reduceCost = (int)(spUpgradeCost * ResearchManager.Instance.GameResearchData.towerCostReduceRate * 0.01f);
        if (spUpgradeCost + reduceCost > 0)
        {
            spUpgradeCost += reduceCost;
        }
    }
}
