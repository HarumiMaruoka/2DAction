using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary> 
/// 装備画面に、プレイヤーステータスを描画するコンポーネント。
/// 現在選択中のパーツを装備することによるステータスの変化量を描画する機能を追記してください。
/// </summary>
public class DrawPlayerStatus_OnEquipmentWindow : UseEventSystemBehavior
{
    /// <summary> ステータステキストのインデックスを表すenum。 </summary>
    enum StatusName
    {
        /// <summary> 名前 </summary>
        PlayerName,
        /// <summary> 最大体力 </summary>
        MaxHP,
        /// <summary> 最大スタミナ </summary>
        MaxStamina,
        /// <summary> 近距離攻撃力 </summary>
        ShortRangeAttackPower,
        /// <summary> 遠距離攻撃力 </summary>
        LongRangeAttackPower,
        /// <summary> 防御力 </summary>
        DefensePower,
        /// <summary> 移動速度 </summary>
        MoveSpeed,
        /// <summary> 吹っ飛びにくさ </summary>
        DifficultToBlowOff
    }

    /// <summary> このクラスを初期化したかどうか </summary>
    bool _whetherInitialized = false;
    /// <summary> 子オブジェクトが持つ、プレイヤーステータスを表示するテキストコンポーネントの配列 </summary>
    Text[] _playerStatusText;

