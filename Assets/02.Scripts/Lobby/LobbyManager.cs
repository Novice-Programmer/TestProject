using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : TSingleton<LobbyManager>
{
    [SerializeField] GameObject _playerUI = null;

    bool _loadEndCheck = true;
    float _timeCheck;

    private void Awake()
    {
        Instance = this;
        Init();
    }

    private void Update()
    {
        if (_loadEndCheck)
        {
            if(SceneControlManager.Instance.NowLoaddingState == ELoaddingState.None)
            {
                _loadEndCheck = false;
                SoundManager.Instance.PlayerBGMSound(ESoundBGM.Lobby);
            }
        }
        else
        {

        }
    }

    public void PlanetSelect(bool open)
    {
        _playerUI.SetActive(!open);
    }
}
