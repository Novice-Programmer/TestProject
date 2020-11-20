using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestUIResource : MonoBehaviour
{
    [SerializeField] Text _towerPartsTxt = null;
    [SerializeField] Text _spaceMineralTxt = null;

    public void UIValueChange()
    {
        _towerPartsTxt.text = TestResourceManager.Instance.TowerPartValue.ToString();
        _spaceMineralTxt.text = TestResourceManager.Instance.SpaceMineralValue.ToString();
    }
}
