using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : TSingleton<LoadManager>
{
    public void Start()
    {
        Load();
    }

    public void Save()
    {

    }

    public void Load()
    {
        PlayerDataManager.Instance.LoadData(null, null, null);
    }
}
