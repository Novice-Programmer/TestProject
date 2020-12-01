using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TestObstacleGameData
{
    public EObstacleType obstacleType;
    public EAttackAble attackAble;
    public EObjectName objectName;
    public string obstacleNameString;
    public string description;
    public int durability;
    public int reduceValue;
    public int buildCost;
    public float[] values;
    public ObstacleResearchResult researchResult;
    public List<EResearch> researchs;
    public ESpecialResearch specialResearch; // 특수 업그레이드

    public TestObstacleGameData(TestObstacleData obstacleData)
    {
        obstacleType = obstacleData.obstacleType;
        attackAble = obstacleData.attackAble;
        obstacleNameString = obstacleData.obstacleName;
        objectName = obstacleData.objectName;
        description = obstacleData.description;
        durability = obstacleData.durability;
        reduceValue = obstacleData.reduceValue;
        buildCost = obstacleData.buildCost;
        values = new float[obstacleData.values.Length];
        for(int i = 0; i < values.Length; i++)
        {
            values[i] = obstacleData.values[i];
        }
        researchs = new List<EResearch>();
        specialResearch = ESpecialResearch.None;
        researchResult = new ObstacleResearchResult();
    }

    public void ResearchAdd(params TestResearchData[] obstacleResearchs)
    {
        for (int i = 0; i < obstacleResearchs.Length; i++)
        {
            switch (obstacleResearchs[i].research)
            {
                case EResearch.ReorganizatedOfDesign:
                    researchResult.costReduceRate += -10;
                    researchResult.valueIncreaseRate += 10;
                    break;
            }
            researchs.Add(obstacleResearchs[i].research);
        }
        
        int reduceCost = (int)(buildCost * researchResult.costReduceRate * 0.01f);
        if (buildCost + reduceCost > 0)
        {
            buildCost += reduceCost;
        }
    }
}