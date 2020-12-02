using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TestWorldStatusUI : MonoBehaviour
{
    [SerializeField] Slider _hpBar = null;
    [SerializeField] Text _hpTxt = null;
    [SerializeField] Slider _mpBar = null;
    [SerializeField] Text _mpTxt = null;
    float _limitViewTime = 3.0f;
    float _timeCheck = 0;
    float _yHeight = 3.0f;

    [SerializeField] Transform _target;

    private void LateUpdate()
    {
        if (gameObject.activeSelf)
        {
            _timeCheck += Time.deltaTime;
            if (_timeCheck >= _limitViewTime)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        if (_target == null)
        {
            gameObject.SetActive(false);
            return;
        }
        Vector3 pos = _target.position;
        pos.y += _yHeight;
        transform.position = pos;
        transform.LookAt(transform.position + Camera.main.transform.forward);
    }

    public void StatusSetting(Transform target, int maxHP, float limitViewTime, bool mpCheck = true)
    {
        gameObject.SetActive(true);
        _target = target;
        _hpBar.maxValue = maxHP;
        _hpBar.value = maxHP;
        _hpTxt.text = _hpBar.value + " / " + maxHP;
        if (!mpCheck)
        {
            _mpBar.gameObject.SetActive(false);
        }
        _timeCheck = 0;
        _limitViewTime = limitViewTime;
    }

    public void StatusSetting(int maxHP)
    {
        _hpBar.maxValue = maxHP;
        _hpTxt.text = _hpBar.value + " / " + maxHP;
    }

    public void HPChange(int hp)
    {
        _hpBar.value = hp;
        gameObject.SetActive(true);
        _hpTxt.text = hp + " / " + _hpBar.maxValue;
        _timeCheck = 0;
        if (hp <= 0)
            gameObject.SetActive(false);
    }

    public void MPChange(int mp)
    {
        _mpBar.value = mp;
        gameObject.SetActive(true);
        _mpTxt.text = mp + " / 100";
        _timeCheck = 0;
    }
}
