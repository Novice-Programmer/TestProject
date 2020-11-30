using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TestData", menuName = "Test/StageResourceData", order = 5)]
public class TestStageResourceData : ScriptableObject
{
    public int stage;
    public int maxWave;
    public int initialTP;
    public int regularTP;
    public int occasionalTP;
    public int waveAddTP;
    public int waveClearMineral;
    public int basicClearMineral;
    public int stageClearMineral;
    public int firstAllClearMineral;
}
