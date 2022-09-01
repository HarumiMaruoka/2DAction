using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary> 
/// ������ʂɁA�v���C���[�X�e�[�^�X��`�悷��R���|�[�l���g�B
/// ���ݑI�𒆂̃p�[�c�𑕔����邱�Ƃɂ��X�e�[�^�X�̕ω��ʂ�`�悷��@�\��ǋL���Ă��������B
/// </summary>
public class DrawPlayerStatus_OnEquipmentWindow : UseEventSystemBehavior
{
    /// <summary> �X�e�[�^�X�e�L�X�g�̃C���f�b�N�X��\��enum�B </summary>
    enum StatusName
    {
        /// <summary> ���O </summary>
        PlayerName,
        /// <summary> �ő�̗� </summary>
        MaxHP,
        /// <summary> �ő�X�^�~�i </summary>
        MaxStamina,
        /// <summary> �ߋ����U���� </summary>
        ShortRangeAttackPower,
        /// <summary> �������U���� </summary>
        LongRangeAttackPower,
        /// <summary> �h��� </summary>
        DefensePower,
        /// <summary> �ړ����x </summary>
        MoveSpeed,
        /// <summary> ������тɂ��� </summary>
        DifficultToBlowOff
    }

    /// <summary> ���̃N���X���������������ǂ��� </summary>
    bool _whetherInitialized = false;
    /// <summary> �q�I�u�W�F�N�g�����A�v���C���[�X�e�[�^�X��\������e�L�X�g�R���|�[�l���g�̔z�� </summary>
    Text[] _playerStatusText;

