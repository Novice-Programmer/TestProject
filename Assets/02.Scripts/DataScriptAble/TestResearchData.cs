using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EResearchType
{
    Tower,
    Obstacle,
    Resource,
    TowerObstacle,
    ALL
}

public enum EResearch
{
    ReorganizatedOfDesign,
    AdvancedAITechnology,
    ResourceRobotDevelopment,

    None =999
}

public enum EResearchTarget
{
    KW9A,
    P013,
    Tower,
    FireWall,
    Obstacle,
    Resource,
    None = 999
}

[CreateAssetMenu(fileName = "TestData", menuName = "Test/TowerResearchData", order = 4)]
public class TestResearchData : ScriptableObject
{
    public long ID;
    public EResearchType type;
    public EResearch requireResearch;
    public EResearch research;
    public EResearchTarget[] target;
}
