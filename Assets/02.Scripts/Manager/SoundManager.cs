using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ESoundBGM
{
    Lobby,
    Mars
}

public enum ESoundEffect
{
    LoadEnd,
    PlayerMove,
}

public class SoundManager : TSingleton<SoundManager>
{
    float _volumeBgm = 0.5f;
    float _volumeEff = 0.5f;

    bool _muteBgm = false;
    bool _muteEff = false;

    [SerializeField] AudioClip[] _bgmClips = null;
    [SerializeField] AudioClip[] _effClips = null;

    AudioSource _playerBGM;

    List<AudioSource> _ltPlayEffect = new List<AudioSource>();

    public float VolumeBGM
    {
        get { return _volumeBgm; }
        private set
        {
            _volumeBgm = value;
            _playerBGM.volume = value;
        }
    }

    public float VolumeEffect
    {
        get { return _volumeEff; }
        private set { _volumeEff = value; }
    }

    public bool MuteBGM
    {
        get { return _muteBgm; }
        private set
        {
            _muteBgm = value;
            _playerBGM.mute = value;
        }
    }

    public bool MuteEff
    {
        get { return _muteEff; }
        private set { _muteEff = value; }
    }

    protected override void Init()
    {
        base.Init();
        Instance = this;
    }

    private void Awake()
    {
        Init();
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _ltPlayEffect.Count; i++)
        {
            if (_ltPlayEffect[i] == null)
            {
                _ltPlayEffect.RemoveAt(i);
                break;
            }
            if (!_ltPlayEffect[i].isPlaying)
            {
                Destroy(_ltPlayEffect[i].gameObject);
                _ltPlayEffect.RemoveAt(i);
                break;
            }
        }
    }

    public AudioSource PlayerBGMSound(ESoundBGM bgmType)
    {
        if (_playerBGM != null)
            Destroy(_playerBGM.gameObject);
        GameObject obj = new GameObject("BGMPlayer");
        obj.transform.parent = Camera.main.transform;
        obj.transform.localPosition = Vector3.zero;

        _playerBGM = obj.AddComponent<AudioSource>();
        _playerBGM.clip = _bgmClips[(int)bgmType];
        _playerBGM.volume = _volumeBgm;
        _playerBGM.mute = _muteBgm;
        _playerBGM.loop = true;
        _playerBGM.Play();

        return _playerBGM;
    }

    public AudioSource PlayEffectSound(ESoundEffect soundType, Transform owner, bool loop = false, float soundDistance = 50.0f)
    {
        GameObject obj = new GameObject("EffectSound");
        if (owner != null)
            obj.transform.parent = owner;
        else
            obj.transform.parent = Camera.main.transform;
        transform.localPosition = Vector3.zero;
        AudioSource effPlayer = obj.AddComponent<AudioSource>();
        effPlayer.clip = _effClips[(int)soundType];
        effPlayer.volume = _volumeEff;
        effPlayer.mute = _muteEff;
        effPlayer.loop = loop;
        effPlayer.minDistance = soundDistance;
        effPlayer.Play();
        _ltPlayEffect.Add(effPlayer);

        return effPlayer;
    }
}
