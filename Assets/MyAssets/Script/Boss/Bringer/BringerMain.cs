using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerMain : BossBase
{
    //�e�p�����[�^
    [Tooltip("�ŏ��̂̃N�[���^�C���Bx��MinValue�Ay��MaxValue�B"), SerializeField] Vector2 _firstRandomAttackCoolTime = default;

    [Tooltip("��U����̃N�[���^�C���Bx��MinValue�Ay��MaxValue�B"), SerializeField] Vector2 _lightAttackCoolTime = default;
    [Tooltip("���U����̃N�[���^�C���Bx��MinValue�Ay��MaxValue�B"), SerializeField] Vector2 _heavyAttackCoolTime = default;
    [Tooltip("�������U����̃N�[���^�C���Bx��MinValue�Ay��MaxValue�B"), SerializeField] Vector2 _longRangeAttackCoolTime = default;

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

    /// <summary> Bringer�Ǝ��́AUpdateBoss�֐� </summary>
    protected override void UpdateBoss()
    {
        //Boss�̃X�e�[�g��ύX����B
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

    //�v���C���[��Boss���A������x�߂Â�����Ă΂��B
    protected override void BattleStart()
    {
        if (base.IsBattleStart())
        {
            Debug.Log("start");
            //������Bringer�Ǝ��́A�퓬�J�n���̏����������B
            //Debug.Log("BringerBattleStart!");
            //�ŏ��̃N�[���^�C����ݒ�
            _coolTimeValue = UnityEngine.Random.Range(_firstRandomAttackCoolTime.x, _firstRandomAttackCoolTime.y);
            _isCoolTimerStart = true;
        }
    }

    /// <summary> �v���C���[��Boss���A���ꂷ������Ă΂��B </summary>
    protected override void BattleExit()
    {
        if (base.IsBattleExit())
        {
            Debug.Log("exit");
            //������Bringer�Ǝ��́A�퓬�I�����̏����������B
            //Debug.Log("BringerBattleExit...");
        }
    }

    /// <summary> Boss��Idle���̏��� </summary>
    void Idle()
    {
        //�N�[���^�C���J�n�̏���
        if (_isCoolTimerStart)
        {
            //Debug.Log("IdleStart");
            _isCoolTimeExit = false;
            _isAttackExit = false;
            StartCoroutine(CoolTime());
        }
        //Idle���̏���
        else
        {
            //Debug.Log("Idle��");
        }
        //�A�^�b�N�Ɉڍs���鎞�̏���
        if (_isCoolTimeExit)
        {
            _isCoolTimeExit = false;
            //Debug.Log("IdleExit");
            StateChangeAttack();
        }
    }

    /// <summary> Boss��Approach���̏��� </summary>
    void Approach()
    {
        //�N�[���^�C���J�n�̏���
        if (_isCoolTimerStart)
        {
            //Debug.Log("ApproachStart");
            _isCoolTimeExit = false;
            _isAttackExit = false;
            StartCoroutine(CoolTime());//�N�[���^�C���J�n
        }
        //Approach���̏���
        else
        {
            //Debug.Log("�O�i��");
            //Approach���̓v���C���[�Ɍ������đO�i����
            _rigidBody2d.velocity = (_playerPos.position.x - transform.position.x) < 0 ?
            new Vector2(-_approachSpeed * Time.deltaTime * 100f, _rigidBody2d.velocity.y) :
            new Vector2(_approachSpeed * Time.deltaTime * 100f, _rigidBody2d.velocity.y);

            //Debug.Log("�v���C���[�Ɍ������đO�i��");
        }
        //�A�^�b�N�Ɉڍs���鎞�̏���
        if (_isCoolTimeExit)
        {
            _isCoolTimeExit = false;
            //Debug.Log("ApproachExit");
            StateChangeAttack();
        }
    }

    /// <summary> Boss��Recession���̏��� </summary>
    void Recession()
    {
        //Debug.Log("RecessionStart");
        //�N�[���^�C���J�n�̏���
        if (_isCoolTimerStart)
        {
            _isCoolTimeExit = false;
            _isAttackExit = false;
            StartCoroutine(CoolTime());//�N�[���^�C���J�n
        }
        //Approach���̏���
        else
        {
            //Debug.Log("��i��");
            //Approach���̓v���C���[�Ɍ������đO�i����
            _rigidBody2d.velocity = (_playerPos.position.x - transform.position.x) < 0 ?
            new Vector2(_recessionSpeed * Time.deltaTime * 100f, _rigidBody2d.velocity.y) :
            new Vector2(-_recessionSpeed * Time.deltaTime * 100f, _rigidBody2d.velocity.y);

            //Debug.Log("�v���C���[�Ɍ������đO�i��");
        }
        //�A�^�b�N�Ɉڍs���鎞�̏���
        if (_isCoolTimeExit)
        {
            _isCoolTimeExit = false;
            //Debug.Log("RecessionExit");
            StateChangeAttack();
        }
    }

    /// <summary> Boss��LightAttack���̏��� </summary>
    void LightAttack()
    {
        //��U���̏���
        if (_isAttackStart)
        {
            //LightAttack�́A�ߋ���:��U��
            //�����Ɏ�U�����̃R�[�h������
            //Debug.Log("AttackNow");
            //�e�X�g�p�R�[�h
            //AttackStateExit();
        }
        //�U���I�����̏���
        if (_isAttackExit)
        {
            //�N�[���^�C���ݒ肵�A�X�e�[�g��ύX����B
            _coolTimeValue = UnityEngine.Random.Range(_lightAttackCoolTime.x, _lightAttackCoolTime.y);
            StateChangeNomal();

            //Debug.Log("��U���I��");
        }
    }

    /// <summary> Boss��HeavyAttack���̏��� </summary>
    void HeavyAttack()
    {
        //���U���̏���
        if (_isAttackStart)
        {
            //HeavyAttack�́A�ߋ���:���U��
            //�����ɋ��U�����̃R�[�h������
            //Debug.Log("AttackNow");
            //�e�X�g�p�R�[�h
            //AttackStateExit();
        }
        //�U���I�����̏���
        if (_isAttackExit)
        {
            //�N�[���^�C���ݒ肵�A�X�e�[�g��ύX����B
            _coolTimeValue = UnityEngine.Random.Range(_heavyAttackCoolTime.x, _heavyAttackCoolTime.y);
            StateChangeNomal();

            //Debug.Log("���U���I��");
        }
    }

    /// <summary> Boss��LongRangeAttack���̏��� </summary>
    void LongRangeAttack()
    {
        //�������U���̏���
        if (_isAttackStart)
        {
            //LongRangeAttack�͉������U��
            //�����ɉ������U�����̃R�[�h������
            //Debug.Log("AttackNow");
            //�e�X�g�p�R�[�h
            //AttackStateExit();
        }
        //�U���I�����̏���
        if (_isAttackExit)
        {
            //�N�[���^�C���ݒ肵�A�X�e�[�g��ύX����B
            _coolTimeValue = UnityEngine.Random.Range(_longRangeAttackCoolTime.x, _longRangeAttackCoolTime.y);
            StateChangeNomal();

            //Debug.Log("�������U���I��");
        }
    }

    /// <summary> �X�e�[�g���A�^�b�N�Ɉڍs </summary>
    void StateChangeAttack()
    {
        _isAttackStart = true;
        _nowState = (BossState)UnityEngine.Random.Range((int)BossState.LIGHT_ATTACK, (int)BossState.ATTACK_END);
    }

    /// <summary> �X�e�[�g���m�[�}���Ɉڍs </summary>
    void StateChangeNomal()
    {
        //�{���̃R�[�h
        _nowState = (BossState)UnityEngine.Random.Range((int)BossState.IDLE, (int)BossState.NOMAL_END);
    }

    //�N�[���^�C����҂�
    IEnumerator CoolTime()
    {
        _isCoolTimerStart = false;
        yield return new WaitForSeconds(_coolTimeValue);
        _isCoolTimeExit = true;
    }

    ///<summary> �A�^�b�N���[�h�̏I�����ɌĂ΂��B�A�j���[�V�����C�x���g����Ăяo���B </summary>
    public void AttackStateExit()
    {
        //Debug.Log("AttackExit");
        _isAttackStart = false;
        _isAttackExit = true;
        _isCoolTimerStart = true;
    }
}
