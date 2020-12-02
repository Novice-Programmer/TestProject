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
    None,
    ReorganizatedOfDesign,
    AdvancedAITechnology,
    ResourceRobotDevelopment,
}


[CreateAssetMenu(fileName = "GameData", menuName = "Data/TowerResearchData", order = 4)]
public class ResearchData : ScriptableObject
{
    public EResearch research;
    public EResearch requireResearch;
    public EResearchType type;
    public EObjectName[] targetNames;
}
