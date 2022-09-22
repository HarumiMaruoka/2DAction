using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装備UIに関わるコンポーネントが継承すべきクラス
/// </summary>
public class EquipmentUIBase : UseEventSystemBehavior
{
    //===== フィールド / プロパティ =====//
    /// <summary>
    /// 装備のUIに関するコンポーネントを集めたクラスの変数。
    /// </summary>
    protected EquipmentUIComponentsManager _equipmentUIComponentsManager = default;

    protected override void Start()
    {
        base.Start();
        if ((_equipmentUIComponentsManager = GameObject.FindObjectOfType<EquipmentUIComponentsManager>()) == null)
        {
            Debug.Log(
                "\"EquipmentUIComponentsManager\"の取得に失敗しました。\n" +
                "確認してください");
        }
        
    }
}
