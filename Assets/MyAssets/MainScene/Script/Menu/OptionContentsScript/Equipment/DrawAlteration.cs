using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 選択されているパーツを装備することによる変化量を描画するコンポーネント。
/// </summary>
public class DrawAlteration : UseEventSystemBehavior
{
    //<===== メンバー変数 =====>//
    /// <summary> 子オブジェクトのテキスト群 </summary>
    Text[] _childrenText;

    /// <summary> 描画するかしないかを表す値 </summary>
    bool _isAmountOfChange = false;
    public bool IsAmountOfChange { get => _isAmountOfChange; set => _isAmountOfChange = value; }

    //<===== Unityメッセージ =====>//
    void Awake()
    {
        base.Initialized_UseEventSystemBehavior();
        Initialized();
        Update_AlterationValue();
    }
    void Update()
    {

    }
    void OnEnable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment += Update_AlterationValue;
    }
    void OnDisable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment -= Update_AlterationValue;
    }


    //<===== privateメンバー関数 =====>//
    bool Initialized()
    {
        _childrenText = transform.GetComponentsInChildren<Text>();
        if (_childrenText == null) return false;
        return true;
    }
    void Update_AlterationValue()
    {
        //選択対象が「装備」かどうかで判定する。
        if (_eventSystem.currentSelectedGameObject != null)
        {
            ChangeAlterationValue(_eventSystem.currentSelectedGameObject.TryGetComponent(out EquipmentButton equipment));
        }
        else
        {
            ChangeAlterationValue(false);
        }
    }
    /// <summary> 変化量(数値)を描画する。 </summary>
    void ChangeAlterationValue(bool drawAmountOfChangeFlag)
    {
        Debug.Log($"fafafa{drawAmountOfChangeFlag}");
        if (drawAmountOfChangeFlag)
        {
            var riseDifference = Get_RiseDifference();

            //種類
            _childrenText[Constants.EQUIPMENT_TYPE_DRAW_AREA].text =
                PlayerStatusManager.Instance.BaseStatus._playerName;

            //体力
            _childrenText[Constants.MAX_HP_DRAW_AREA].text =
                $"{CcompareWithZero(riseDifference._maxHp)}";

            //スタミナ
            _childrenText[Constants.MAX_STAMINA_TYPE_DRAW_AREA].text =
                $"{CcompareWithZero(riseDifference._maxStamina)}";

            //近距離攻撃力
            _childrenText[Constants.SHORT_RANGE_ATTACK_POWER_DRAW_AREA].text =
                $"{CcompareWithZero(riseDifference._shortRangeAttackPower)}";

            //遠距離攻撃
            _childrenText[Constants.LONG_RANGE_ATTACK_POWER_DRAW_AREA].text =
                $"{CcompareWithZero(riseDifference._longRangeAttackPower)}";

            //防御力
            _childrenText[Constants.DEFENSE_POWER_DRAW_AREA].text =
                $"{CcompareWithZero(riseDifference._defensePower)}";

            //移動速度
            _childrenText[Constants.MOVE_SPEED_DRAW_AREA].text =
                $"{CcompareWithZero(riseDifference._moveSpeed)}";

            //吹っ飛びにくさ
            _childrenText[Constants.DIFFICULT_TO_BLOW_OFF_DRAW_AREA].text =
                $"{CcompareWithZero(riseDifference._difficultToBlowOff)}";
        }
        //全て空文字列を代入
        else
        {
            _childrenText[Constants.EQUIPMENT_TYPE_DRAW_AREA].text = "";
            _childrenText[Constants.MAX_HP_DRAW_AREA].text = "";
            _childrenText[Constants.MAX_STAMINA_TYPE_DRAW_AREA].text = "";
            _childrenText[Constants.SHORT_RANGE_ATTACK_POWER_DRAW_AREA].text = "";
            _childrenText[Constants.LONG_RANGE_ATTACK_POWER_DRAW_AREA].text = "";
            _childrenText[Constants.DEFENSE_POWER_DRAW_AREA].text = "";
            _childrenText[Constants.MOVE_SPEED_DRAW_AREA].text = "";
            _childrenText[Constants.DIFFICULT_TO_BLOW_OFF_DRAW_AREA].text = "";
        }
    }
    /// <summary> 指定された種類の、着用している装備の、ステータス上昇量を取得する。 </summary>
    /// <param name="type"> 種類 </param>
    /// <param name="armFrag"> 腕以外 右腕 左腕 を判断する値 </param>
    /// <returns> 指定された種類の、着用している装備の、ステータス上昇量 </returns>
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
    /// <summary>
    /// 選択中のパーツを装備する場合のパラメータの差を取得する。
    /// 腕の場合の処理がまだ未記入なので修正が必要です。
    /// </summary>
    /// <param name="armFrag"> 腕以外、右腕、左腕 を判断する値 </param>
    /// <returns> パラメータの差 </returns>
    PlayerStatusManager.PlayerStatus Get_RiseDifference(int armFrag = Constants.NOT_ARM)
    {
        PlayerStatusManager.PlayerStatus result = PlayerStatusManager.PlayerStatus.Zero;
        if (_eventSystem.currentSelectedGameObject != null && _eventSystem.currentSelectedGameObject.TryGetComponent(out EquipmentButton button))
        {
            //選択中のパーツの種類を取得する。
            Equipment.EquipmentType type = button._myEquipment._myType;
            //選択中のパーツの種類を基に処理を行う。
            //頭 胴 足 の場合の処理。
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
            //腕の場合の処理。
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
    string CcompareWithZero(float target)
    {
        if (Mathf.Approximately(target, 0f)) return $"±{target}";
        else if (target > 0) return $"+{target}";
        else return $"{target}";
    }
}
