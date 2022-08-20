using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss�̊��N���X :
/// �V�����ABossBase�R���|�[�l���g�B
/// ���͂܂��g�p���Ă��Ȃ��B
/// 
/// �ύX�_ : EnemyBase���p�����Ă���_�B
///          ���̑����낢��œK��
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
    private bool _beforeIsCoolTimerNow = false;
    /// <summary> �N�[���^�C������ </summary>
    protected float _coolTimeValue = 0f;
    /// <summary> ���ݍU�������ǂ��� </summary>
    protected bool _isAttackNow = false;

    /// <summary> �{�X�U����̃N�[���^�C�� </summary>
    Dictionary<BossState, float> _bossAttackCoolTime = new Dictionary<BossState, float>();

    [Tooltip("�퓬�J�n�܂ł̋���"), SerializeField] private Vector2 _fightStartDistance;
    [Tooltip("�퓬��~�܂ł̋���"), SerializeField] private Vector2 _fightStopDistance;

    /// <summary> ���݂̃X�e�[�g </summary>
    public BossState _nowState { get; protected set; }

    protected Animator _animator;


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
        if (!(_animator=GetComponent<Animator>()))
        {
            Debug.LogError($"Animator�R���|�[�l���g�̎擾�Ɏ��s���܂����B : {gameObject.name}");
            Debug.LogError($"�������Ɏ��s���܂����B{gameObject.name}");
            return false;
        }
        return true;
    }
    /// <summary> �{�X���ʂ̍X�V���� : �I�[�o�[���C�h�� </summary>
    protected virtual void CommonUpdate_BossBase()
    {
        // ��̃t���[���p�ɁA�N�[���^�C�����ǂ����𔻒肷��l��ۑ����Ă����B
        _beforeIsCoolTimerNow = _isCoolTimerNow;

        // �ȉ��͔��������ōs���Ă���̂ŁA���s���ׂ��^�C�~���O�ŏ���Ɏ��s���Ă����B
        Update_StartAttackProcess();
        Update_EndAttackProcess();
    }

    /// <summary> �U���J�n�̃t���[�������m���� </summary>
    /// <returns> �U���J�n�̃t���[���� true ��Ԃ��B </returns>
    protected bool Get_IsAttackStart()
    {
        return _beforeIsCoolTimerNow == false && _isCoolTimerNow == true;
    }
    /// <summary> �U���I���̃t���[�������m���� </summary>
    /// <returns> �U���I���̃t���[���� true ��Ԃ��B </returns>
    protected bool Get_IsAttackEnd()
    {
        return _beforeIsCoolTimerNow == true && _isCoolTimerNow == false;
    }

    //<============= private�����o�[�֐� =============>//
    /// <summary> �U���J�n�����m���ď��������s����B </summary>
    void Update_StartAttackProcess()
    {
        if (Get_IsAttackStart())
        {
            StartAttackProcess();
        }
    }
    /// <summary> �U���I�������m���ď��������s����B </summary>
    void Update_EndAttackProcess()
    {
        if (Get_IsAttackEnd())
        {
            EndAttackProcess();
        }
    }

    //<============= �R���[�`�� =============>//
    /// <summary> �N�[���^�C�����J�n����B : �w�肳�ꂽ���ԃN�[���^�C���ϐ��� true �ɂ���B </summary>
   �@protected IEnumerator StartCoolTime()
    {
        _isCoolTimerNow = true;
        yield return new WaitForSeconds(_coolTimeValue);
        _isCoolTimerNow = false;
    }


    //<============= ���z�֐� =============>//
    /// <summary> �U���J�n���� : �I�[�o�[���C�h���� </summary>
    protected virtual void StartAttackProcess()
    {
        // �����ɁA�I�[�o�[���C�h��ŃA�j���[�V�����̑J�ڏ������́A�U���J�n�Ɋւ�鏈�����L�q���Ă��������B
    }
    /// <summary> �U���I������ : �I�[�o�[���C�h���� </summary>
    protected virtual void EndAttackProcess()
    {
        // �����ɁA�I�[�o�[���C�h��ŃA�j���[�V�����̑J�ڏ�����A�N�[���^�C���J�n�������́A�U���I���Ɋւ�鏈�����L�q���Ă��������B
    }
}
