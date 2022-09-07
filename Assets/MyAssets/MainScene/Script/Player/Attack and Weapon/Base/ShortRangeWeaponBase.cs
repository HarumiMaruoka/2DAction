using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �ߋ����U������̊��N���X�B
/// </summary>
public class ShortRangeWeaponBase : WeaponBase
{
    /// <summary> �G�l�~�[�ɑ΂��čU�����q�b�g�����ۂ̍d������ </summary>
    [Header("�G�l�~�[�ɑ΂��čU�����q�b�g�����ۂ̍d������"), SerializeField]
    protected float _basicRecoveryTime;
    /// <summary> �G�l�~�[�ɑ΂��čU�����q�b�g�����ۂ̃m�b�N�o�b�N�� </summary>
    [Header("�G�l�~�[�ɑ΂��čU�����q�b�g�����ۂ̃m�b�N�o�b�N��"), SerializeField]
    protected float _basicKnockbackForce;
    protected override void OnHitEnemy(EnemyBase enemy)
    {
        //�v���C���[�̋ߋ����U���͂ɉ����āA�ΏۂɃ_���[�W��^����B
        //���A�v���C���[�̏d�ʂɉ����đΏۂ��m�b�N�o�b�N�����A�d��������B
        enemy.HitPlayerAttack
            (
            PlayerStatusManager.Instance.ConsequentialPlayerStatus._shortRangeAttackPower,
            _basicRecoveryTime,
            _basicKnockbackForce
            );
    }

    /// <summary> ���������� </summary>
    /// <returns> �������ɐ��������� true ,�����łȂ���� false ��Ԃ��B</returns>
    protected virtual bool Initialized()
    {
        return true;
    }
}
