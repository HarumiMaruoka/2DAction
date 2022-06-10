using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerMain : BossBase
{
    //�e�p�����[�^
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
        StartCoroutine(CoolTime());//�N�[���^�C���J�n
        //�N�[���^�C�����c���Ă���Ƃ��̏���
        if (_isCoolTimeNow)
        {
            //Idle���͓��ɉ������Ȃ�
            Debug.Log("Idle��");
        }
        //�N�[���^�C���������ɃA�^�b�N�Ɉڍs
        else
        {
            Debug.Log("Idle");
            StateChangeAttack();
        }
    }

    /// <summary> Boss��Approach���̏��� </summary>
    void Approach()
    {
        StartCoroutine(CoolTime());//�N�[���^�C���J�n
        //�N�[���^�C�����c���Ă���Ƃ��̏���
        if (_isCoolTimeNow)
        {
            //Approach���̓v���C���[�Ɍ������đO�i����
            _rigidBody2d.velocity = (_playerPos.position.x - transform.position.x) < 0 ?
            new Vector2(_approachSpeed * Time.deltaTime, _rigidBody2d.velocity.y) :
            new Vector2(-_approachSpeed * Time.deltaTime, _rigidBody2d.velocity.y);

            Debug.Log("�v���C���[�Ɍ������đO�i��");
        }
        //�N�[���^�C���������ɃA�^�b�N�Ɉڍs
        else
        {
            Debug.Log("Approach");
            StateChangeAttack();
        }
    }

    /// <summary> Boss��Recession���̏��� </summary>
    void Recession()
    {
        StartCoroutine(CoolTime());//�N�[���^�C���J�n
        //�N�[���^�C�����c���Ă���Ƃ��̏���
        if (_isCoolTimeNow)
        {
            //Recession���̓v���C���[�ɑ΂��Č�ނ���
            _rigidBody2d.velocity = (_playerPos.position.x - transform.position.x) < 0 ?
                new Vector2(-_recessionSpeed * Time.deltaTime, _rigidBody2d.velocity.y) :
                new Vector2(_recessionSpeed * Time.deltaTime, _rigidBody2d.velocity.y);

            Debug.Log("�v���C���[�ɑ΂��Č�ޒ�");
        }
        //�N�[���^�C���������ɃA�^�b�N�Ɉڍs
        else
        {
            Debug.Log("Recession");
            StateChangeAttack();
        }
    }

    /// <summary> Boss��LightAttack���̏��� </summary>
    void LightAttack()
    {
        //LightAttack�́A�ߋ���:��U��
        Debug.Log("��U���U������");
        //�N�[���^�C���ݒ�
        _coolTimeValue = UnityEngine.Random.Range(_lightAttackCoolTime.x, _lightAttackCoolTime.y);
        //ChangeState Nomal
        StateChangeNomal();
    }

    /// <summary> Boss��HeavyAttack���̏��� </summary>
    void HeavyAttack()
    {
        //HeavyAttack�́A�ߋ���:���U��
        Debug.Log("���U������");
        //�N�[���^�C���ݒ�
        _coolTimeValue = UnityEngine.Random.Range(_heavyAttackCoolTime.x, _heavyAttackCoolTime.y);
        //ChangeState Nomal
        StateChangeNomal();
    }

    /// <summary> Boss��LongRangeAttack���̏��� </summary>
    void LongRangeAttack()
    {
        //LongRangeAttack�͉������U��
        Debug.Log("�������U������");
        //�N�[���^�C���ݒ�
        _coolTimeValue = UnityEngine.Random.Range(_longRangeAttackCoolTime.x, _longRangeAttackCoolTime.y);
        //ChangeState Nomal
        StateChangeNomal();
    }

    /// <summary> �X�e�[�g���A�^�b�N�Ɉڍs </summary>
    void StateChangeAttack()
    {
        //�{���̃R�[�h
        //_nowState = (BossState)UnityEngine.Random.Range((int)BossState.LIGHT_ATTACK, (int)BossState.ATTACK_END);

        //�e�X�g�p�R�[�h
        _nowState = BossState.LIGHT_ATTACK;
    }

    /// <summary> �X�e�[�g���m�[�}���Ɉڍs </summary>
    void StateChangeNomal()
    {
        _nowState = (BossState)UnityEngine.Random.Range((int)BossState.IDLE, (int)BossState.NOMAL_END);
    }

    IEnumerator CoolTime()
    {
        _isCoolTimeNow = true;
        yield return new WaitForSeconds(_coolTimeValue);
        _isCoolTimeNow = false;
    }
}
