using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 選択中の装備を捨てるボタン <br/>
/// </summary>
public class EquipLostButton : UseEventSystemBehavior
{
    /// <summary> 保持する用の入れ物 </summary>
    GameObject _holdObject = default;
    [Header("確認用パネル"), SerializeField]
    GameObject _confirmationPanel;

    //===== Unityメッセージ =====//
    protected override void Start()
    {
        base.Start();
    }

    //===== ボタンから呼び出す想定で作成されたメソッド =====//
    /// <summary>
    /// 選択中のオブジェクトを保持する。
    /// </summary>
    public void HoldObject()
    {
        // 保持する処理。
        _holdObject = _beforeSelectedGameObject;
        // 確認用パネルを表示する。
        if(_holdObject != null && _holdObject.TryGetComponent(out EquipmentButton equipment))
        {
            _confirmationPanel.SetActive(true);
        }
    }
    /// <summary>
    /// 装備を捨てる。
    /// </summary>
    public void LostEquipment()
    {
        if (_holdObject != null && _holdObject.TryGetComponent(out EquipmentButton equipment))
        {
            EquipmentManager.Instance.EquippedLost((int)equipment._myEquipment._myID);
            ReleaseHoldObject();
        }
        else
        {
            Debug.LogError($"ホールドしているオブジェクトは、nullか、EquipmentButtonコンポーネントを持っていません。");
        }
    }
    /// <summary>
    /// 保持中のオブジェクトを開放する。
    /// </summary>
    public void ReleaseHoldObject()
    {
        _holdObject = null;
    }
}
