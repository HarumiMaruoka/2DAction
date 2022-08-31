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
    protected bool Initialized_EventSystemBehavior()
    {
        Debug.Log("ggggggggggggggggggggggggggggggg");
        if (!(_eventSystem = GameObject.FindObjectOfType<EventSystem>())) return false;
        return true;
    }

    //<===== 便利系 =====>//
    /// <summary> 
    /// 選択していたオブジェクトの変化を検知する。
    /// 使用する場合、Update_UseEventSystemBehavior()をUpdate()で呼んでください。 
    /// </summary>
    /// <returns> 変化を検知したフレームで true を返す。 </returns>
    protected bool IsChangeSelectedObject()
    {
        return _eventSystem.currentSelectedGameObject != _beforeSelectedGameObject;
    }
    /// <summary> 
    /// UseEventSystemBehaviorのアップデート関数。
    /// </summary>
    protected void Update_UseEventSystemBehavior()
    {
        //今フレームで選択していたオブジェクトを保存。
        _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }
}
