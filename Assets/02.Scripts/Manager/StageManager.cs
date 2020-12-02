using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public int nowStage = 1;

    static StageManager _uniqueInstance;

    public static StageManager Instance { get { return _uniqueInstance; } }

    private void Awake()
    {
        _uniqueInstance = this;
    }
}
