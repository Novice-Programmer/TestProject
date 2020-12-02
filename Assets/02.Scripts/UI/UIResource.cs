using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResource : MonoBehaviour
{
    [SerializeField] Text _towerPartsTxt = null;
    [SerializeField] Text _spaceMineralTxt = null;

    public void UIValueChange()
    {
        _towerPartsTxt.text = ResourceManager.Instance.TowerPartValue.ToString();
        _spaceMineralTxt.text = ResourceManager.Instance.SpaceMineralValue.ToString();
    }
}
