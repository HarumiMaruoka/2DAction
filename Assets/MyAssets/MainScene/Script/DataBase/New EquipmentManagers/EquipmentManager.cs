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
    public void Swap_HaveToEquipped(EquipmentID fromNowEquipmentID, Equipment.EquipmentType fromNowEquipmentType, EquipmentButton button, int armFlag = -1)
    {
        //Debug.Log("これから着用する装備のID : " + fromNowEquipmentID);
        //Debug.Log("これから着用する装備のType : " + fromNowEquipmentType);

        EquipmentID temporary = EquipmentID.None;//仮の入れ物
        //Typeを基に着用する
        //腕以外の場合
        if (fromNowEquipmentType != Equipment.EquipmentType.ARM_PARTS)
        {
            switch (fromNowEquipmentType)
            {
                //頭パーツの場合
                case Equipment.EquipmentType.HEAD_PARTS:
                    temporary = _currentEquippedData.Equipped._headPartsID;
                    _currentEquippedData.ChangeEquipped(Equipment.EquipmentType.HEAD_PARTS, Constants.NOT_ARM, fromNowEquipmentID);
                    //_currentEquippedData.Equipped._headPartsID = fromNowEquipmentID;
                    break;

                //胴パーツの場合
                case Equipment.EquipmentType.TORSO_PARTS:
                    temporary = _equipped._torsoPartsID;
                    _equipped._torsoPartsID = fromNowEquipmentID;
                    break;

                //足パーツの場合
                case Equipment.EquipmentType.FOOT_PARTS:
                    temporary = _equipped._footPartsID;
                    _equipped._footPartsID = fromNowEquipmentID;
                    break;
            }
            //着脱した装備をインベントリに格納する
            if (temporary != -1) button.Set_Equipment(EquipmentData[temporary]);
            else button.Set_Equipment(null);
            //表示を更新する
            _draw_NowEquipped.Update_Equipped(fromNowEquipmentType);
            if (temporary != -1) _managerOfPossessedEquipment.Update_RiseValueText(EquipmentData[temporary]);
        }
        //腕の場合
        else
        {
            if (armFlag == 0)
            {
                temporary = _equipped._armLeftPartsID;
                _equipped._armLeftPartsID = fromNowEquipmentID;
            }
            else if (armFlag == 1)
            {
                temporary = _equipped._armRightPartsID;
                _equipped._armRightPartsID = fromNowEquipmentID;
            }
            else
            {
                Debug.LogError($"不正な値です{armFlag}");
            }
            //着脱した装備をインベントリに格納する
            if (temporary != -1) button.Set_Equipment(EquipmentData[temporary]);
            else button.Set_Equipment(null);
            //表示を更新する
            _draw_NowEquipped.Update_Equipped(fromNowEquipmentType, armFlag);
            if (temporary != -1) _managerOfPossessedEquipment.Update_RiseValueText(EquipmentData[temporary]);
        }
    }
}
