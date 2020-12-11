using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectedMiniUI : MonoBehaviour
{
    [SerializeField] Sprite _lockSprite = null;
    [SerializeField] Image _background = null;
    [SerializeField] Image _icon = null;

    public void SelectedSetting(Sprite icon)
    {
        _icon.sprite = icon;
    }

    public void NoneSelectSetting()
    {
        _background.color = Color.red;
    }

    public void LockSelectSetting()
    {
        _icon.sprite = _lockSprite;
        _icon.color = Color.red;
    }
}
