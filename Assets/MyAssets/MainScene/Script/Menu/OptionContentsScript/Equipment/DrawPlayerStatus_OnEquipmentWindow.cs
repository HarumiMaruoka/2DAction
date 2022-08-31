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
public class DrawPlayerStatus_OnEquipmentWindow : MonoBehaviour
{
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

    const int LEFT_ARM = 0;
    const int RIGHT_ATM = 1;
    const int NOT_ARM = -1;

    /// <summary> このクラスを初期化したかどうか </summary>
    bool _whetherInitialized = false;
    //子オブジェクトを格納する場所
    Text[] _playerStatusText;
    /// <summary> イベントシステム </summary>
    [Header("イベントシステム"), SerializeField] EventSystem _eventSystem;

    //<===== unityメッセージ =====>//
    void Start()
    {
        //初期化していなければ初期化する。
        if (!_whetherInitialized)
        {
            if (!(_whetherInitialized = Initialize_ThisClass()))
            {
                Debug.LogError($"初期化に失敗しました。 : ゲームオブジェクト名 {gameObject.name}");
            }
        }
    }
    void OnEnable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment += SetALL_PlayerStatusText;
    }
    void OnDisable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment -= SetALL_PlayerStatusText;
    }
    void Update()
    {
        //DebugDraw_Get_RiseDifference();
    }

    //<===== privateメンバー関数 =====>//
    /// <summary> このクラスの初期化関数。成功したらtrueを返す。 </summary>
    bool Initialize_ThisClass()
    {
        //テキストを持つ全ての子オブジェクトを取得する。
        _playerStatusText = GetComponentsInChildren<Text>();
        SetALL_PlayerStatusText();
        if (_eventSystem == null) Debug.LogError("イベントシステムをアサインしてください！");

        return true;
    }
    void SetALL_PlayerStatusText()
    {
        var status = PlayerStatusManager.Instance.ConsequentialPlayerStatus;
        var riseDifference = Get_RiseDifference();

        if (_eventSystem.currentSelectedGameObject != null)
        {
            if (_eventSystem.currentSelectedGameObject.TryGetComponent(out EquipmentButton equipment))
            {
                string drawPlus = "";
                //ステータスの変化幅を表示する版の処理
                _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.BaseStatus._playerName;

                drawPlus = riseDifference._maxHp > 0 ? "+" : "";
                if (riseDifference._maxHp == 0) drawPlus = "±";
                 _playerStatusText[(int)StatusName.MaxHP].text = $"最大体力 : {status._maxHp} {drawPlus}{Get_RiseDifference()._maxHp}";

                drawPlus = riseDifference._maxStamina > 0 ? "+" : "";
                if (riseDifference._maxStamina == 0) drawPlus = "±";
                _playerStatusText[(int)StatusName.MaxStamina].text = $"最大スタミナ : {status._maxStamina} {drawPlus}{Get_RiseDifference()._maxStamina}";

                drawPlus = riseDifference._shortRangeAttackPower > 0 ? "+" : "";
                if (riseDifference._shortRangeAttackPower == 0) drawPlus = "±";
                _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = $"近距離攻撃力 : {status._shortRangeAttackPower} {drawPlus}{Get_RiseDifference()._shortRangeAttackPower}";

                drawPlus = riseDifference._longRangeAttackPower > 0 ? "+" : "";
                if (riseDifference._longRangeAttackPower == 0) drawPlus = "±";
                _playerStatusText[(int)StatusName.LongRangeAttackPower].text = $"遠距離攻撃力 : {status._longRangeAttackPower} {drawPlus}{Get_RiseDifference()._longRangeAttackPower}";

                drawPlus = riseDifference._defensePower > 0 ? "+" : "";
                if (riseDifference._defensePower == 0) drawPlus = "±";
                _playerStatusText[(int)StatusName.DefensePower].text = $"防御力 : {status._defensePower} {drawPlus}{Get_RiseDifference()._defensePower}";

                drawPlus = riseDifference._moveSpeed > 0 ? "+" : "";
                if (riseDifference._moveSpeed == 0) drawPlus = "±";
                _playerStatusText[(int)StatusName.MoveSpeed].text = $"移動速度 : {status._moveSpeed} {drawPlus}{Get_RiseDifference()._moveSpeed}";

                drawPlus = riseDifference._difficultToBlowOff > 0 ? "+" : "";
                if (riseDifference._difficultToBlowOff == 0) drawPlus = "±";
                _playerStatusText[(int)StatusName.DifficultToBlowOff].text = $"吹っ飛びにくさ : {status._difficultToBlowOff} {drawPlus}{Get_RiseDifference()._difficultToBlowOff}";
            }
            else
            {
                //ステータスの変化幅を表示しない版の処理
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
        else
        {
            //ステータスの変化幅を表示しない版の処理
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
    PlayerStatusManager.PlayerStatus Get_RiseDifference(int armFrag = NOT_ARM)
    {
        PlayerStatusManager.PlayerStatus result = PlayerStatusManager.PlayerStatus.Zero;
        if (_eventSystem.currentSelectedGameObject != null && _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>() != null)
        {
            //選択中のパーツの種類を取得する。
            Equipment.EquipmentType type = _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._myType;
            //選択中のパーツの種類を基に処理を行う。
            if (type != Equipment.EquipmentType.ARM_PARTS)
            {
                if (type == Equipment.EquipmentType.HEAD_PARTS ||
                    type == Equipment.EquipmentType.TORSO_PARTS ||
                    type == Equipment.EquipmentType.FOOT_PARTS)
                {
                    result = _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment.ThisEquipment_StatusRisingValue;
                    result -= Get_SelectedEquipment(type);
                }
                else
                {
                    Debug.LogError("不正な値です！");
                }
            }
            else
            {
                if (armFrag == LEFT_ARM)
                {
                    result = _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment.ThisEquipment_StatusRisingValue;
                    result -= Get_SelectedEquipment(type, LEFT_ARM);
                }
                else if (armFrag == RIGHT_ATM)
                {
                    result = _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment.ThisEquipment_StatusRisingValue;
                    result -= Get_SelectedEquipment(type, RIGHT_ATM);
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
    PlayerStatusManager.PlayerStatus Get_SelectedEquipment(Equipment.EquipmentType type, int armFrag = NOT_ARM)
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
                if (armFrag == LEFT_ARM)
                {
                    if (EquipmentDataBase.Instance.Equipped._armLeftPartsID != -1)
                        result = EquipmentDataBase.Instance.EquipmentData[EquipmentDataBase.Instance.Equipped._armLeftPartsID].ThisEquipment_StatusRisingValue;
                }
                else if (armFrag == RIGHT_ATM)
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
}
