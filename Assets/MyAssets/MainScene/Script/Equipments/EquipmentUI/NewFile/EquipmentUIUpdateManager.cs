using UnityEngine;

/// <summary>
/// まだ実装していない。<br/>
/// 装備UIの更新を担当するクラス。<br/>
/// </summary>
public class EquipmentUIUpdateManager : UseEventSystemBehavior
{
    //===== フィールド / プロパティ =====//
    /// <summary> 選択しているボタンが変更されたタイミングで呼び出されるデリゲート変数。 </summary>
    public static System.Action<GameObject, GameObject> ChangeEquipmentButtonUpdate;
    /// <summary> 装備更新時に呼び出されるデリゲート変数。 </summary>
    public static System.Action SwapEquipmentUpdate;

    //===== Unityメッセージ =====//
    void Update()
    {
        // 選択しているオブジェクトが変更された時、かつ、UpdateEquipmentUIに値が入っている時に呼び出す。
        if (_beforeSelectedGameObject != _eventSystem.currentSelectedGameObject && ChangeEquipmentButtonUpdate != null)
        {
            ChangeEquipmentButtonUpdate(_eventSystem.currentSelectedGameObject, _beforeSelectedGameObject);
        }
        if (EquipmentManager.Instance.IsSwap && SwapEquipmentUpdate != null)
        {
            EquipmentManager.Instance.IsSwap = false;
            SwapEquipmentUpdate();
        }
        _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }
}