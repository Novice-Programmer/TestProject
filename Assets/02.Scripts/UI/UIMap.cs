using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMap : MonoBehaviour
{
    [SerializeField] Text _mapWaveTxt = null;
    [SerializeField] CanvasGroup _mapWaveTxtCG = null;
    [SerializeField] float _waveTxtViewTime = 1f;

    bool _view = false;
    bool _waveUpdate = true;

    private void Update()
    {
        if (_waveUpdate)
        {
            if (_mapWaveTxtCG.alpha <= 0)
            {
                _waveUpdate = false;
                return;
            }
            _mapWaveTxtCG.alpha -= Time.deltaTime / (_waveTxtViewTime * 1.5f);
        }
        else
        {
            if (_view)
            {
                if (_mapWaveTxtCG.alpha < 1)
                    _mapWaveTxtCG.alpha += Time.deltaTime / _waveTxtViewTime;
            }
            else
            {
                if (_mapWaveTxtCG.alpha > 0)
                    _mapWaveTxtCG.alpha -= Time.deltaTime / _waveTxtViewTime;
            }
        }
    }

    public void WaveSetting(int waveNumber)
    {
        _mapWaveTxt.text = "Wave " + (waveNumber+1).ToString();
        _mapWaveTxtCG.alpha = 1;
        _waveUpdate = true;
    }

    public void OnPointerMap(bool pointerEnter)
    {
        _view = pointerEnter;
    }
}
