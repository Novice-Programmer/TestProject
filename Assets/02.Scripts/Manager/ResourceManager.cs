using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPaymentType
{
    Initial,
    Regular,
    Occasional
}

public class ResourceManager : TSingleton<ResourceManager>
{
    int ingameValue = 0;
    int mineralValue = 0;
    public int TowerPartValue { get { return ingameValue; } set { ingameValue += value; ValueChange(); } }
    public int SpaceMineralValue { get { return mineralValue; } set { mineralValue += value; ValueChange(); } }

    [SerializeField] StageResourceData[] _stageResourceDatas = null;
    StageResourceData _stageData;

    ResourceResearchResult _researchResult = new ResourceResearchResult();
    private void Awake()
    {
        Init();
        Instance = this;
    }

    public void GameResourceSetting(ResearchData[] researchDatas)
    {
        _stageData = _stageResourceDatas[StageManager.Instance.NowStage];

        for (int i = 0; i < researchDatas.Length; i++)
        {
            switch (researchDatas[i].research)
            {
                default:
                    break;
            }
        }
    }

    public void TowerPartPayment(EPaymentType paymentType, int wave)
    {
        switch (paymentType)
        {
            case EPaymentType.Initial:
                TowerPartValue = _stageData.initialTP + (int)(_stageData.initialTP * 0.01f * _researchResult.towerPartAddRate);
                break;
            case EPaymentType.Regular:
                TowerPartValue = _stageData.regularTP + (int)((_stageData.regularTP+wave * _stageData.waveAddTP) * 0.01f * _researchResult.towerPartAddRate);
                break;
            case EPaymentType.Occasional:
                TowerPartValue = _stageData.occasionalTP + (int)(_stageData.occasionalTP * 0.01f * _researchResult.towerPartAddRate);
                break;
        }
    }

    public void WaveClear(int wave)
    {
        if (wave == 0)
        {
            SpaceMineralValue = _stageData.basicClearMineral + (int)(_stageData.basicClearMineral * 0.01f * _researchResult.mineralAddRate);
        }
        else if(wave == _stageData.maxWave - 1)
        {
            if (_stageData.stage > PlayerDataManager.Instance.Stage)
            {
                PlayerDataManager.Instance.Stage = _stageData.stage;
                SpaceMineralValue = _stageData.firstAllClearMineral;
            }
            SpaceMineralValue = _stageData.stageClearMineral + (int)(_stageData.stageClearMineral * 0.01f * _researchResult.mineralAddRate);
        }
        SpaceMineralValue = _stageData.waveClearMineral + (int)(_stageData.waveClearMineral * 0.01f * _researchResult.mineralAddRate);
    }

    void ValueChange()
    {
        if (SceneControlManager.SceneType == ESceneType.Ingame)
        {
            GameUI.Instance.ResourceValueChange();
        }
        else
        {

        }
    }
}
