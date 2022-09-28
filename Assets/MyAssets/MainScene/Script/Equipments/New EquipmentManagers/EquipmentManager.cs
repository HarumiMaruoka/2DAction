using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// 装備に関わる情報を管理するクラス。<br/>
/// プレイヤーが目にすることがない内部的な情報 <br/>
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
    private static EquipmentManager _instance = default;
    /// <summary>
    /// インスタンスのプロパティ
    /// </summary>
    public static EquipmentManager Instance
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
    /// <summary> 全ての装備の情報のデータベース </summary>
    public NewEquipmentDataBase NewEquipmentDataBase => _newEquipmentDataBase;
    [Tooltip("プレイヤーが\"現在着用している\"装備の情報")]
    CurrentEquippedDataBase _currentEquippedData = new CurrentEquippedDataBase();
    /// <summary> プレイヤーが "現在着用している" 装備の情報 </summary>
    public CurrentEquippedDataBase CurrentEquippedData => _currentEquippedData;
    [Tooltip("プレイヤーが\"現在所持している\"装備の情報")]
    HaveEquipmentDataBase _haveEquipmentData = new HaveEquipmentDataBase();
    /// <summary> プレイヤーが "現在所持している" 装備の情報 </summary>
    public HaveEquipmentDataBase HaveEquipmentData => _haveEquipmentData;

    /// <summary> 更新を検知する値 </summary>
    bool _isSwap;
    /// <summary> 
    /// 更新を検知する値 : <br/>
    /// 更新をしたフレームでtrueになる。
    /// </summary>
    public bool IsSwap { get => _isSwap; set => _isSwap = value; }

    //===== publicメソッド =====//
    /// <summary>
    /// "所持"している装備と、"着用"している装備を交換する。
    /// </summary>
    /// <param name="fromNowEquipmentID"> 装着する装備のID </param>
    /// <param name="fromNowEquipmentType"> 装着するする装備の種類 </param>
    /// <param name="button"> 基となる「装備」ボタン </param>
    /// <param name="armFlag"> 腕の場合どちらか </param>
    /// 
    public void Swap_HaveToEquipped(EquipmentButton button, int armFlag = Constants.NOT_ARM)
    {
        // 仮の入れ物
        EquipmentID temporary = EquipmentID.None;

        // 現在着用している装備を取得し保存する。
        switch (button._myEquipment._myType)
        {
            case Equipment.EquipmentType.HEAD_PARTS:
                temporary = _currentEquippedData.Equipped._headPartsID;
                break;
            case Equipment.EquipmentType.TORSO_PARTS:
                temporary = _currentEquippedData.Equipped._torsoPartsID;
                break;
            case Equipment.EquipmentType.ARM_PARTS:
                if (armFlag == Constants.LEFT_ARM) 
                    temporary = _currentEquippedData.Equipped._armLeftPartsID;
                else if (armFlag == Constants.RIGHT_ARM) 
                    temporary = _currentEquippedData.Equipped._armRightPartsID;
                else Debug.LogError("不正な値です。");
                break;
            case Equipment.EquipmentType.FOOT_PARTS:
                temporary = _currentEquippedData.Equipped._footPartsID;
                break;
        }
        // 着用する。
        _currentEquippedData.ChangeEquipped(button, armFlag);
        //着脱した装備をインベントリに格納する
        HaveEquipmentData.GetEquipment((int)temporary);

        //ボタンに装備を設定する。
        if (temporary != EquipmentID.None)
            button.Set_EquipmentButton(
                NewEquipmentDataBase.EquipmentData[(int)temporary]);
        else button.Set_EquipmentButton(null);

        //プレイヤーに装備分の上昇ステータスを適用する。
        PlayerStatusManager.Instance.Equipment_RisingValue = _currentEquippedData.GetEquipmentStatus(_newEquipmentDataBase.EquipmentData);

        _isSwap = true;
    }
    /// <summary> 装備を取得する。</summary>
    /// <param name="getEquipmentID"> 取得する装備のID </param>
    /// <returns> 取得に成功したらtrue,失敗したらfalseを返す。 </returns>
    public bool EquippedGet(int getEquipmentID)
    {

        // 範囲内である事をチェックする。
        if (getEquipmentID > (int)EquipmentID.None && getEquipmentID < (int)EquipmentID.ID_END)
        {
            // 装備の取得処理
            for (int i = 0; i < _haveEquipmentData.HaveEquipment._equipments.Length; i++)
            {

                // 空のスロットを見つける
                if (_haveEquipmentData.HaveEquipment._equipments[i] == (int)EquipmentID.None)
                {
                    // その箇所に値を代入する。
                    _haveEquipmentData.HaveEquipment._equipments[i] = getEquipmentID;
                    // 取得メッセージをコンソールに表示する。
                    Debug.Log($"\"{_newEquipmentDataBase.EquipmentData[getEquipmentID]._myName}\"を取得しました。");
                    return true;
                }
            }
        }
        else
        {
            Debug.LogError("不正な値です。修正してください。");
        }
        return false;
    }
    /// <summary> 装備を失う。</summary>
    /// <param name="lostEquipmentID"> 失う装備のID </param>
    /// <returns> 喪失に成功したらtrue,失敗したらfalseを返す。 </returns>
    public bool EquippedLost(int lostEquipmentID)
    {
        //装備の喪失処理
        for (int i = 0; i < _haveEquipmentData.HaveEquipment._equipments.Length; i++)
        {
            if (_haveEquipmentData.HaveEquipment._equipments[i] == lostEquipmentID)
            {
                _haveEquipmentData.HaveEquipment._equipments[i] = -1;
                return true;
            }
        }
        return false;
    }
}
