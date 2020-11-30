using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestStageManager : MonoBehaviour
{
    public int nowStage = 1;

    static TestStageManager _uniqueInstance;

    public static TestStageManager Instance { get { return _uniqueInstance; } }

    private void Awake()
    {
        _uniqueInstance = this;
    }
}
