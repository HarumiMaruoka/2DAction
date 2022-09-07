using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �������U������̊��N���X�B
/// </summary>
public class LongRangeWeaponBase : WeaponBase
{
    //<===== �����o�[�ϐ� =====>//


    //<===== ���z�֐� =====>//
    protected override void OnHitEnemy(EnemyBase enemy)
    {
        //�v���C���[�̉������U���͂ɉ����āA�ΏۂɃ_���[�W��^����B
        enemy.HitPlayerAttack(PlayerStatusManager.Instance.ConsequentialPlayerStatus._longRangeAttackPower);
        //�_���[�W��^��������ł���B
        Destroy(gameObject);
    }

    /// <summary> ���������� </summary>
    /// <returns> �������ɐ��������� true ,�����łȂ���� false ��Ԃ��B</returns>
    protected virtual bool Initialized()
    {
        return true;
    }

    /// <summary> ���G�ȓ��������镐��̏ꍇ�A�ړ������̃��\�b�h�ɋL�ڂ���B </summary>
    protected virtual void Move()
    {

    }
}
