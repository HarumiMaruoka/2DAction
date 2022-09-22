using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 
/// ������ʂɁA�v���C���[�X�e�[�^�X��`�悷��R���|�[�l���g�B
/// </summary>
public class DrawPlayerStatus_OnEquipmentWindow : MonoBehaviour
{

    /// <summary> ���̃N���X���������������ǂ��� </summary>
    bool _whetherInitialized = false;
    /// <summary> �q�I�u�W�F�N�g�����A�v���C���[�X�e�[�^�X��\������e�L�X�g�R���|�[�l���g�̔z�� </summary>
    Text[] _playerStatusText;

    //<===== unity���b�Z�[�W =====>//
    void Start()
    {
        Initialize_ThisClass();
    }
    void OnEnable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment += Update_StatusText;
    }
    void OnDisable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment -= Update_StatusText;
    }

    //<===== private�����o�[�֐� =====>//
    /// <summary> ���̃N���X�̏������֐��B����������true��Ԃ��B </summary>
    bool Initialize_ThisClass()
    {
        //�e�L�X�g�����S�Ă̎q�I�u�W�F�N�g���擾����B
        _playerStatusText = GetComponentsInChildren<Text>();
        //�擾�����e�L�X�g���X�V����B
        Update_StatusText();

        return true;
    }
    /// <summary> �v���C���[�X�e�[�^�X�e�L�X�g���X�V���鏈�� </summary>
    /// <param name="drawAmountOfChangeFlag"> �ω�����\�����邩�ǂ��� : true �Ȃ�\������B </param>
    void Update_StatusText()
    {
        var status = PlayerStatusManager.Instance.ConsequentialPlayerStatus;
        _playerStatusText[Constants.PLAYER_NAME_DRAW_AREA].text = PlayerStatusManager.Instance.BaseStatus._playerName;

        _playerStatusText[Constants.MAX_HP_DRAW_AREA].text = $"�ő�̗� : {status._maxHp}";

        _playerStatusText[Constants.MAX_STAMINA_TYPE_DRAW_AREA].text = $"�ő�X�^�~�i : {status._maxStamina}";

        _playerStatusText[Constants.SHORT_RANGE_ATTACK_POWER_DRAW_AREA].text = $"�ߋ����U���� : {status._shortRangeAttackPower}";

        _playerStatusText[Constants.LONG_RANGE_ATTACK_POWER_DRAW_AREA].text = $"�������U���� : {status._longRangeAttackPower}";

        _playerStatusText[Constants.DEFENSE_POWER_DRAW_AREA].text = $"�h��� : {status._defensePower}";

        _playerStatusText[Constants.MOVE_SPEED_DRAW_AREA].text = $"�ړ����x : {status._moveSpeed}";

        _playerStatusText[Constants.DIFFICULT_TO_BLOW_OFF_DRAW_AREA].text = $"������тɂ��� : {status._difficultToBlowOff}";
    }
}
