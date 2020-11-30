using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESceneType
{
    Ingame,
}

public class TestSceneControlManager : MonoBehaviour
{
    public static ESceneType SceneType = ESceneType.Ingame;
    public static TestSceneControlManager Instance { set; get; }
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
