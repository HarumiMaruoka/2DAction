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
public class DrawPlayerStatus_OnEquipmentWindow : MonoBehaviour
{
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

    const int LEFT_ARM = 0;
    const int RIGHT_ATM = 1;
    const int NOT_ARM = -1;

    /// <summary> ���̃N���X���������������ǂ��� </summary>
    bool _whetherInitialized = false;
    //�q�I�u�W�F�N�g���i�[����ꏊ
    Text[] _playerStatusText;
    /// <summary> �C�x���g�V�X�e�� </summary>
    [Header("�C�x���g�V�X�e��"), SerializeField] EventSystem _eventSystem;

    //<===== unity���b�Z�[�W =====>//
    void Start()
    {
        //���������Ă��Ȃ���Ώ���������B
        if (!_whetherInitialized)
        {
            if (!(_whetherInitialized = Initialize_ThisClass()))
            {
                Debug.LogError($"�������Ɏ��s���܂����B : �Q�[���I�u�W�F�N�g�� {gameObject.name}");
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

    //<===== private�����o�[�֐� =====>//
    /// <summary> ���̃N���X�̏������֐��B����������true��Ԃ��B </summary>
    bool Initialize_ThisClass()
    {
        //�e�L�X�g�����S�Ă̎q�I�u�W�F�N�g���擾����B
        _playerStatusText = GetComponentsInChildren<Text>();
        SetALL_PlayerStatusText();
        if (_eventSystem == null) Debug.LogError("�C�x���g�V�X�e�����A�T�C�����Ă��������I");

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
                //�X�e�[�^�X�̕ω�����\������ł̏���
                _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.BaseStatus._playerName;

                drawPlus = riseDifference._maxHp > 0 ? "+" : "";
                if (riseDifference._maxHp == 0) drawPlus = "�}";
                 _playerStatusText[(int)StatusName.MaxHP].text = $"�ő�̗� : {status._maxHp} {drawPlus}{Get_RiseDifference()._maxHp}";

                drawPlus = riseDifference._maxStamina > 0 ? "+" : "";
                if (riseDifference._maxStamina == 0) drawPlus = "�}";
                _playerStatusText[(int)StatusName.MaxStamina].text = $"�ő�X�^�~�i : {status._maxStamina} {drawPlus}{Get_RiseDifference()._maxStamina}";

                drawPlus = riseDifference._shortRangeAttackPower > 0 ? "+" : "";
                if (riseDifference._shortRangeAttackPower == 0) drawPlus = "�}";
                _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = $"�ߋ����U���� : {status._shortRangeAttackPower} {drawPlus}{Get_RiseDifference()._shortRangeAttackPower}";

                drawPlus = riseDifference._longRangeAttackPower > 0 ? "+" : "";
                if (riseDifference._longRangeAttackPower == 0) drawPlus = "�}";
                _playerStatusText[(int)StatusName.LongRangeAttackPower].text = $"�������U���� : {status._longRangeAttackPower} {drawPlus}{Get_RiseDifference()._longRangeAttackPower}";

                drawPlus = riseDifference._defensePower > 0 ? "+" : "";
                if (riseDifference._defensePower == 0) drawPlus = "�}";
                _playerStatusText[(int)StatusName.DefensePower].text = $"�h��� : {status._defensePower} {drawPlus}{Get_RiseDifference()._defensePower}";

                drawPlus = riseDifference._moveSpeed > 0 ? "+" : "";
                if (riseDifference._moveSpeed == 0) drawPlus = "�}";
                _playerStatusText[(int)StatusName.MoveSpeed].text = $"�ړ����x : {status._moveSpeed} {drawPlus}{Get_RiseDifference()._moveSpeed}";

                drawPlus = riseDifference._difficultToBlowOff > 0 ? "+" : "";
                if (riseDifference._difficultToBlowOff == 0) drawPlus = "�}";
                _playerStatusText[(int)StatusName.DifficultToBlowOff].text = $"������тɂ��� : {status._difficultToBlowOff} {drawPlus}{Get_RiseDifference()._difficultToBlowOff}";
            }
            else
            {
                //�X�e�[�^�X�̕ω�����\�����Ȃ��ł̏���
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
        else
        {
            //�X�e�[�^�X�̕ω�����\�����Ȃ��ł̏���
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
    PlayerStatusManager.PlayerStatus Get_RiseDifference(int armFrag = NOT_ARM)
    {
        PlayerStatusManager.PlayerStatus result = PlayerStatusManager.PlayerStatus.Zero;
        if (_eventSystem.currentSelectedGameObject != null && _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>() != null)
        {
            //�I�𒆂̃p�[�c�̎�ނ��擾����B
            Equipment.EquipmentType type = _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._myType;
            //�I�𒆂̃p�[�c�̎�ނ���ɏ������s���B
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
                    Debug.LogError("�s���Ȓl�ł��I");
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
                    Debug.LogError("�s���Ȓl�ł��I");
                }
                break;
            default: Debug.LogError("�s���Ȓl�ł��I"); break;
        }
        return result;
    }

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
}
