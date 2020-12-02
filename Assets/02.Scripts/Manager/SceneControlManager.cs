using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESceneType
{
    Ingame,
}

public class SceneControlManager : MonoBehaviour
{
    public static ESceneType SceneType = ESceneType.Ingame;
    public static SceneControlManager Instance { set; get; }
    private void Awake()
    {
        Instance = this;
    }
}
