using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// ゲーム内で使用する装備の情報を管理するクラス。<br/>
/// 持っている機能 : 
/// </para>
/// <para>
/// ・全ての装備の情報 <br/>
/// ・プレイヤーが"所持"している装備の情報 <br/>
/// ・プレイヤーが"着用"している装備の情報
/// </para>
/// <para>
/// このクラスはシングルトンで実装する。<br/>
/// </para>
/// </summary>
public class EquipmentManager
{
    //===== シングルトン関連 =====//
    /// <summary>
    /// このクラスの唯一の実体
    /// </summary>
    static private EquipmentManager _instance = default;
    /// <summary>
    /// インスタンスのプロパティ
    /// </summary>
    public EquipmentManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EquipmentManager();
            }
            return _instance;
        }
    }
    /// <summary>
    /// privateなコンストラクタ
    /// </summary>
    private EquipmentManager()
    {

    }

    //===== フィールド / プロパティ =====//
    [Tooltip("全ての装備の情報")]
    NewEquipmentDataBase _newEquipmentDataBase = new NewEquipmentDataBase();
    public NewEquipmentDataBase NewEquipmentDataBase => _newEquipmentDataBase;
    [Tooltip("プレイヤーが\"現在着用している\"装備の情報")]
    CurrentEquippedDataBase _currentEquippedData = new CurrentEquippedDataBase();
    public CurrentEquippedDataBase CurrentEquippedData => _currentEquippedData;
    [Tooltip("プレイヤーが\"現在所持している\"装備の情報")]
    HaveEquipmentDataBase _haveEquipmentData = new HaveEquipmentDataBase();
    public HaveEquipmentDataBase HaveEquipmentData => _haveEquipmentData;

    //===== publicメソッド =====//
    /// <summary>
    /// "所持"している装備と、"着用"している装備を交換する。
    /// </summary>
    /// <param name="fromNowEquipmentID"></param>
    /// <param name="fromNowEquipmentType"></param>
    /// <param name="button"></param>
    /// <param name="armFlag"></param>
    /// 
    public void Swap_HaveToEquipped(EquipmentID fromNowEquipmentID, Equipment.EquipmentType fromNowEquipmentType, EquipmentButton button, int armFlag = Constants.NOT_ARM)
    {
        // 仮の入れ物
        EquipmentID temporary = EquipmentID.None;

        // 現在着用している装備を取得し保存する。
        switch (fromNowEquipmentType)
        {
            case Equipment.EquipmentType.HEAD_PARTS:
                temporary = _currentEquippedData.Equipped._headPartsID;
                break;
            case Equipment.EquipmentType.TORSO_PARTS:
                temporary = _currentEquippedData.Equipped._torsoPartsID;
                break;
            case Equipment.EquipmentType.ARM_PARTS:
                if (armFlag == Constants.LEFT_ARM) temporary = _currentEquippedData.Equipped._armLeftPartsID;
                else if (armFlag == Constants.RIGHT_ARM) temporary = _currentEquippedData.Equipped._armRightPartsID;
                else Debug.LogError("不正な値です。");
                break;
            case Equipment.EquipmentType.FOOT_PARTS:
                temporary = _currentEquippedData.Equipped._footPartsID;
                break;
        }
        //Typeを基に着用する
        _currentEquippedData.ChangeEquipped(fromNowEquipmentType, fromNowEquipmentID, armFlag);
        //着脱した装備をインベントリに格納する
        HaveEquipmentData.GetEquipment((int)temporary);

        //ボタンに装備を設定する。
        if (temporary != EquipmentID.None)
            button.Set_Equipment(NewEquipmentDataBase.EquipmentData[(int)temporary]);
        else button.Set_Equipment(null);
    }
}
