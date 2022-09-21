using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 装備UIに関わるコンポーネントが継承すべきクラス
/// </summary>
public class EquipmentUIBase : MonoBehaviour
{
    //===== フィールド / プロパティ =====//
    /// <summary>
    /// 装備のUIに関するコンポーネントを集めたクラスの変数。
    /// </summary>
    protected EquipmentUIComponentsManager _equipmentUIComponentsManager = default;
    /// <summary> 
    /// イベントシステム 
    /// </summary>
    protected EventSystem _eventSystem = default;
    /// <summary>
    /// 前フレームで選択していたゲームオブジェクト
    /// </summary>
    protected GameObject _beforeSelectedGameObject = default;

    protected virtual void Start()
    {
        if ((_equipmentUIComponentsManager = GameObject.FindObjectOfType<EquipmentUIComponentsManager>()) == null)
        {
            Debug.Log(
                "\"EquipmentUIComponentsManager\"の取得に失敗しました。\n" +
                "確認してください");
        }
        if ((_eventSystem = GameObject.FindObjectOfType<EventSystem>()) == null)
        {
            Debug.Log(
                "\"EventSystem\"の取得に失敗しました。\n" +
                "確認してください");
        }
    }
}
