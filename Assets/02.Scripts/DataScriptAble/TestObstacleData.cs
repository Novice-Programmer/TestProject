using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestData", menuName = "Test/ObstacleData", order = 6)]
public class TestObstacleData : ScriptableObject
{
    public EObstacleType obstacleName;
    public EAttackAble obstacleType;
    public int durability;
    public int reduceValue;
    public float[] values;
}
