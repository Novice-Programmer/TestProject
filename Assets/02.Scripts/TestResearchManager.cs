using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResearchManager : MonoBehaviour
{
    public static TestResearchManager Instance { set; get; }
    [SerializeField] TestResearchData[] _researchAllDatas = null;
    Dictionary<EResearch, TestResearchData> _researchDataDic = new Dictionary<EResearch, TestResearchData>();
    Dictionary<EResearchType, TestResearchData[]> _researchTypeDic = new Dictionary<EResearchType, TestResearchData[]>();

    private void Awake()
    {
        Instance = this;
        ResearchDictionarySetting();
    }

    void ResearchDictionarySetting()
    {
        List<TestResearchData> towerResearchDatas = new List<TestResearchData>();
        List<TestResearchData> obstacleResearchDatas = new List<TestResearchData>();
        List<TestResearchData> resourceResearchDatas = new List<TestResearchData>();
        for (int i = 0; i < _researchAllDatas.Length; i++)
        {
            _researchDataDic.Add(_researchAllDatas[i].research, _researchAllDatas[i]);
            switch (_researchAllDatas[i].type)
            {
                case EResearchType.Tower:
                    towerResearchDatas.Add(_researchAllDatas[i]);
                    break;
                case EResearchType.Obstacle:
                    obstacleResearchDatas.Add(_researchAllDatas[i]);
                    break;
                case EResearchType.Resource:
                    resourceResearchDatas.Add(_researchAllDatas[i]);
                    break;
                case EResearchType.TowerObstacle:
                    towerResearchDatas.Add(_researchAllDatas[i]);
                    obstacleResearchDatas.Add(_researchAllDatas[i]);
                    break;
                case EResearchType.ALL:
                    towerResearchDatas.Add(_researchAllDatas[i]);
                    obstacleResearchDatas.Add(_researchAllDatas[i]);
                    resourceResearchDatas.Add(_researchAllDatas[i]);
                    break;
            }
        }
        _researchTypeDic.Add(EResearchType.Tower, towerResearchDatas.ToArray());
        _researchTypeDic.Add(EResearchType.Obstacle, obstacleResearchDatas.ToArray());
        _researchTypeDic.Add(EResearchType.Resource, resourceResearchDatas.ToArray());
    }

    public TestResearchData[] GetResearchTypeData(EResearchType researchType)
    {
        return _researchTypeDic[researchType];
    }

    public TestResearchData GetResearchData(EResearch research)
    {
        return _researchDataDic[research];
    }
}
