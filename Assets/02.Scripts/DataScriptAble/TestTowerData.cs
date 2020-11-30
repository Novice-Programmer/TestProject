using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETowerType
{
    None,
    KW9A,
    P013,
}

[CreateAssetMenu(fileName = "TestData", menuName = "Test/TowerData", order = 2)]
public class TestTowerData : ScriptableObject
{
    public EObjectName objectName;
    public ETowerType towerType; // 타워 종류
    public string towerName; // 타워 이름
    public string description; // 설명
    public TestIntVector2 size; // 타워 크기
    public int hp; // 체력
    public int ep; // 전력
    public int atk; // 공격력
    public int def; // 방어력
    public float atkSpd; // 공격 속도
    public float atkRange; // 공격 범위
    public int atkNumber; // 공격 횟수
    public int targetNumber; // 타겟 수
    public int maxUpgrade; // 최대 업그레이드
    public int maxSpUpgrade; // 최대 특수 업그레이드
    public float[] spValue; // 특수 능력치

    public int buildCost; // 설치 비용
    public int defUpgradeCost; // 방어 업그레이드 비용
    public int atkUpgradeCost; // 공격 업그레이드 비용
    public int spUpgradeCost; // 특수 업그레이드 비용

    private void OnValidate()
    {
        if (size.x <= 0)
        {
            size.x = 1;
        }
        if (size.y <= 0)
        {
            size.y = 1;
        }
    }
}