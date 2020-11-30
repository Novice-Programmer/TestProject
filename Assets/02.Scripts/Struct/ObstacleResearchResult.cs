using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObstacleResearchResult
{
    public float valueIncreaseRate; // 특수 능력치 증가량
    public float costReduceRate; // 비용 감소량
    public ESpecialResearch specialResearch;
}
