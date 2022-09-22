using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 選択されているパーツを装備することによる変化量を描画するコンポーネント。
/// </summary>
public class DrawAlteration : UseEventSystemBehavior
{
    //===== フィールド / プロパティ =====//
    /// <summary> 子オブジェクトのテキスト群 </summary>
    Text[] _childrenText;
    Animator _animator;
    //左腕か右腕かを表す
    int _armType;
    string _animName_None = "None";
    string _animName_EquipmentAlterationValue = "EquipmentAlterationValue";

    /// <summary> 描画するかしないかを表す値 </summary>
    bool _isAmountOfChange = false;
    public bool IsAmountOfChange { get => _isAmountOfChange; set => _isAmountOfChange = value; }

    //===== Unityメッセージ =====//
    protected override void Start()
    {
        Init();
        base.Start();
    }
    void Update()
    {
        if (_childrenText[0].color.a < 0.01f)
        {
            ChangeAlterationValue(true);
        }
    }
    void OnEnable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment += Update_AlterationValue;
    }
    void OnDisable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment -= Update_AlterationValue;
    }


    //===== privateメソッド群 =====//
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns>
    /// 初期化に成功したら true ,そうでない場合 false を返す。
    /// </returns>
    bool Init()
    {
        _animator = GetComponent<Animator>();
        _childrenText = transform.GetComponentsInChildren<Text>();
        if (_childrenText == null) return false;
        return true;
    }
    /// <summary> このクラスの更新処理。 </summary>
    void Update_AlterationValue()
    {
        //選択対象が「装備」かどうか判定する。
        if (_eventSystem.currentSelectedGameObject != null)
        {
            ChangeAlterationValue(_eventSystem.currentSelectedGameObject.TryGetComponent(out EquipmentButton equipment));
            if (equipment?._myEquipment._myType == Equipment.EquipmentType.ARM_PARTS)
            {
                _armType = Constants.RIGHT_ARM;
                _animator.Play(_animName_EquipmentAlterationValue);
            }
            else
            {
                _animator.Play(_animName_None);
            }
        }
        else
        {
            ChangeAlterationValue(false);
        }
    }
    /// <summary>  変化量(数値)を描画する。 </summary>
    /// <param name="drawAmountOfChangeFlag"> 
    /// 表示するかどうかを表す真偽値。<br/>
    /// true なら変化量(数値)を表示する。<br/>
    /// false の場合、何も表示しない。<br/>
    /// </param>
    void ChangeAlterationValue(bool drawAmountOfChangeFlag)
    {
        if (drawAmountOfChangeFlag)
        {
            var riseDifference = Get_RiseDifference(_armType);

            //種類
            _childrenText[Constants.EQUIPMENT_TYPE_DRAW_AREA].text =
                Conversion_EquipmentTypeToString
                (
                    _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._myType,
                    _armType
                );

            //体力
            _childrenText[Constants.MAX_HP_DRAW_AREA].text =
                riseDifference._maxHp.ToString("+0;-0;±0");

            //スタミナ
            _childrenText[Constants.MAX_STAMINA_TYPE_DRAW_AREA].text =
                riseDifference._maxStamina.ToString("+0;-0;±0");

            //近距離攻撃力
            _childrenText[Constants.SHORT_RANGE_ATTACK_POWER_DRAW_AREA].text =
                riseDifference._shortRangeAttackPower.ToString("+0;-0;±0");

            //遠距離攻撃
            _childrenText[Constants.LONG_RANGE_ATTACK_POWER_DRAW_AREA].text =
                riseDifference._longRangeAttackPower.ToString("+0;-0;±0");

            //防御力
            _childrenText[Constants.DEFENSE_POWER_DRAW_AREA].text =
                riseDifference._defensePower.ToString("+0;-0;±0");

            //移動速度
            _childrenText[Constants.MOVE_SPEED_DRAW_AREA].text =
                riseDifference._moveSpeed.ToString("+0;-0;±0");

            //吹っ飛びにくさ
            _childrenText[Constants.DIFFICULT_TO_BLOW_OFF_DRAW_AREA].text =
                riseDifference._difficultToBlowOff.ToString("+0;-0;±0");
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
    /// <summary> 
    /// 指定された種類の、着用している装備の、ステータス上昇量を取得する。<br/>
    /// Get_RiseDifference()と連携して使用する。<br/>
    /// </summary>
    /// <param name="type"> 種類 </param>
    /// <param name="armFrag"> 腕以外 右腕 左腕 を判断する値 </param>
    /// <returns> 指定された種類の、着用している装備の、ステータス上昇量 </returns>
    PlayerStatusManager.PlayerStatus Get_SelectedEquipment(Equipment.EquipmentType type, int armFrag = Constants.NOT_ARM)
    {
        PlayerStatusManager.PlayerStatus result = default;
        switch (type)
        {
            //頭パーツの場合の処理
            case Equipment.EquipmentType.HEAD_PARTS:
                if (EquipmentDataBase.Instance.Equipped._headPartsID != -1)
                    result =
                        EquipmentDataBase.Instance.
                        EquipmentData[EquipmentDataBase.Instance.Equipped._headPartsID].
                        ThisEquipment_StatusRisingValue;
                break;
            //胴パーツの場合の処理
            case Equipment.EquipmentType.TORSO_PARTS:
                if (EquipmentDataBase.Instance.Equipped._torsoPartsID != -1)
                    result =
                        EquipmentDataBase.Instance.
                        EquipmentData[EquipmentDataBase.Instance.Equipped._torsoPartsID].
                        ThisEquipment_StatusRisingValue;
                break;
            //足パーツの場合の処理
            case Equipment.EquipmentType.FOOT_PARTS:
                if (EquipmentDataBase.Instance.Equipped._footPartsID != -1)
                    result =
                        EquipmentDataBase.Instance.
                        EquipmentData[EquipmentDataBase.Instance.Equipped._footPartsID].
                        ThisEquipment_StatusRisingValue;
                break;
            //腕パーツの場合の処理
            case Equipment.EquipmentType.ARM_PARTS:
                //左腕の場合の処理
                if (armFrag == Constants.LEFT_ARM)
                {
                    if (EquipmentDataBase.Instance.Equipped._armLeftPartsID != -1)
                        result =
                            EquipmentDataBase.Instance.
                            EquipmentData[EquipmentDataBase.Instance.Equipped._armLeftPartsID].
                            ThisEquipment_StatusRisingValue;
                }
                //右腕の場合の処理
                else if (armFrag == Constants.RIGHT_ARM)
                {
                    if (EquipmentDataBase.Instance.Equipped._armRightPartsID != -1)
                        result =
                            EquipmentDataBase.Instance.
                            EquipmentData[EquipmentDataBase.Instance.Equipped._armRightPartsID].
                            ThisEquipment_StatusRisingValue;
                }
                //エラー値の処理
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
    /// 選択中のパーツを装備する場合のパラメータの差を取得する。<br/>
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
                    _animator.Play(_animName_None);
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
                else if (armFrag == Constants.RIGHT_ARM)
                {
                    result = button._myEquipment.ThisEquipment_StatusRisingValue;
                    result -= Get_SelectedEquipment(type, Constants.RIGHT_ARM);
                }
                else
                {
                    Debug.LogError("不正な値です！");
                }
            }
        }
        return result;
    }
    /// <summary>
    /// "装備の種類" を "文字列" で表したモノに変換する。
    /// </summary>
    /// <param name="type"> 種類 </param>
    /// <param name="armType"> 腕の場合 左腕か右腕かを判定する。 </param>
    /// <returns> 変換後の値を返す。 </returns>
    string Conversion_EquipmentTypeToString(Equipment.EquipmentType type,int armType=Constants.RIGHT_ARM)
    {
        switch (type)
        {
            case Equipment.EquipmentType.HEAD_PARTS: return "頭";
            case Equipment.EquipmentType.TORSO_PARTS: return "胴";
            case Equipment.EquipmentType.ARM_PARTS: 
                if(armType==Constants.RIGHT_ARM)return "右腕の場合";
                else return "左腕の場合";
            case Equipment.EquipmentType.FOOT_PARTS: return "足";
            default: return "";
        }
    }
}
