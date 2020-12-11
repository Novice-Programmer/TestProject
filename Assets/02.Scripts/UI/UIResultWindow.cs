using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIResultWindow : MonoBehaviour
{
    [SerializeField] GameObject _clearObject = null;
    [SerializeField] GameObject _overObject = null;
    [SerializeField] Text _waveTxt = null;
    [SerializeField] Text _enemyTxt = null;

    Animator _endAnim;
    private void Awake()
    {
        _endAnim = GetComponent<Animator>();
    }
    public void GameEnd(bool clear, int wave, int enemyDie)
    {
        if (clear)
        {
            _endAnim.SetTrigger("Clear");
            _clearObject.SetActive(true);
        }
        else
        {
            _endAnim.SetTrigger("GameOver");
            _overObject.SetActive(true);
        }
        _waveTxt.text = "Clear Wave " + wave.ToString();
        _enemyTxt.text = "Enemy " + enemyDie.ToString();
    }

    public void ClickReGame()
    {
        SceneControlManager.Instance.SceneChange(ESceneType.Ingame);
    }

    public void ClickLobby()
    {
        SceneControlManager.Instance.SceneChange(ESceneType.Lobby);
    }
}
