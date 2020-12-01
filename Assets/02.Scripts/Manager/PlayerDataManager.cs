using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance { set; get; }

    [SerializeField] List<EResearch> _playerAllResearch = new List<EResearch>();
    [SerializeField] List<ETowerType> _playerSelectTower = new List<ETowerType>();
    [SerializeField] List<EObstacleType> _playerSelectObstacle = new List<EObstacleType>();

    public int Stage = 0;

    private void Awake()
    {
        Instance = this;
    }

    public SelectTowerData[] GetPlayerSelectTower()
    {
        List<SelectTowerData> selectTowerDatas = new List<SelectTowerData>();

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
                    else if (researchData.target[k] == EResearchTarget.Tower || researchData.target[k] == EResearchTarget.ALL)
                    {
                        researchDatas.Add(researchData);
                    }
                }
            }
            selectTowerDatas.Add(new SelectTowerData(towerType, researchDatas.ToArray()));
        }

        return selectTowerDatas.ToArray();
    }

    public SelectObstacleData[] GetPlayerSelectObstacle()
    {
        List<SelectObstacleData> selectObstacleDatas = new List<SelectObstacleData>();

        for(int i = 0; i < _playerSelectObstacle.Count; i++)
        {
            EObstacleType obstacleType = _playerSelectObstacle[i];
            List<TestResearchData> researchDatas = new List<TestResearchData>();
            EResearchTarget target = EResearchTarget.None;
            switch (obstacleType)
            {
                case EObstacleType.FireWall:
                    target = EResearchTarget.FireWall;
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
                    else if (researchData.target[k] == EResearchTarget.Obstacle || researchData.target[k] == EResearchTarget.ALL)
                    {
                        researchDatas.Add(researchData);
                    }
                }
            }
            selectObstacleDatas.Add(new SelectObstacleData(obstacleType, researchDatas.ToArray()));
        }

        return selectObstacleDatas.ToArray();
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
