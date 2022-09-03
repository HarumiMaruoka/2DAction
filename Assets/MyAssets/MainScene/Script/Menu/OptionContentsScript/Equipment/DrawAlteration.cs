using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// �I������Ă���p�[�c�𑕔����邱�Ƃɂ��ω��ʂ�`�悷��R���|�[�l���g�B
/// </summary>
public class DrawAlteration : UseEventSystemBehavior
{
    //<===== �����o�[�ϐ� =====>//
    /// <summary> �q�I�u�W�F�N�g�̃e�L�X�g�Q </summary>
    Text[] _childrenText;

    /// <summary> �`�悷�邩���Ȃ�����\���l </summary>
    bool _isAmountOfChange = false;
    public bool IsAmountOfChange { get => _isAmountOfChange; set => _isAmountOfChange = value; }

    //<===== Unity���b�Z�[�W =====>//
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


    //<===== private�����o�[�֐� =====>//
    bool Initialized()
    {
        _childrenText = transform.GetComponentsInChildren<Text>();
        if (_childrenText == null) return false;
        return true;
    }
    void Update_AlterationValue()
    {
        //�I��Ώۂ��u�����v���ǂ����Ŕ��肷��B
        if (_eventSystem.currentSelectedGameObject != null)
        {
            ChangeAlterationValue(_eventSystem.currentSelectedGameObject.TryGetComponent(out EquipmentButton equipment));
        }
        else
        {
            ChangeAlterationValue(false);
        }
    }
    /// <summary> �ω���(���l)��`�悷��B </summary>
    void ChangeAlterationValue(bool drawAmountOfChangeFlag)
    {
        if (drawAmountOfChangeFlag)
        {
            var riseDifference = Get_RiseDifference();

            //���
            _childrenText[Constants.EQUIPMENT_TYPE_DRAW_AREA].text =
                Conversion_EquipmentTypeToString
                (
                    _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._myType
                );

            //�̗�
            _childrenText[Constants.MAX_HP_DRAW_AREA].text =
                riseDifference._maxHp.ToString("+0;-0;�}0");

            //�X�^�~�i
            _childrenText[Constants.MAX_STAMINA_TYPE_DRAW_AREA].text =
                riseDifference._maxStamina.ToString("+0;-0;�}0");

            //�ߋ����U����
            _childrenText[Constants.SHORT_RANGE_ATTACK_POWER_DRAW_AREA].text =
                riseDifference._shortRangeAttackPower.ToString("+0;-0;�}0");

            //�������U��
            _childrenText[Constants.LONG_RANGE_ATTACK_POWER_DRAW_AREA].text =
                riseDifference._longRangeAttackPower.ToString("+0;-0;�}0");

            //�h���
            _childrenText[Constants.DEFENSE_POWER_DRAW_AREA].text =
                riseDifference._defensePower.ToString("+0;-0;�}0");

            //�ړ����x
            _childrenText[Constants.MOVE_SPEED_DRAW_AREA].text =
                riseDifference._moveSpeed.ToString("+0;-0;�}0");

            //������тɂ���
            _childrenText[Constants.DIFFICULT_TO_BLOW_OFF_DRAW_AREA].text =
                riseDifference._difficultToBlowOff.ToString("+0;-0;�}0");
        }
        //�S�ċ󕶎������
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
    /// <summary> �w�肳�ꂽ��ނ́A���p���Ă��鑕���́A�X�e�[�^�X�㏸�ʂ��擾����B </summary>
    /// <param name="type"> ��� </param>
    /// <param name="armFrag"> �r�ȊO �E�r ���r �𔻒f����l </param>
    /// <returns> �w�肳�ꂽ��ނ́A���p���Ă��鑕���́A�X�e�[�^�X�㏸�� </returns>
    PlayerStatusManager.PlayerStatus Get_SelectedEquipment(Equipment.EquipmentType type, int armFrag = Constants.NOT_ARM)
    {
        PlayerStatusManager.PlayerStatus result = default;
        switch (type)
        {
            //���p�[�c�̏ꍇ�̏���
            case Equipment.EquipmentType.HEAD_PARTS:
                if (EquipmentDataBase.Instance.Equipped._headPartsID != -1)
                    result =
                        EquipmentDataBase.Instance.
                        EquipmentData[EquipmentDataBase.Instance.Equipped._headPartsID].
                        ThisEquipment_StatusRisingValue;
                break;
            //���p�[�c�̏ꍇ�̏���
            case Equipment.EquipmentType.TORSO_PARTS:
                if (EquipmentDataBase.Instance.Equipped._torsoPartsID != -1)
                    result =
                        EquipmentDataBase.Instance.
                        EquipmentData[EquipmentDataBase.Instance.Equipped._torsoPartsID].
                        ThisEquipment_StatusRisingValue;
                break;
            //���p�[�c�̏ꍇ�̏���
            case Equipment.EquipmentType.FOOT_PARTS:
                if (EquipmentDataBase.Instance.Equipped._footPartsID != -1)
                    result =
                        EquipmentDataBase.Instance.
                        EquipmentData[EquipmentDataBase.Instance.Equipped._footPartsID].
                        ThisEquipment_StatusRisingValue;
                break;
            //�r�p�[�c�̏ꍇ�̏���
            case Equipment.EquipmentType.ARM_PARTS:
                //���r�̏ꍇ�̏���
                if (armFrag == Constants.LEFT_ARM)
                {
                    if (EquipmentDataBase.Instance.Equipped._armLeftPartsID != -1)
                        result =
                            EquipmentDataBase.Instance.
                            EquipmentData[EquipmentDataBase.Instance.Equipped._armLeftPartsID].
                            ThisEquipment_StatusRisingValue;
                }
                //�E�r�̏ꍇ�̏���
                else if (armFrag == Constants.RIGHT_ATM)
                {
                    if (EquipmentDataBase.Instance.Equipped._armRightPartsID != -1)
                        result =
                            EquipmentDataBase.Instance.
                            EquipmentData[EquipmentDataBase.Instance.Equipped._armRightPartsID].
                            ThisEquipment_StatusRisingValue;
                }
                //�G���[�l�̏���
                else
                {
                    Debug.LogError("�s���Ȓl�ł��I");
                }
                break;
            default: Debug.LogError("�s���Ȓl�ł��I"); break;
        }
        return result;
    }
    /// <summary>
    /// �I�𒆂̃p�[�c�𑕔�����ꍇ�̃p�����[�^�̍����擾����B
    /// �r�̏ꍇ�̏������܂����L���Ȃ̂ŏC�����K�v�ł��B
    /// </summary>
    /// <param name="armFrag"> �r�ȊO�A�E�r�A���r �𔻒f����l </param>
    /// <returns> �p�����[�^�̍� </returns>
    PlayerStatusManager.PlayerStatus Get_RiseDifference(int armFrag = Constants.NOT_ARM)
    {
        PlayerStatusManager.PlayerStatus result = PlayerStatusManager.PlayerStatus.Zero;
        if (_eventSystem.currentSelectedGameObject != null && _eventSystem.currentSelectedGameObject.TryGetComponent(out EquipmentButton button))
        {
            //�I�𒆂̃p�[�c�̎�ނ��擾����B
            Equipment.EquipmentType type = button._myEquipment._myType;
            //�I�𒆂̃p�[�c�̎�ނ���ɏ������s���B
            //�� �� �� �̏ꍇ�̏����B
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
                    Debug.LogError("�s���Ȓl�ł��I");
                }
            }
            //�r�̏ꍇ�̏����B
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
                    Debug.LogError("�s���Ȓl�ł��I");
                }
            }
        }
        return result;
    }

    string Conversion_EquipmentTypeToString(Equipment.EquipmentType type)
    {
        switch (type)
        {
            case Equipment.EquipmentType.HEAD_PARTS: return "���p�[�c";
            case Equipment.EquipmentType.TORSO_PARTS: return "���p�[�c";
            case Equipment.EquipmentType.ARM_PARTS: return "�r�p�[�c";
            case Equipment.EquipmentType.FOOT_PARTS: return "���p�[�c";
            default: return "";
        }
    }
}
