using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ��{�a���U���N���X </summary>
public class NomalSlashingAttack : PlayerWeaponBase
{
    /// <summary> �A�N�e�B�u�ɂȂ������̃v���C���[�̌��� </summary>
    bool _onActivationPlayerDirection_isRight;

    /// <summary> ��{�a���U���N���X�Ǝ��̏��������� </summary>
    protected override void WeaponInit()
    {
        // GetComponent<>() ���̋��ʂ̏������s���B
        base.WeaponInit();
        // ����ID��ݒ肷��B
        _myWeaponID = (int)ArmParts.WeaponID.WeaponID_00_NomalShotAttack;
        //PressType��ݒ肷��B
        _isPressType = true;
    }

    private void Update()
    {
        if(_onActivationPlayerDirection_isRight == PlayerStatusManager.Instance.IsRight)
        {
            transform.position = transform.position + _positionOffsetAtBirth;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary> �A�N�e�B�u�ɂȂ������̏��� </summary>
    private void OnEnable()
    {
        OnEnable_ThisWeapon();
        //�A�N�e�B�u�ɂȂ������̃v���C���[�̌�����ۑ����Ă����B
        _onActivationPlayerDirection_isRight = PlayerStatusManager.Instance.IsRight;
    }

    /// <summary> ��A�N�e�B�u�ɂȂ������̏��� </summary>
    private void OnDisable()
    {
        
    }
}
