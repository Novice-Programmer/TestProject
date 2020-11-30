using System;

public enum ESpecialResearch
{
    KW9A_MultipleRocket,
    KW9A_BigBangRocket,
    None
}

[Serializable]
public class TestTowerResearchResult
{
    public float hpAddRate; // 체력 증가량
    public float epAddRate; // 전력 증가량
    public float atkAddRate; // 공격력 증가량
    public float defAddRate; // 방어력 증가량
    public float atkSpdRate; // 공격 속도 증가량
    public float atkRangeRate; // 공격 범위 증가량
    public float[] spValueRate; // 특수 능력치 증가량
    public float costReduceRate; // 비용 감소량
    public int maxUpgradeAdd; // 최대 업그레이드 증가량
    public ESpecialResearch specialResearch;
}
