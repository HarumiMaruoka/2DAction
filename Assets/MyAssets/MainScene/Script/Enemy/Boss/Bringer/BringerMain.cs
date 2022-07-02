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

    [Header("�eButton���q�Ɏ��L�����o�X"), SerializeField] GameObject _bottonCanvas;

    Transform _heavyAttack;
    Transform _lightAttack;

    bool _isLRAStart = false;

    void Start()
    {
        base.InitBoss();
        _heavyAttack = transform.GetChild(1);
        _lightAttack = transform.GetChild(2);
    }

    void Update()
    {
        base.CommonUpdateBoss();
    }

    /// <summary> Bringer�Ǝ��́AUpdateBoss�֐� </summary>
    protected override void UpdateBoss()
    {
        //�̗͂��Ȃ��Ȃ������̏���
        if (_hitPoint <= 0)
        {
            //�̗͂��Ȃ��Ȃ�������ł���
            _nowState = BossState.DIE;
            //Button�Q���A�N�e�B�u�ɂ���
            _bottonCanvas.SetActive(true);
        }

        //�F��ς���K�v������Ες���
        if (_isColorChange)
        {
            _spriteRenderer.color = Color.gray;
            _isColorChange = false;
        }
        //�F�����ɖ߂�
        else if (_colorChangeTimeValue < 0)
        {
            _spriteRenderer.color = new Color(1, 1, 1, 1);
        }
        //�N�[���^�C������
        else if (_colorChangeTimeValue > 0)
        {
            _colorChangeTimeValue -= Time.deltaTime;
        }

        _rigidBody2d.velocity = Vector2.zero;
        //Boss�̃X�e�[�g��ύX����B
        switch (_nowState)
        {
            case BossState.IDLE: Idle(); break;
            case BossState.APPROACH: Approach(); break;
            case BossState.RECESSION: Recession(); break;

            case BossState.LIGHT_ATTACK: LightAttack(); break;
            case BossState.HEAVY_ATTACK: HeavyAttack(); break;
            case BossState.LONG_RANGE_ATTACK: LongRangeAttack(); break;

            case BossState.DIE: Die(); break;
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
            _rigidBody2d.velocity = new Vector2(0f, _rigidBody2d.velocity.y);
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
            new Vector2(-_approachSpeed, _rigidBody2d.velocity.y) :
            new Vector2(_approachSpeed, _rigidBody2d.velocity.y);

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
            new Vector2(_recessionSpeed, _rigidBody2d.velocity.y) :
            new Vector2(-_recessionSpeed, _rigidBody2d.velocity.y);

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
            if (_isLRAStart)
            {
                _isLRAStart = false;
                //Spell���A�N�e�B�u�ɂ��APlaySpell�����s
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetComponent<SpellController>().PlaySpell();
            }
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
        if (_nowState == BossState.LONG_RANGE_ATTACK)
        {
            _isLRAStart = true;
        }
    }

    /// <summary> �X�e�[�g���m�[�}���Ɉڍs </summary>
    void StateChangeNomal()
    {

        int random = UnityEngine.Random.Range(0, 10);
        //10����2�Ń{�X�X�e�[�g��Idle�Ɉڍs
        if (random < 2)
        {
            _nowState = BossState.IDLE;
        }
        //10����6�Ń{�X�X�e�[�g��Approach�Ɉڍs
        else if (random < 9)
        {
            _nowState = BossState.APPROACH;
        }
        //10����2�Ń{�X�X�e�[�g��Recession�Ɉڍs
        else
        {
            _nowState = BossState.RECESSION;
        }
    }

    //Bringer���|���ꂽ�Ƃ��̏���
    void Die()
    {
        //Boss�����񂾎��̏����������ɏ���
        Destroy(gameObject, 1.0f);
    }

    //(�A�^�b�N���)�N�[���^�C����҂�
    IEnumerator CoolTime()
    {
        _isCoolTimerStart = false;
        yield return new WaitForSeconds(_coolTimeValue);
        _isCoolTimeExit = true;
    }

    ///<summary> �A�^�b�N���[�h�̏I�����ɌĂ΂��B�A�j���[�V�����C�x���g����Ăяo���B�e�t���O�̏������������s���B </summary>
    public void AttackStateExit()
    {
        //Debug.Log("AttackExit");
        _isAttackStart = false;
        _isAttackExit = true;
        _isCoolTimerStart = true;
    }

    //�A�^�b�N�֘A�̃��\�b�h�B�A�j���[�V�����C�x���g����Ăяo���B
    public void OnHeavyAttack()
    {
        _heavyAttack.gameObject.SetActive(true);
    }
    public void OffHeavyAttack()
    {
        _heavyAttack.gameObject.SetActive(false);
    }
    public void OnLightAttack()
    {
        _lightAttack.gameObject.SetActive(true);
    }
    public void OffLightAttack()
    {
        _lightAttack.gameObject.SetActive(false);
    }
}