    //<===== unityメッセージ =====>//
    void Start()
    {
        _whetherInitialized = Initialize_ThisClass();
        //初期化する
        if (_whetherInitialized)
        {
            Debug.Log($"{gameObject.name}の初期化に成功しました。 ");
        }
    }
    void OnEnable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment += UpdateALL_PlayerStatusText;
    }
    void OnDisable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment -= UpdateALL_PlayerStatusText;
    }
    void Update()
    {

    }

    //<===== privateメンバー関数 =====>//
    /// <summary> このクラスの初期化関数。成功したらtrueを返す。 </summary>
    bool Initialize_ThisClass()
    {
        //基底クラスを初期化
        base.Initialized_UseEventSystemBehavior();
        //テキストを持つ全ての子オブジェクトを取得する。
        _playerStatusText = GetComponentsInChildren<Text>();
        UpdateALL_PlayerStatusText();

        return true;
    }
    /// <summary> プレイヤーステータスの表示を更新する。 </summary>
    void UpdateALL_PlayerStatusText()
    {
        if (_eventSystem.currentSelectedGameObject != null)
        {
            if (_eventSystem.currentSelectedGameObject.TryGetComponent(out EquipmentButton equipment))
            {
                //ステータスの変化幅を表示する版の処理
                Update_StatusText(Constants.DRAW_AMPLITUDE);
            }
            else
            {
                //ステータスの変化幅を表示しない版の処理
                Update_StatusText(Constants.NOT_DRAW_AMPLITUDE);
            }
        }
        else
        {
            ////ステータスの変化幅を表示しない版の処理
            Update_StatusText(Constants.NOT_DRAW_AMPLITUDE);
        }
    }
    /// <summary> プレイヤーステータステキストを更新する処理 </summary>
    /// <param name="isDrawAmountOfChange"> 変化幅を表示するかどうか : true なら表示する。 </param>
    void Update_StatusText(bool drawAmountOfChangeFlag)
    {
        var status = PlayerStatusManager.Instance.ConsequentialPlayerStatus;
        var riseDifference = Get_RiseDifference();
        if (drawAmountOfChangeFlag)
        {
            string drawPlus = "";

            _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.BaseStatus._playerName;

            drawPlus = CcompareWithZero(riseDifference._maxHp);
            _playerStatusText[(int)StatusName.MaxHP].text = $"最大体力 : {status._maxHp} {drawPlus}{Get_RiseDifference()._maxHp}";

            drawPlus = CcompareWithZero(riseDifference._maxStamina);
            _playerStatusText[(int)StatusName.MaxStamina].text = $"最大スタミナ : {status._maxStamina} {drawPlus}{Get_RiseDifference()._maxStamina}";

            drawPlus = CcompareWithZero(riseDifference._shortRangeAttackPower);
            _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = $"近距離攻撃力 : {status._shortRangeAttackPower} {drawPlus}{Get_RiseDifference()._shortRangeAttackPower}";
            
            drawPlus = CcompareWithZero(riseDifference._longRangeAttackPower);
            _playerStatusText[(int)StatusName.LongRangeAttackPower].text = $"遠距離攻撃力 : {status._longRangeAttackPower} {drawPlus}{Get_RiseDifference()._longRangeAttackPower}";

            drawPlus = CcompareWithZero(riseDifference._defensePower);
            _playerStatusText[(int)StatusName.DefensePower].text = $"防御力 : {status._defensePower} {drawPlus}{Get_RiseDifference()._defensePower}";

            drawPlus = CcompareWithZero(riseDifference._moveSpeed);
            _playerStatusText[(int)StatusName.MoveSpeed].text = $"移動速度 : {status._moveSpeed} {drawPlus}{Get_RiseDifference()._moveSpeed}";

            drawPlus = CcompareWithZero(riseDifference._difficultToBlowOff);
            _playerStatusText[(int)StatusName.DifficultToBlowOff].text = $"吹っ飛びにくさ : {status._difficultToBlowOff} {drawPlus}{Get_RiseDifference()._difficultToBlowOff}";
        }
        else
        {
            _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.BaseStatus._playerName;

            _playerStatusText[(int)StatusName.MaxHP].text = $"最大体力 : {status._maxHp}";

            _playerStatusText[(int)StatusName.MaxStamina].text = $"最大スタミナ : {status._maxStamina}";

            _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = $"近距離攻撃力 : {status._shortRangeAttackPower}";

            _playerStatusText[(int)StatusName.LongRangeAttackPower].text = $"遠距離攻撃力 : {status._longRangeAttackPower}";

            _playerStatusText[(int)StatusName.DefensePower].text = $"防御力 : {status._defensePower}";

            _playerStatusText[(int)StatusName.MoveSpeed].text = $"移動速度 : {status._moveSpeed}";

            _playerStatusText[(int)StatusName.DifficultToBlowOff].text = $"吹っ飛びにくさ : {status._difficultToBlowOff}";
        }
    }
    /// <summary> 選択中のパーツを装備した場合のパラメータの変化量を取得する。 </summary>
    /// <returns> 変化量 </returns>
    PlayerStatusManager.PlayerStatus Get_RiseDifference(int armFrag = Constants.NOT_ARM)
    {
        PlayerStatusManager.PlayerStatus result = PlayerStatusManager.PlayerStatus.Zero;
        if (_eventSystem.currentSelectedGameObject != null && _eventSystem.currentSelectedGameObject.TryGetComponent(out EquipmentButton button))
        {
            //選択中のパーツの種類を取得する。
            Equipment.EquipmentType type = button._myEquipment._myType;
            //選択中のパーツの種類を基に処理を行う。
            if (type != Equipment.EquipmentType.ARM_PARTS)
            {
                if (type == Equipment.EquipmentType.HEAD_PARTS ||
                    type == Equipment.EquipmentType.TORSO_PARTS ||
                    type == Equipment.EquipmentType.FOOT_PARTS)
                {
                    result = button._myEquipment.ThisEquipment_StatusRisingValue;
                    result -= Get_SelectedEquipment(type);
                }
                else
                {
                    Debug.LogError("不正な値です！");
                }
            }
            else
            {
                if (armFrag == Constants.LEFT_ARM)
                {
                    result = button._myEquipment.ThisEquipment_StatusRisingValue;
                    result -= Get_SelectedEquipment(type, Constants.LEFT_ARM);
                }
                else if (armFrag == Constants.RIGHT_ATM)
                {
                    result = button._myEquipment.ThisEquipment_StatusRisingValue;
                    result -= Get_SelectedEquipment(type, Constants.RIGHT_ATM);
                }
                else
                {
                    Debug.LogError("不正な値です！");
                }
            }
        }

        return result;
    }
    /// <summary> 指定された種類の、着用している装備の、ステータス上昇量を取得する。 </summary>
    /// <param name="type"> 種類 </param>
    /// <param name="armFrag"> アームフラグ </param>
    /// <returns></returns>
    PlayerStatusManager.PlayerStatus Get_SelectedEquipment(Equipment.EquipmentType type, int armFrag = Constants.NOT_ARM)
    {
        PlayerStatusManager.PlayerStatus result = default;
        switch (type)
        {
            case Equipment.EquipmentType.HEAD_PARTS:
                if (EquipmentDataBase.Instance.Equipped._headPartsID != -1)
                    result = EquipmentDataBase.Instance.EquipmentData[EquipmentDataBase.Instance.Equipped._headPartsID].ThisEquipment_StatusRisingValue;
                break;
            case Equipment.EquipmentType.TORSO_PARTS:
                if (EquipmentDataBase.Instance.Equipped._torsoPartsID != -1)
                    result = EquipmentDataBase.Instance.EquipmentData[EquipmentDataBase.Instance.Equipped._torsoPartsID].ThisEquipment_StatusRisingValue;
                break;
            case Equipment.EquipmentType.FOOT_PARTS:
                if (EquipmentDataBase.Instance.Equipped._footPartsID != -1)
                    result = EquipmentDataBase.Instance.EquipmentData[EquipmentDataBase.Instance.Equipped._footPartsID].ThisEquipment_StatusRisingValue;
                break;
            case Equipment.EquipmentType.ARM_PARTS:
                if (armFrag == Constants.LEFT_ARM)
                {
                    if (EquipmentDataBase.Instance.Equipped._armLeftPartsID != -1)
                        result = EquipmentDataBase.Instance.EquipmentData[EquipmentDataBase.Instance.Equipped._armLeftPartsID].ThisEquipment_StatusRisingValue;
                }
                else if (armFrag == Constants.RIGHT_ATM)
                {
                    if (EquipmentDataBase.Instance.Equipped._armRightPartsID != -1)
                        result = EquipmentDataBase.Instance.EquipmentData[EquipmentDataBase.Instance.Equipped._armRightPartsID].ThisEquipment_StatusRisingValue;
                }
                else
                {
                    Debug.LogError("不正な値です！");
                }
                break;
            default: Debug.LogError("不正な値です！"); break;
        }
        return result;
    }

    /// <summary> Debug用処理。コンソールにステータス変化幅を表示する。 </summary>
    void DebugDraw_Get_RiseDifference()
    {
        Debug.Log(
            $"体力 : {Get_RiseDifference()._maxHp}" +
            $"スタミナ : {Get_RiseDifference()._maxStamina}" +
            $"移動速度 : {Get_RiseDifference()._moveSpeed}" +
            $"近距離攻撃力 : {Get_RiseDifference()._shortRangeAttackPower}" +
            $"遠距離攻撃力 : {Get_RiseDifference()._longRangeAttackPower}" +
            $"防御力 : {Get_RiseDifference()._defensePower}" +
            $"吹っ飛びにくさ : {Get_RiseDifference()._difficultToBlowOff}"
            );
    }

    string CcompareWithZero(float target)
    {
        if (Mathf.Approximately(target, 0f)) return "±";
        else if (target > 0) return "+";
        else return "";
    }
}
