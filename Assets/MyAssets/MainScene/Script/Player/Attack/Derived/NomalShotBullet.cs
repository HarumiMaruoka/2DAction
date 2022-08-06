using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> ��{�ˌ��U���R���|�[�l���g </summary>
public class NomalShotBullet : PlayerWeaponBase
{
    //<======= �����o�[�ϐ� =======>//
    [Header("�e�̑��x"), SerializeField] float _bulletSpeed = 10f;

    /// <summary> ��{�ˌ��U���N���X�Ǝ��̏��������� </summary>
    protected override void WeaponInit()
    {
        base.WeaponInit();
        //ID��ݒ肷��B
        _myWeaponID = (int)ArmParts.WeaponID.WeaponID_00_NomalShotAttack;
        //PressType��ݒ肷��B
        _isPressType = false;
    }

    /// <summary> Fire�{�^���������ꂽ�Ƃ��̏��� : �f���Q�[�g�ϐ��ɓo�^���Ăяo���B </summary>
    public override void Run_FireProcess()
    {
        //���̃R���|�[�l���g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g���A�N�e�B�u�ɂ���B
        gameObject.SetActive(true);
        //�A�j���[�V�������Đ�����B : �A�j���[�V�����p�����[�^��ݒ肷��B
        _animator.SetInteger("WeaponID", _myWeaponID);
        //�v���C���[�������Ă�������Ɉ꒼���ɔ��ōs��
        _rigidBody2D.velocity = Vector2.right * (PlayerStatusManager.Instance.IsRight ? _bulletSpeed : -_bulletSpeed);
    }

    /// <summary> �I�u�W�F�N�g���A�N�e�B�u�ɂȂ������̏��� </summary>
    private void OnEnable()
    {
        OnEnable_ThisWeapon();
    }

    /// <summary> �I�u�W�F�N�g����A�N�e�B�u�ɂȂ������̏��� </summary>
    private void OnDisable()
    {

    }
}
