using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchManager : TSingleton<ResearchManager>
{
    [SerializeField] ResearchData[] _researchAllDatas = null;
    Dictionary<EResearchType, Dictionary<int, List<ResearchData>>> _researchTypeDic = new Dictionary<EResearchType, Dictionary<int, List<ResearchData>>>();
    ResearchResult _researchResult;

    public ResearchResult GameResearchData { get { return _researchResult; } }
    public Dictionary<EResearchType, Dictionary<int, List<ResearchData>>> ResearchDic { get { return _researchTypeDic; } }

    private void Awake()
    {
        Init();
        Instance = this;
        ResearchDictionarySetting();
    }

    void ResearchDictionarySetting()
    {
        for (int i = 0; i < _researchAllDatas.Length; i++)
        {
            Dictionary<int, List<ResearchData>> stepResearch;
            if (_researchTypeDic.ContainsKey(_researchAllDatas[i].type))
            {
                stepResearch = _researchTypeDic[_researchAllDatas[i].type];
            }
            else
            {
                stepResearch = new Dictionary<int, List<ResearchData>>();
                _researchTypeDic.Add(_researchAllDatas[i].type, stepResearch);
            }

            List<ResearchData> researchDatas;
            if (stepResearch.ContainsKey(_researchAllDatas[i].step))
            {
                researchDatas = _researchTypeDic[_researchAllDatas[i].type][_researchAllDatas[i].step];
            }
            else
            {
                researchDatas = new List<ResearchData>();
                _researchTypeDic[_researchAllDatas[i].type].Add(_researchAllDatas[i].step, researchDatas);
            }
            researchDatas.Add(_researchAllDatas[i]);
        }
    }

    public Dictionary<int, List<ResearchData>> GetStepResearch(EResearchType researchType)
    {
        return _researchTypeDic[researchType];
    }

    public ResearchData[] GetStepResearch(EResearchType researchType, int step)
    {
        return _researchTypeDic[researchType][step].ToArray();
    }

    public ResearchData GetResearchData(EResearch research)
    {
        for (int i = 0; i < _researchAllDatas.Length; i++)
        {
            if (_researchAllDatas[i].research == research)
                return _researchAllDatas[i];
        }
        return null;
    }

    public void ResearchUpdate(List<EResearch> selectResearch)
    {
        _researchResult = new ResearchResult();
        _researchResult = ResearchValueInit(selectResearch.ToArray());
    }

    ResearchResult ResearchValueInit(params EResearch[] researches)
    {
        ResearchResult researchResult = new ResearchResult();
        for (int i = 0; i < researches.Length; i++)
        {
            switch (researches[i])
            {
                case EResearch.TowerLv0:
                    researchResult.epAddRate += 5;
                    researchResult.towerCostReduceRate += -5;
                    break;
                case EResearch.TowerLv1_1:
                    researchResult.startATKUpgrade++;
                    researchResult.startDEFUpgrade++;
                    researchResult.startSPUpgrade++;
                    researchResult.towerCostReduceRate += 30;
                    break;
                case EResearch.TowerLv1_2:
                    researchResult.defAddRate += 10;
                    researchResult.hpAddRate += 10;
                    researchResult.epAddRate += 10;
                    researchResult.towerCostReduceRate += -15;
                    break;
                case EResearch.TowerLv1_3:
                    researchResult.maxATKUpgradeAdd++;
                    researchResult.maxDEFUpgradeAdd++;
                    researchResult.maxSPUpgradeAdd++;
                    researchResult.towerCostReduceRate += 10;
                    break;
                case EResearch.TowerLv2_1:
                    researchResult.atkAddRate += 20;
                    researchResult.atkRangeAddRate += 20;
                    researchResult.atkSpdAddRate += -10;
                    break;
                case EResearch.TowerLv2_2:
                    researchResult.epAddRate += 30;
                    researchResult.atkSpdAddRate += 10;
                    researchResult.spAddRate += 10;
                    researchResult.hpAddRate += -15;
                    researchResult.defAddRate += -15;
                    break;
                case EResearch.TowerLv2_3:
                    researchResult.atkAddRate += 5;
                    researchResult.atkRangeAddRate += 5;
                    researchResult.atkSpdAddRate += 5;
                    researchResult.hpAddRate += 5;
                    researchResult.epAddRate += 5;
                    researchResult.defAddRate += 5;
                    break;
                case EResearch.TowerLv3_1:
                    researchResult.atkAddRate += 20;
                    researchResult.atkRangeAddRate += 20;
                    researchResult.atkSpdAddRate += 10;
                    break;
                case EResearch.TowerLv3_2:
                    researchResult.epAddRate += 20;
                    researchResult.defAddRate += 20;
                    researchResult.hpAddRate += 20;
                    researchResult.spAddRate += 20;
                    break;

                case EResearch.ObstacleLv0:
                    researchResult.valueIncreaseRate += 5;
                    researchResult.obstacleCostReduceRate += -5;
                    break;
                case EResearch.ObstacleLv1_1:
                    researchResult.valueIncreaseRate += 5;
                    break;
                case EResearch.ObstacleLv1_2:
                    researchResult.obstacleCostReduceRate += -5;
                    break;
                case EResearch.ObstacleLv2_1:
                    researchResult.valueIncreaseRate += 5;
                    researchResult.obstacleCostReduceRate += -20;
                    break;
                case EResearch.ObstacleLv2_2:
                    researchResult.valueIncreaseRate += 15;
                    researchResult.obstacleCostReduceRate += -10;
                    break;
                case EResearch.ObstacleLv3_1:
                    researchResult.valueIncreaseRate += 30;
                    researchResult.obstacleCostReduceRate += 50;
                    break;
                case EResearch.ObstacleLv3_2:
                    researchResult.valueIncreaseRate += -20;
                    researchResult.obstacleCostReduceRate += -50;
                    break;


                case EResearch.ResourceLv0:
                    researchResult.towerPartAddRate += 5;
                    researchResult.mineralAddRate += 5;
                    break;
                case EResearch.ResourceLv1_1:
                    researchResult.towerPartAddRate += 20;
                    break;
                case EResearch.ResourceLv1_2:
                    researchResult.mineralAddRate += 10;
                    break;
                case EResearch.ResourceLv2_1:
                    researchResult.occasionalAddRate += 30;
                    researchResult.occasionalReduceTime += 30;
                    break;
                case EResearch.ResourceLv2_2:
                    researchResult.occasionalAddRate += 5;
                    researchResult.occasionalReduceTime += -30;
                    break;
                case EResearch.ResourceLv3_1:
                    researchResult.towerPartAddRate += 30;
                    researchResult.occasionalAddRate += -30;
                    researchResult.occasionalReduceTime += 15;
                    break;
                case EResearch.ResourceLv3_2:
                    researchResult.towerPartAddRate += -10;
                    researchResult.occasionalAddRate += 25;
                    researchResult.occasionalReduceTime += -20;
                    break;
            }
        }

        return researchResult;
    }

    public string GetAllResearchString(EResearchType researchType)
    {
        return ResearchString(researchType, _researchResult);
    }

    string ResearchString(EResearchType researchType, ResearchResult researchResult)
    {
        string resultTxt = "";
        switch (researchType)
        {
            case EResearchType.Tower:
                if (researchResult.hpAddRate != 0)
                    resultTxt += "체력 " + researchResult.hpAddRate + "% 증가\n";
                if (researchResult.epAddRate != 0)
                    resultTxt += "전력 " + researchResult.epAddRate + "% 증가\n";
                if (researchResult.atkAddRate != 0)
                    resultTxt += "공격력 " + researchResult.atkAddRate + "% 증가\n";
                if (researchResult.atkSpdAddRate != 0)
                    resultTxt += "공격속도 " + researchResult.atkSpdAddRate + "% 증가\n";
                if (researchResult.atkRangeAddRate != 0)
                    resultTxt += "공격범위 " + researchResult.atkRangeAddRate + "% 증가\n";
                if (researchResult.defAddRate != 0)
                    resultTxt += "방어력 " + researchResult.defAddRate + "% 증가\n";
                if (researchResult.spAddRate != 0)
                    resultTxt += "특수공격 " + researchResult.spAddRate + "% 증가\n";
                if (researchResult.towerCostReduceRate != 0)
                    resultTxt += "타워비용 " + researchResult.towerCostReduceRate + "% 증가\n";
                if (researchResult.startATKUpgrade != 0)
                    resultTxt += "시작 공격 업그레이드 " + researchResult.startATKUpgrade + " 증가\n";
                if (researchResult.startDEFUpgrade != 0)
                    resultTxt += "시작 방어 업그레이드 " + researchResult.startDEFUpgrade + " 증가\n";
                if (researchResult.startSPUpgrade != 0)
                    resultTxt += "시작 특수 업그레이드 " + researchResult.startSPUpgrade + " 증가\n";
                if (researchResult.maxATKUpgradeAdd != 0)
                    resultTxt += "최대 공격 업그레이드 " + researchResult.maxATKUpgradeAdd + " 증가\n";
                if (researchResult.maxDEFUpgradeAdd != 0)
                    resultTxt += "최대 방어 업그레이드 " + researchResult.maxDEFUpgradeAdd + " 증가\n";
                if (researchResult.maxSPUpgradeAdd != 0)
                    resultTxt += "최대 특수 업그레이드 " + researchResult.maxSPUpgradeAdd + " 증가\n";
                break;
            case EResearchType.Obstacle:
                if (researchResult.valueIncreaseRate != 0)
                    resultTxt += "능력 " + researchResult.valueIncreaseRate + "% 증가\n";
                if (researchResult.obstacleCostReduceRate != 0)
                    resultTxt += "방해물비용 " + researchResult.obstacleCostReduceRate + "% 증가\n";
                break;
            case EResearchType.Resource:
                if (researchResult.towerPartAddRate != 0)
                    resultTxt += "부품 획득 " + researchResult.towerPartAddRate + "% 증가\n";
                if (researchResult.mineralAddRate != 0)
                    resultTxt += "미네랄 획득 " + researchResult.mineralAddRate + "% 증가\n";
                if (researchResult.occasionalAddRate != 0)
                    resultTxt += "최대 수시 지급 " + researchResult.occasionalAddRate + "% 증가\n";
                if (researchResult.occasionalReduceTime != 0)
                    resultTxt += "수시 지급 시간 " + researchResult.occasionalReduceTime + "% 증가\n";
                break;
        }
        resultTxt= resultTxt.Remove(resultTxt.Length - 1);
        return resultTxt;
    }

    public string GetSelectResearchString(EResearchType researchType, EResearch research)
    {
        return ResearchString(researchType, ResearchValueInit(research));
    }
}
