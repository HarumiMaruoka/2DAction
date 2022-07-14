using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> ������ʂɃv���C���[�X�e�[�^�X��`�悷��@�\ </summary>
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

    void Start()
    {
        //���������Ă��Ȃ���Ώ���������B
        if (!_whetherInitialized)
        {
            _whetherInitialized = Initialize_ThisClass();
        }
        if (!_whetherInitialized)
        {
            Debug.LogError("�N���X�̏������Ɏ��s���܂����B : �N���X�� : DrawPlayerStatus_OnEquipmentWindow");
        }
    }

    void Update()
    {

    }

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
        _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.PlayerName;
        //�v���C���[�̍ő�̗͂�ݒ肷��
        _playerStatusText[(int)StatusName.MaxHP].text = PlayerStatusManager.Instance.PlayerMaxHealthPoint.ToString();
        //�v���C���[�̍ő�X�^�~�i��ݒ肷��
        _playerStatusText[(int)StatusName.MaxStamina].text = PlayerStatusManager.Instance.PlayerMaxStamina.ToString();
        //�v���C���[�̋ߋ����U���͂�ݒ肷��
        _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = PlayerStatusManager.Instance.PlayerShortRangeAttackPower.ToString();
        //�v���C���[�̉������U���͂�ݒ肷��
        _playerStatusText[(int)StatusName.LongRangeAttackPower].text = PlayerStatusManager.Instance.PlayerLongRangeAttackPower.ToString();
        //�v���C���[�̖h��͂�ݒ肷��
        _playerStatusText[(int)StatusName.DefensePower].text = PlayerStatusManager.Instance.PlayerDefensePower.ToString();
        //�v���C���[�̈ړ����x��ݒ肷��
        _playerStatusText[(int)StatusName.MoveSpeed].text = PlayerStatusManager.Instance.PlayerMoveSpeed.ToString();
        //�v���C���[�̐�����тɂ�����ݒ肷��
        _playerStatusText[(int)StatusName.DifficultToBlowOff].text = PlayerStatusManager.Instance.PlayerDifficultToBlowOff.ToString();
    }

    /// <summary> �^�[�Q�b�g�̃X�e�[�^�X�e�L�X�g��ݒ肷��B </summary>
    /// <param name="target"></param>
    void Set_PlayerStatusText(StatusName target)
    {
        switch (target)
        {
            case StatusName.PlayerName: _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.PlayerName; break;
            case StatusName.MaxHP: _playerStatusText[(int)StatusName.MaxHP].text = PlayerStatusManager.Instance.PlayerMaxHealthPoint.ToString(); break;
            case StatusName.MaxStamina: _playerStatusText[(int)StatusName.MaxStamina].text = PlayerStatusManager.Instance.PlayerMaxStamina.ToString(); break;
            case StatusName.ShortRangeAttackPower: _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = PlayerStatusManager.Instance.PlayerShortRangeAttackPower.ToString(); break;
            case StatusName.LongRangeAttackPower: _playerStatusText[(int)StatusName.LongRangeAttackPower].text = PlayerStatusManager.Instance.PlayerLongRangeAttackPower.ToString(); break;
            case StatusName.DefensePower: _playerStatusText[(int)StatusName.DefensePower].text = PlayerStatusManager.Instance.PlayerDefensePower.ToString(); break;
            case StatusName.MoveSpeed: _playerStatusText[(int)StatusName.MoveSpeed].text = PlayerStatusManager.Instance.PlayerMoveSpeed.ToString(); break;
            case StatusName.DifficultToBlowOff: _playerStatusText[(int)StatusName.DifficultToBlowOff].text = PlayerStatusManager.Instance.PlayerDifficultToBlowOff.ToString(); break;
            default: Debug.LogError("�����Ȓl�ł��B : Set_PlayerStatusText(StatusName target);");break;
        }
    }
}
