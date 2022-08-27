using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    /// <summary> ���̃N���X���������������ǂ��� </summary>
    bool _whetherInitialized = false;

    //�q�I�u�W�F�N�g���i�[����ꏊ
    Text[] _playerStatusText;

    //<===== unity���b�Z�[�W =====>//
    void Start()
    {
        //���������Ă��Ȃ���Ώ���������B
        if (!_whetherInitialized)
        {
            if(!(_whetherInitialized = Initialize_ThisClass()))
            {
                Debug.LogError($"�������Ɏ��s���܂����B : �Q�[���I�u�W�F�N�g�� {gameObject.name}");
            }
        }
    }
    private void OnEnable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment += SetALL_PlayerStatusText;
    }
    private void OnDisable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment -= SetALL_PlayerStatusText;
    }

    //<===== private�����o�[�֐� =====>//
    /// <summary> ���̃N���X�̏������֐��B����������true��Ԃ��B </summary>
    bool Initialize_ThisClass()
    {
        //�e�L�X�g�����S�Ă̎q�I�u�W�F�N�g���擾����B
        _playerStatusText = GetComponentsInChildren<Text>();
        SetALL_PlayerStatusText();

        return true;
    }

    /// <summary> �v���C���[�̃X�e�[�^�X���A�e�L�X�g�ɐݒ肷��B </summary>
    void SetALL_PlayerStatusText()
    {
        //�v���C���[�̖��O��ݒ肷��
        _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.BaseStatus._playerName;

        //�J��Ԃ��g�p����̂ŕϐ��ɕۑ��B
        var status = PlayerStatusManager.Instance.ConsequentialPlayerStatus;
        //�v���C���[�̍ő�̗͂�ݒ肷��
        _playerStatusText[(int)StatusName.MaxHP].text =
            $"�ő�̗� : {status._maxHp}";
        //�v���C���[�̍ő�X�^�~�i��ݒ肷��
        _playerStatusText[(int)StatusName.MaxStamina].text =
            $"�ő�X�^�~�i : {status._maxStamina}";
        //�v���C���[�̋ߋ����U���͂�ݒ肷��
        _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = 
            $"�ߋ����U���� : {status._shortRangeAttackPower}";
        //�v���C���[�̉������U���͂�ݒ肷��
        _playerStatusText[(int)StatusName.LongRangeAttackPower].text =
            $"�������U���� : {status._longRangeAttackPower}";
        //�v���C���[�̖h��͂�ݒ肷��
        _playerStatusText[(int)StatusName.DefensePower].text = 
            $"�h��� : {status._defensePower}";
        //�v���C���[�̈ړ����x��ݒ肷��
        _playerStatusText[(int)StatusName.MoveSpeed].text =
            $"�ړ����x : {status._moveSpeed}";
        //�v���C���[�̐�����тɂ�����ݒ肷��
        _playerStatusText[(int)StatusName.DifficultToBlowOff].text = 
            $"������тɂ��� : {status._defensePower}";
    }

    /// <summary> �^�[�Q�b�g�̃X�e�[�^�X�e�L�X�g��ݒ肷��B </summary>
    /// <param name="target"></param>
   �@void Update_TargetPlayerStatusText(StatusName target)
    {
        switch (target)
        {
            case StatusName.PlayerName: _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.BaseStatus._playerName; break;
            case StatusName.MaxHP: _playerStatusText[(int)StatusName.MaxHP].text = "�ő�̗� : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp.ToString(); break;
            case StatusName.MaxStamina: _playerStatusText[(int)StatusName.MaxStamina].text = "�ő�X�^�~�i : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxStamina.ToString(); break;
            case StatusName.ShortRangeAttackPower: _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = "�ߋ����U���� : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._shortRangeAttackPower.ToString(); break;
            case StatusName.LongRangeAttackPower: _playerStatusText[(int)StatusName.LongRangeAttackPower].text = "�������U���� : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._longRangeAttackPower.ToString(); break;
            case StatusName.DefensePower: _playerStatusText[(int)StatusName.DefensePower].text = "�h��� : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._defensePower.ToString(); break;
            case StatusName.MoveSpeed: _playerStatusText[(int)StatusName.MoveSpeed].text = "�ړ����x : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._moveSpeed.ToString(); break;
            case StatusName.DifficultToBlowOff: _playerStatusText[(int)StatusName.DifficultToBlowOff].text = "������тɂ��� : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._difficultToBlowOff.ToString(); break;
            default: Debug.LogError("�����Ȓl�ł��B : Set_PlayerStatusText(StatusName target);"); break;
        }
    }
}
