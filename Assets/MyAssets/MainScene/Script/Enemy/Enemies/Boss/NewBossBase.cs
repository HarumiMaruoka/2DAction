using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss�̊��N���X :
/// �V�����ABossBase�R���|�[�l���g�B
/// ���͂܂��g�p���Ă��Ȃ��B
/// 
/// �ύX�_ : EnemyBase���p�����Ă���_�B
/// </summary>
public class NewBossBase : EnemyBase
{
    //<=========== ���̃N���X�Ŏg�p����^ ===========>//
    /// <summary> �{�X�̃X�e�[�g��\���^ </summary>
    public enum BossState
    {
        IDLE,//�ҋ@
        APPROACH,//�ڋ�
        RECESSION,//���
        NOMAL_END,

        LIGHT_ATTACK,//��U��
        HEAVY_ATTACK,//���U��
        LONG_RANGE_ATTACK,//�������U��

        ATTACK_END,

        DIE,//��
    }

    //<============= �����o�[�ϐ� =============>//
    // �퓬�����ǂ����֘A
    /// <summary> �O�̃t���[���Ő퓬��Ԃ��������H : �퓬���ł���� true </summary>
    private bool _beforeFrameIsFight = false;
    /// <summary> ���݂̃t���[���Ő퓬��Ԃ��H : �퓬���ł���� true </summary>
    private bool _isFight = false;

    //�N�[���^�C���֘A
    /// <summary> ���݃N�[���^�C�������ǂ��� </summary>
    protected bool _isCoolTimerNow = false;
    /// <summary> ���݃N�[���^�C�������ǂ����̑O�t���[���̒l </summary>
    protected bool _beforeIsCoolTimerNow = false;
    /// <summary> �N�[���^�C������ </summary>
    protected float _coolTimeValue = 0f;

    //�U���֘A
    /// <summary> �U�����J�n���邩�H : �J�n����t���[���� true </summary>
    protected bool _isAttackStart = false;
    /// <summary> �U�����I�����邩�H : �I������t���[���� true </summary>
    protected bool _isAttackExit = false;

    /// <summary> �{�X�U����̃N�[���^�C�� </summary>
    Dictionary<BossState, float> _bossAttackCoolTime = new Dictionary<BossState, float>();

    [Tooltip("�퓬�J�n�܂ł̋���"), SerializeField] private Vector2 _fightStartDistance;
    [Tooltip("�퓬��~�܂ł̋���"), SerializeField] private Vector2 _fightStopDistance;

    /// <summary> ���݂̃X�e�[�g </summary>
    public BossState _nowState { get; protected set; }


    //<============= protected�����o�[�֐� =============>//
    /// <summary> BossBase�̏������֐� </summary>
    /// <returns> ���������� true ��Ԃ��B </returns>
    protected bool Initialize_BossBase()
    {
        if (!base.Initialize_Enemy())
        {
            Debug.LogError($"�������Ɏ��s���܂����B{gameObject.name}");
            return false;
        }
        return true;
    }
    protected virtual void Update_BossBase()
    {

    }

    /// <summary> �U���J�n�̃t���[�������m���� </summary>
    /// <returns> �U���J�n�̃t���[���� true ��Ԃ��B </returns>
    protected bool StartAttack()
    {
        return _beforeIsCoolTimerNow == false && _isCoolTimerNow == true;
    }
    /// <summary> �U���I���̃t���[�������m���� </summary>
    /// <returns> �U���I���̃t���[���� true ��Ԃ��B </returns>
    protected bool EndAttack()
    {
        return _beforeIsCoolTimerNow == true && _isCoolTimerNow == false;
    }

    //<============= private�����o�[�֐� =============>//



    //<============= �R���[�`�� =============>//
    IEnumerator CoolTime_Coroutine()
    {
        _isCoolTimerNow = true;
        yield return new WaitForSeconds(_coolTimeValue);
        _isCoolTimerNow = false;
    }


    //<============= ���z�֐� =============>//
    /// <summary> �U���J�n���� : �I�[�o�[���C�h�� </summary>
    protected virtual void StartAttackProcess() { }
    /// <summary> �U���I������ : �I�[�o�[���C�h�� </summary>
    protected virtual void EndAttackProcess() { }
}
