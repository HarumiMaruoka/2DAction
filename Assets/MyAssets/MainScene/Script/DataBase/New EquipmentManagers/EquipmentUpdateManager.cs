using UnityEngine;

/// <summary>
/// まだ実装していない。<br/>
/// 装備UIの更新を担当するクラス。<br/>
/// </summary>
public class EquipmentUpdateManager : UseEventSystemBehavior
{
    //===== フィールド / プロパティ =====//
    /// <summary> 装備更新時に呼び出されるデリゲート変数。 </summary>
    public System.Action<GameObject,GameObject> UpdateEquipmentUI;

    /// <summary> 
    /// 一時敵に保持していたいゲームオブジェクト :<br/>
    /// ただし、この変数の扱いには注意すること。<br/>
    /// </summary>
    GameObject _holdGameObject;

    //===== Unityメッセージ =====//
    void Update()
    {
        if(_beforeSelectedGameObject!=_eventSystem.currentSelectedGameObject&& UpdateEquipmentUI != null)
        {
            UpdateEquipmentUI(_eventSystem.currentSelectedGameObject, _beforeSelectedGameObject);
        }
    }
}