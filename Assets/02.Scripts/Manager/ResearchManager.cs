using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : MonoBehaviour
{
    public static ResearchManager Instance { set; get; }
    [SerializeField] ResearchData[] _researchAllDatas = null;
    Dictionary<EResearch, ResearchData> _researchDataDic = new Dictionary<EResearch, ResearchData>();
    Dictionary<EResearchType, ResearchData[]> _researchTypeDic = new Dictionary<EResearchType, ResearchData[]>();

    private void Awake()
    {
        Instance = this;
        ResearchDictionarySetting();
    }

    void ResearchDictionarySetting()
    {
        List<ResearchData> towerResearchDatas = new List<ResearchData>();
        List<ResearchData> obstacleResearchDatas = new List<ResearchData>();
        List<ResearchData> resourceResearchDatas = new List<ResearchData>();
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

    public ResearchData[] GetResearchTypeData(EResearchType researchType)
    {
        return _researchTypeDic[researchType];
    }

    public ResearchData GetResearchData(EResearch research)
    {
        return _researchDataDic[research];
    }
}
