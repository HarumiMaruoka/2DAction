using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// イベントシステムを利用するクラスが継承すべきクラス。
/// イベントシステムを利用するうえで、様々な便利機能を提供する。
/// </summary>
public class UseEventSystemBehavior : MonoBehaviour
{
    //<===== メンバー変数 =====>//
    /// <summary> イベントシステム </summary>
    protected EventSystem _eventSystem;
    /// <summary> 前フレームで選択していたゲームオブジェクト(Button) </summary>
    protected GameObject _beforeSelectedGameObject;

    //<===== 初期化処理系 =====>//
    /// <summary> UseEventSystemBehaviorクラスの初期化処理 </summary>
    /// <returns> 初期化成功の可否を返す。成功したら true 。 </returns>
    protected bool Init()
    {
        if ((_eventSystem = GameObject.FindObjectOfType<EventSystem>()) == null)
        {
            Debug.Log("\"EventSystem\"の取得に失敗しました。");
            return false;
        }
        return true;
    }
}
