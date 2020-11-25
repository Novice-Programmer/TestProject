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
    [SerializeField] float _limitViewTime = 3.0f;
    float _timeCheck = 0;

    private void Start()
    {
        _timeCheck = _limitViewTime;
    }

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
        transform.LookAt(Camera.main.transform);
    }


}
