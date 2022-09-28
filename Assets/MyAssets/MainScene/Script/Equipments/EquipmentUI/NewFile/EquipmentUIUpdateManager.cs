using UnityEngine;

/// <summary>
/// まだ実装していない。<br/>
/// 装備UIの更新を担当するクラス。<br/>
/// </summary>
public class EquipmentUIUpdateManager : UseEventSystemBehavior
{
    //===== フィールド =====//
    /// <summary>
    /// 選択しているボタンが変更されたタイミングで呼び出されるデリゲート変数。<br/>
    /// 第1引数には現在選択中のゲームオブジェクトが渡され、<br/>
    /// 第2引数には前フレームで選択されていたゲームオブジェクトが渡される。<br/>
    /// </summary>
    public static System.Action<GameObject, GameObject> ChangeSelectedObject;
    /// <summary> 
    /// 装備更新時に呼び出されるデリゲート変数。 <br/>
    /// 第1引数には現在選択中のゲームオブジェクトが渡され、<br/>
    /// 第2引数には前フレームで選択されていたゲームオブジェクトが渡される。<br/>
    /// </summary>
    public static System.Action<GameObject, GameObject> SwapEquipmentUpdate_UseSelectedGameObject;

    /// <summary> 
    /// 装備更新時に呼び出されるデリゲート変数。<br/> 
    /// </summary>
    public static System.Action SwapEquipmentUpdate;

    //===== Unityメッセージ =====//
    void Update()
    {
        // 選択しているオブジェクトが変更された時、かつ、UpdateEquipmentUIに値が入っている時に呼び出す。
        if (_beforeSelectedGameObject != _eventSystem.currentSelectedGameObject && ChangeSelectedObject != null)
        {
            ChangeSelectedObject(_eventSystem.currentSelectedGameObject, _beforeSelectedGameObject);
        }
        if (EquipmentManager.Instance.IsSwap)
        {
            SwapEquipmentUpdate_UseSelectedGameObject
                (_eventSystem.currentSelectedGameObject, _beforeSelectedGameObject);
            SwapEquipmentUpdate();
            EquipmentManager.Instance.IsSwap = false;
        }
        _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }
}