using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerDataManager : MonoBehaviour
{
    public static TestPlayerDataManager Instance { set; get; }

    [SerializeField] List<EResearch> _playerAllResearch = new List<EResearch>();
    [SerializeField] List<ETowerType> _playerSelectTower = new List<ETowerType>();

    public int Stage = 0;

    private void Awake()
    {
        Instance = this;
    }

    public TestSelectTowerData[] GetPlayerSelectTower()
    {
        List<TestSelectTowerData> selectTowerDatas = new List<TestSelectTowerData>();

        for (int i = 0; i < _playerSelectTower.Count; i++)
        {
            ETowerType towerType = _playerSelectTower[i];
            List<TestResearchData> researchDatas = new List<TestResearchData>();
            EResearchTarget target = EResearchTarget.None;
            switch (towerType)
            {
                case ETowerType.KW9A:
                    target = EResearchTarget.KW9A;
                    break;
                case ETowerType.P013:
                    target = EResearchTarget.P013;
                    break;
            }
            for (int j = 0; j < _playerAllResearch.Count; j++)
            {
                TestResearchData researchData = TestResearchManager.Instance.GetResearchData(_playerAllResearch[j]);
                for (int k = 0; k < researchData.target.Length; k++)
                {
                    if (researchData.target[k] == target)
                    {
                        researchDatas.Add(researchData);
                    }
                }
                if (researchData.target[0] == EResearchTarget.Tower)
                {
                    researchDatas.Add(researchData);
                }
            }
            selectTowerDatas.Add(new TestSelectTowerData(towerType, researchDatas.ToArray()));
        }

        return selectTowerDatas.ToArray();
    }

    public TestResearchData[] GetResourceResearchData()
    {
        List<TestResearchData> researchDatas = new List<TestResearchData>();
        for(int i = 0; i < _playerAllResearch.Count; i++)
        {
            TestResearchData researchData = TestResearchManager.Instance.GetResearchData(_playerAllResearch[i]);
            if(researchData.type == EResearchType.Resource)
            {
                researchDatas.Add(researchData);
            }
        }
        return researchDatas.ToArray();
    }
}
