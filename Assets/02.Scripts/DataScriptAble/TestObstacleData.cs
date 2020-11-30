using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestData", menuName = "Test/ObstacleData", order = 6)]
public class TestObstacleData : ScriptableObject
{
    public EObjectName objectName;
    public EObstacleType obstacleType;
    public EAttackAble attackAble;
    public TestIntVector2 size;
    public string obstacleName;
    public string description;
    public int durability;
    public int reduceValue;
    public int buildCost;
    public float[] values;
}
