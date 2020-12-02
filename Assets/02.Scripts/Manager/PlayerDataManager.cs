using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { set; get; }

    [SerializeField] List<EResearch> _playerAllResearch = new List<EResearch>();
    [SerializeField] List<ObjectData> _selectObjects = new List<ObjectData>();

    public int Stage = 0;

    private void Awake()
    {
        Instance = this;
    }

    public SelectData[] GetSelectObject()
    {
        List<SelectData> selectDatas = new List<SelectData>();

        for (int i = 0; i < _selectObjects.Count; i++)
        {
            List<ResearchData> researchDatas = new List<ResearchData>();
            for (int j = 0; j < _playerAllResearch.Count; j++)
            {
                ResearchData researchData = ResearchManager.Instance.GetResearchData(_playerAllResearch[j]);
                for (int k = 0; k < researchData.targetNames.Length; k++)
                {
                    if (researchData.targetNames[k] == _selectObjects[i].objectName)
                    {
                        researchDatas.Add(researchData);
                    }

                    else if (researchData.targetNames[k] == EObjectName.ALL)
                    {
                        researchDatas.Add(researchData);
                    }

                    if (_selectObjects[i].objectType == EObjectType.Tower && researchData.targetNames[k] == EObjectName.Tower)
                    {
                        researchDatas.Add(researchData);
                    }
                    else if (_selectObjects[i].objectType == EObjectType.Obstacle && researchData.targetNames[k] == EObjectName.Obstacle)
                    {
                        researchDatas.Add(researchData);
                    }
                }
            }
            selectDatas.Add(new SelectData(_selectObjects[i].objectType, _selectObjects[i].objectName, researchDatas.ToArray()));
        }

        return selectDatas.ToArray();
    }

    public ResearchData[] GetResourceResearchData()
    {
        List<ResearchData> researchDatas = new List<ResearchData>();
        for (int i = 0; i < _playerAllResearch.Count; i++)
        {
            ResearchData researchData = ResearchManager.Instance.GetResearchData(_playerAllResearch[i]);
            if (researchData.type == EResearchType.Resource)
            {
                researchDatas.Add(researchData);
            }
        }
        return researchDatas.ToArray();
    }
}
