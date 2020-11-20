using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TestSelectTowerData
{
    public ETowerType towerType;
    public TestResearchData[] researchDatas;

    public TestSelectTowerData(ETowerType towerType,TestResearchData[] researchDatas)
    {
        this.towerType = towerType;
        this.researchDatas = researchDatas;
    }
}
