using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerMain : BossBase
{
    //�e�p�����[�^
    [Tooltip("�ŏ��̂̃N�[���^�C���Bx��MinValue�Ay��MaxValue�B"), SerializeField] Vector2 _firstRandomAttackCoolTime = default;

    [Tooltip("��U����̃N�[���^�C���Bx��MinValue�Ay��MaxValue�B"), SerializeField] Vector2 _lightAttackCoolTime = default;
    [Tooltip("��U����̃N�[���^�C���Bx��MinValue�Ay��MaxValue�B"), SerializeField] Vector2 _heavyAttackCoolTime = default;
    [Tooltip("��U����̃N�[���^�C���Bx��MinValue�Ay��MaxValue�B"), SerializeField] Vector2 _longRangeAttackCoolTime = default;

    [Tooltip("�ڋ߃X�s�[�h"), SerializeField] float _approachSpeed;
    [Tooltip("��ރX�s�[�h"), SerializeField] float _recessionSpeed;



    void Start()
    {
        base.InitBoss();
    }

    void Update()
    {
        base.CommonUpdateBoss();
    }

    /// <summary> Bringer�Ǝ��́AChangeState�֐� </summary>
    protected override void CangeState()
    {
        //�N�[���^�C�����c���Ă���ꍇ�A�U���ȊO�̍s�������

        //�N�[���^�C�����������ꂽ��A�U������
    }

    /// <summary> Bringer�Ǝ��́AUpdateBoss�֐� </summary>
    protected override void UpdateBoss()
    {
        switch (_nowState)
        {
            case BossState.IDLE: Idle(); break;
            case BossState.APPROACH: Approach(); break;
            case BossState.RECESSION: Recession(); break;

            case BossState.LIGHT_ATTACK: LightAttack(); break;
            case BossState.HEAVY_ATTACK: HeavyAttack(); break;
            case BossState.LONG_RANGE_ATTACK: LongRangeAttack(); break;

            //�Ē��I����R�[�h�������ɏ���
            //if��Nomal��Attack�ɕ��򂵁A������ς���
            default: Debug.Log("���̃{�X�̃X�e�[�g�͂���܂���B"); break;
        }
    }

    protected override void BattleStart()
    {
        if (base.CommonBattleStart())
        {
            //������Bringer�Ǝ��́A�퓬�J�n���̏����������B
            Debug.Log("BringerBattleStart!");
            //�ŏ��̃N�[���^�C����ݒ�
            _coolTimeValue = UnityEngine.Random.Range(_firstRandomAttackCoolTime.x, _firstRandomAttackCoolTime.y);
            _isCoolTimerStart = true;
        }
    }

    protected override void BattleExit()
    {
        if (base.CommonBattleExit())
        {
            //������Bringer�Ǝ��́A�퓬�I�����̏����������B
            Debug.Log("BringerBattleExit...");
        }
    }

    /// <summary> Boss��Idle���̏��� </summary>
    void Idle()
    {
        //�N�[���^�C���c�莞�Ԃ̏���
        if (_isCoolTimerStart)
        {
            Debug.Log("IdleTimerStart");
            StartCoroutine(CoolTime());//�N�[���^�C���J�n
            //Idle���͓��ɉ������Ȃ�
        }
        //Idle���̏���
        else
        {
            //Debug.Log("Idle��");
        }
        //�N�[���^�C���������ɃA�^�b�N�Ɉڍs
        if (_isCoolTimeExit)
        {
            _isCoolTimeExit = false;
            Debug.Log("IdleExit");
            StateChangeAttack();
        }
    }

    /// <summary> Boss��Approach���̏��� </summary>
    void Approach()
    {
        //�N�[���^�C���c�莞�Ԃ̏���
        if (_isCoolTimerStart)
        {
            Debug.Log("ApproachTimerStart");
            StartCoroutine(CoolTime());//�N�[���^�C���J�n
        }
        //Approach���̏���
        else
        {
            //Approach���̓v���C���[�Ɍ������đO�i����
            _rigidBody2d.velocity = (_playerPos.position.x - transform.position.x) < 0 ?
            new Vector2(_approachSpeed * Time.deltaTime, _rigidBody2d.velocity.y) :
            new Vector2(-_approachSpeed * Time.deltaTime, _rigidBody2d.velocity.y);

            //Debug.Log("�v���C���[�Ɍ������đO�i��");
        }
        //�N�[���^�C���������ɃA�^�b�N�Ɉڍs
        if (_isCoolTimeExit)
        {
            _isCoolTimeExit = false;
            Debug.Log("ApproachExit");
            StateChangeAttack();
        }
    }

    /// <summary> Boss��Recession���̏��� </summary>
    void Recession()
    {
        //�N�[���^�C���c�莞�Ԃ̏���
        if (_isCoolTimerStart)
        {
            Debug.Log("RecessionTimerStart");
            StartCoroutine(CoolTime());//�N�[���^�C���J�n
        }
        //Approach���̏���
        else
        {
            //Approach���̓v���C���[�Ɍ������đO�i����
            _rigidBody2d.velocity = (_playerPos.position.x - transform.position.x) < 0 ?
            new Vector2(-_recessionSpeed * Time.deltaTime, _rigidBody2d.velocity.y) :
            new Vector2(_recessionSpeed * Time.deltaTime, _rigidBody2d.velocity.y);

            //Debug.Log("�v���C���[�Ɍ������đO�i��");
        }
        //�N�[���^�C���������ɃA�^�b�N�Ɉڍs
        if (_isCoolTimeExit)
        {
            _isCoolTimeExit = false;
            Debug.Log("RecessionExit");
            StateChangeAttack();
        }
    }

    /// <summary> Boss��LightAttack���̏��� </summary>
    void LightAttack()
    {
        if (_isAttackStart)
        {
            //LightAttack�́A�ߋ���:��U��
            //�����Ɏ�U�����̃R�[�h������

            //�e�X�g�p�R�[�h
            AttackStateExit();
        }
        if (_isAttackExit)
        {
            //�N�[���^�C���ݒ�
            _coolTimeValue = UnityEngine.Random.Range(_lightAttackCoolTime.x, _lightAttackCoolTime.y);
            Debug.Log("��U���I��");
            //ChangeState Nomal
            StateChangeNomal();
        }
    }

    /// <summary> Boss��HeavyAttack���̏��� </summary>
    void HeavyAttack()
    {
        if (_isAttackStart)
        {
            //HeavyAttack�́A�ߋ���:���U��
            //�����ɋ��U�����̃R�[�h������

            //�e�X�g�p�R�[�h
            AttackStateExit();
        }
        if (_isAttackExit)
        {
            //�N�[���^�C���ݒ�
            _coolTimeValue = UnityEngine.Random.Range(_heavyAttackCoolTime.x, _heavyAttackCoolTime.y);
            Debug.Log("���U���I��");
            //ChangeState Nomal
            StateChangeNomal();
        }
    }

    /// <summary> Boss��LongRangeAttack���̏��� </summary>
    void LongRangeAttack()
    {
        if (_isAttackStart)
        {
            //LongRangeAttack�͉������U��
            //�����ɉ������U�����̃R�[�h������

            //�e�X�g�p�R�[�h
            AttackStateExit();
        }
        if (_isAttackExit)
        {
            //�N�[���^�C���ݒ�
            _coolTimeValue = UnityEngine.Random.Range(_longRangeAttackCoolTime.x, _longRangeAttackCoolTime.y);
            Debug.Log("�������U���I��");
            //ChangeState Nomal
            StateChangeNomal();
        }
    }

    /// <summary> �X�e�[�g���A�^�b�N�Ɉڍs </summary>
    void StateChangeAttack()
    {
        _isAttackStart = true;
        //�{���̃R�[�h
        _nowState = (BossState)UnityEngine.Random.Range((int)BossState.LIGHT_ATTACK, (int)BossState.ATTACK_END);
    }

    /// <summary> �X�e�[�g���m�[�}���Ɉڍs </summary>
    void StateChangeNomal()
    {
        //�{���̃R�[�h
        _nowState = (BossState)UnityEngine.Random.Range((int)BossState.IDLE, (int)BossState.NOMAL_END);
    }

    IEnumerator CoolTime()
    {
        _isCoolTimerStart = false;
        _isCoolTimeExit = false;
        yield return new WaitForSeconds(_coolTimeValue);
        _isCoolTimeExit = true;
    }

    ///<summary> �A�^�b�N���[�h�̏I�����ɌĂ΂��B�A�j���[�V�����C�x���g����Ăяo���B </summary>
    public void AttackStateExit()
    {
        Debug.Log("AttackExit");
        _isAttackStart = false;
        _isAttackExit = true;
        _isCoolTimerStart = true;
    }
}
