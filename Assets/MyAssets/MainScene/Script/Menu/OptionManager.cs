using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    //オプション管理用スクリプト
    //オプションのコンテンツ
    [SerializeField] GameObject _exitPanel;
    [SerializeField] GameObject _toolPanel;

    /// <summary> 終了確認ダイアログを表示する。 </summary>
    public void OnExitButton()
    {
        if (_exitPanel == null)
        {
            Debug.LogError("Exitパネルの取得に失敗しました。");
        }
        _exitPanel.SetActive(true);
    }

    /// <summary> 道具画面を表示する。 </summary>
    public void OnToolButton()
    {
        if (_toolPanel == null)
        {
            Debug.LogError("Toolパネルの取得に失敗しました。");
        }
        _toolPanel.SetActive(true);
    }
}
