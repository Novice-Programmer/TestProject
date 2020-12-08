using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : TSingleton<PlayerDataManager>
{
    Dictionary<EResearchType, Dictionary<int, EResearch>> _playerSelectResearch;
    Dictionary<EObjectType, List<EObjectName>> _playerSelectObject;
    Dictionary<EResearchType, int> _playerResearchCheck;

    List<EResearch> SelectResearchList
    {
        get
        {
            List<EResearch> researches = new List<EResearch>();
            foreach (Dictionary<int, EResearch> stepResearch in _playerSelectResearch.Values)
            {
                foreach (EResearch research in stepResearch.Values)
                {
                    researches.Add(research);
                }
            }
            return researches;
        }
    }

    public int Stage = 0;

    private void Awake()
    {
        Init();
        Instance = this;
    }

    public void LoadData(Dictionary<EResearchType, Dictionary<int, EResearch>> selectResearch, Dictionary<EObjectType, List<EObjectName>> selectObject,
        Dictionary<EResearchType, int> checkResearch)
    {
        if (selectResearch != null)
        {
            _playerSelectResearch = selectResearch;
            ResearchManager.Instance.ResearchUpdate(SelectResearchList);
        }
        else
        {
            _playerSelectResearch = new Dictionary<EResearchType, Dictionary<int, EResearch>>();
        }
        if (selectObject != null)
        {
            _playerSelectObject = selectObject;
        }
        else
        {
            _playerSelectObject = new Dictionary<EObjectType, List<EObjectName>>();
        }

        if (checkResearch != null)
        {
            _playerResearchCheck = checkResearch;
        }
        else
        {
            _playerResearchCheck = new Dictionary<EResearchType, int>();
            _playerResearchCheck.Add(EResearchType.Tower, 3);
            _playerResearchCheck.Add(EResearchType.Obstacle, 3);
            _playerResearchCheck.Add(EResearchType.Resource, 3);
            ResearchUpdate(EResearchType.Tower, 0, EResearch.TowerLv0);
            ResearchUpdate(EResearchType.Obstacle, 0, EResearch.ObstacleLv0);
            ResearchUpdate(EResearchType.Resource, 0, EResearch.ResourceLv0);
            ResearchManager.Instance.ResearchUpdate(SelectResearchList);
        }
    }

    public void ResearchUpdate(EResearchType researchType, int step, EResearch research)
    {
        Dictionary<int, EResearch> stepResearch;
        if (_playerSelectResearch.ContainsKey(researchType))
        {
            stepResearch = _playerSelectResearch[researchType];
        }
        else
        {
            stepResearch = new Dictionary<int, EResearch>();
            _playerSelectResearch.Add(researchType, stepResearch);
        }
        if (stepResearch.ContainsKey(step))
        {
            if (research == EResearch.None)
            {
                stepResearch.Remove(step);
            }
            else
            {
                stepResearch[step] = research;
            }
        }
        else
        {
            stepResearch.Add(step, research);
        }
        ResearchManager.Instance.ResearchUpdate(SelectResearchList);
    }

    public EResearch GetSelectResearch(EResearchType researchType, int step)
    {
        if (_playerSelectResearch.ContainsKey(researchType))
        {
            Dictionary<int, EResearch> stepResearch = _playerSelectResearch[researchType];
            if (stepResearch.ContainsKey(step))
                return _playerSelectResearch[researchType][step];
        }
        return EResearch.None;
    }

    public bool GetResearchCheck(EResearchType researchType, int step)
    {
        if (_playerResearchCheck[researchType] >= step)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
