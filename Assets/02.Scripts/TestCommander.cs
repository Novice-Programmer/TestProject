using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCommander : ObjectGame
{
    public static TestCommander Instance { set; get; }
    [SerializeField] TestWorldStatusUI _statusUI = null;

    [SerializeField] int _hp = 500;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        _statusUI.StatusSetting(_hp);
        _statusUI.HPChange(_hp);
        TestGameUI.Instance.CommanderSetting(_hp);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Hit(int damage, EWeakType weakType)
    {
        _hp -= damage;
        _statusUI.HPChange(_hp);
        if (_hp <= 0)
        {
            _hp = 0;
            TestGameManager.Instance.GameEnd();
        }
        TestGameUI.Instance.CommanderHit(_hp);
    }
}
