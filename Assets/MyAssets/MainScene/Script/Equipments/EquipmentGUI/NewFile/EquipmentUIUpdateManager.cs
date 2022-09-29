using UnityEngine;

/// <summary>
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
        // 選択しているオブジェクトが変更された時デリゲートに登録された処理を呼び出す。
        if (_beforeSelectedGameObject != _eventSystem.currentSelectedGameObject)
        {
            if (ChangeSelectedObject != null)
            {
                ChangeSelectedObject(_eventSystem.currentSelectedGameObject, _beforeSelectedGameObject);
            }
        }
        // "着用している装備"と、"所持している装備"を交換したときに、デリゲートに登録された処理を呼び出す。
        if (EquipmentManager.Instance.IsSwap)
        {
            if (SwapEquipmentUpdate_UseSelectedGameObject != null)
            {
                SwapEquipmentUpdate_UseSelectedGameObject
                    (_eventSystem.currentSelectedGameObject, _beforeSelectedGameObject);
            }
            if (SwapEquipmentUpdate != null)
            {
                SwapEquipmentUpdate();
            }
            EquipmentManager.Instance.IsSwap = false;
        }
        // 次フレーム用に、現在の値を保存しておく。
        _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }
}