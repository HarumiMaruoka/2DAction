using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ��{�a���U���N���X </summary>
public class NomalSlashingAttack : PlayerWeaponBase
{
    /// <summary> ��{�a���U���N���X�Ǝ��̏��������� </summary>
    protected override void WeaponInit()
    {
        // �v���C���[�̃g�����X�t�H�[�����擾����B
        _playerPos = transform.parent;
        // ����ID��ݒ肷��B
        _myWeaponID = (int)ArmParts.WeaponID.WeaponID_01_NomalSlashingAttack;
        //PressType��ݒ肷��B
        _isPressType = true;
    }

    private void Start()
    {
        
    }

    public override void Run_FireProcess()
    {
        UpdatePosition();
    }
}
