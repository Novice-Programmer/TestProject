using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct SelectTowerData
{
    public ETowerType towerType;
    public TestResearchData[] researchDatas;

    public SelectTowerData(ETowerType towerType,TestResearchData[] researchDatas)
    {
        this.towerType = towerType;
        this.researchDatas = researchDatas;
    }
}
