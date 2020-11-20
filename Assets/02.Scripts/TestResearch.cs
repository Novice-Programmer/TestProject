using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestResearch : MonoBehaviour
{
    public static TestResearch Instance { set; get; }
    [SerializeField] TestResearchData[] _researchDatas;

    public void Awake()
    {
        Instance = this;
    }

    public void ResearchGameSetting(TestResearchData[] researchDatas)
    {
        _researchDatas = researchDatas;
    }

    public TestResearchData[] ResearchResultPass(EResearchType type, EResearchTarget target)
    {
        List<TestResearchData> researchDatas = new List<TestResearchData>();

        for(int i = 0; i < _researchDatas.Length; i++)
        {
            if(type == EResearchType.Tower)
            {
                if (_researchDatas[i].target.Length > 1)
                {
                    for (int j = 0; j < _researchDatas[j].target.Length; j++)
                    {
                        if (_researchDatas[i].target[j] == target)
                        {
                            researchDatas.Add(_researchDatas[j]);
                        }
                    }
                }
                else
                {
                    if (_researchDatas[i].target[0] == target || _researchDatas[i].target[0] == EResearchTarget.Tower)
                    {
                        researchDatas.Add(_researchDatas[i]);
                    }
                }
                
            }
            else if(type == EResearchType.Obstacle)
            {

            }
            else
            {

            }
        }

        return researchDatas.ToArray();
    }
}