    //<===== unity���b�Z�[�W =====>//
    void Start()
    {
        _whetherInitialized = Initialize_ThisClass();
        //����������
        if (_whetherInitialized)
        {
            Debug.Log($"{gameObject.name}�̏������ɐ������܂����B ");
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

    //<===== private�����o�[�֐� =====>//
    /// <summary> ���̃N���X�̏������֐��B����������true��Ԃ��B </summary>
    bool Initialize_ThisClass()
    {
        //���N���X��������
        base.Initialized_UseEventSystemBehavior();
        //�e�L�X�g�����S�Ă̎q�I�u�W�F�N�g���擾����B
        _playerStatusText = GetComponentsInChildren<Text>();
        UpdateALL_PlayerStatusText();

        return true;
    }
    /// <summary> �v���C���[�X�e�[�^�X�̕\�����X�V����B </summary>
    void UpdateALL_PlayerStatusText()
    {
        if (_eventSystem.currentSelectedGameObject != null)
        {
            if (_eventSystem.currentSelectedGameObject.TryGetComponent(out EquipmentButton equipment))
            {
                //�X�e�[�^�X�̕ω�����\������ł̏���
                Update_StatusText(Constants.DRAW_AMPLITUDE);
            }
            else
            {
                //�X�e�[�^�X�̕ω�����\�����Ȃ��ł̏���
                Update_StatusText(Constants.NOT_DRAW_AMPLITUDE);
            }
        }
        else
        {
            ////�X�e�[�^�X�̕ω�����\�����Ȃ��ł̏���
            Update_StatusText(Constants.NOT_DRAW_AMPLITUDE);
        }
    }
    /// <summary> �v���C���[�X�e�[�^�X�e�L�X�g���X�V���鏈�� </summary>
    /// <param name="isDrawAmountOfChange"> �ω�����\�����邩�ǂ��� : true �Ȃ�\������B </param>
    void Update_StatusText(bool drawAmountOfChangeFlag)
    {
        var status = PlayerStatusManager.Instance.ConsequentialPlayerStatus;
        var riseDifference = Get_RiseDifference();
        if (drawAmountOfChangeFlag)
        {
            string drawPlus = "";

            _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.BaseStatus._playerName;

            drawPlus = CcompareWithZero(riseDifference._maxHp);
            _playerStatusText[(int)StatusName.MaxHP].text = $"�ő�̗� : {status._maxHp} {drawPlus}{Get_RiseDifference()._maxHp}";

            drawPlus = CcompareWithZero(riseDifference._maxStamina);
            _playerStatusText[(int)StatusName.MaxStamina].text = $"�ő�X�^�~�i : {status._maxStamina} {drawPlus}{Get_RiseDifference()._maxStamina}";

            drawPlus = CcompareWithZero(riseDifference._shortRangeAttackPower);
            _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = $"�ߋ����U���� : {status._shortRangeAttackPower} {drawPlus}{Get_RiseDifference()._shortRangeAttackPower}";
            
            drawPlus = CcompareWithZero(riseDifference._longRangeAttackPower);
            _playerStatusText[(int)StatusName.LongRangeAttackPower].text = $"�������U���� : {status._longRangeAttackPower} {drawPlus}{Get_RiseDifference()._longRangeAttackPower}";

            drawPlus = CcompareWithZero(riseDifference._defensePower);
            _playerStatusText[(int)StatusName.DefensePower].text = $"�h��� : {status._defensePower} {drawPlus}{Get_RiseDifference()._defensePower}";

            drawPlus = CcompareWithZero(riseDifference._moveSpeed);
            _playerStatusText[(int)StatusName.MoveSpeed].text = $"�ړ����x : {status._moveSpeed} {drawPlus}{Get_RiseDifference()._moveSpeed}";

            drawPlus = CcompareWithZero(riseDifference._difficultToBlowOff);
            _playerStatusText[(int)StatusName.DifficultToBlowOff].text = $"������тɂ��� : {status._difficultToBlowOff} {drawPlus}{Get_RiseDifference()._difficultToBlowOff}";
        }
        else
        {
            _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.BaseStatus._playerName;

            _playerStatusText[(int)StatusName.MaxHP].text = $"�ő�̗� : {status._maxHp}";

            _playerStatusText[(int)StatusName.MaxStamina].text = $"�ő�X�^�~�i : {status._maxStamina}";

            _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = $"�ߋ����U���� : {status._shortRangeAttackPower}";

            _playerStatusText[(int)StatusName.LongRangeAttackPower].text = $"�������U���� : {status._longRangeAttackPower}";

            _playerStatusText[(int)StatusName.DefensePower].text = $"�h��� : {status._defensePower}";

            _playerStatusText[(int)StatusName.MoveSpeed].text = $"�ړ����x : {status._moveSpeed}";

            _playerStatusText[(int)StatusName.DifficultToBlowOff].text = $"������тɂ��� : {status._difficultToBlowOff}";
        }
    }
    /// <summary> �I�𒆂̃p�[�c�𑕔������ꍇ�̃p�����[�^�̕ω��ʂ��擾����B </summary>
    /// <returns> �ω��� </returns>
    PlayerStatusManager.PlayerStatus Get_RiseDifference(int armFrag = Constants.NOT_ARM)
    {
        PlayerStatusManager.PlayerStatus result = PlayerStatusManager.PlayerStatus.Zero;
        if (_eventSystem.currentSelectedGameObject != null && _eventSystem.currentSelectedGameObject.TryGetComponent(out EquipmentButton button))
        {
            //�I�𒆂̃p�[�c�̎�ނ��擾����B
            Equipment.EquipmentType type = button._myEquipment._myType;
            //�I�𒆂̃p�[�c�̎�ނ���ɏ������s���B
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
    /// <summary> �w�肳�ꂽ��ނ́A���p���Ă��鑕���́A�X�e�[�^�X�㏸�ʂ��擾����B </summary>
    /// <param name="type"> ��� </param>
    /// <param name="armFrag"> �A�[���t���O </param>
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
                    Debug.LogError("�s���Ȓl�ł��I");
                }
                break;
            default: Debug.LogError("�s���Ȓl�ł��I"); break;
        }
        return result;
    }

    /// <summary> Debug�p�����B�R���\�[���ɃX�e�[�^�X�ω�����\������B </summary>
    void DebugDraw_Get_RiseDifference()
    {
        Debug.Log(
            $"�̗� : {Get_RiseDifference()._maxHp}" +
            $"�X�^�~�i : {Get_RiseDifference()._maxStamina}" +
            $"�ړ����x : {Get_RiseDifference()._moveSpeed}" +
            $"�ߋ����U���� : {Get_RiseDifference()._shortRangeAttackPower}" +
            $"�������U���� : {Get_RiseDifference()._longRangeAttackPower}" +
            $"�h��� : {Get_RiseDifference()._defensePower}" +
            $"������тɂ��� : {Get_RiseDifference()._difficultToBlowOff}"
            );
    }

    string CcompareWithZero(float target)
    {
        if (Mathf.Approximately(target, 0f)) return "�}";
        else if (target > 0) return "+";
        else return "";
    }
}
