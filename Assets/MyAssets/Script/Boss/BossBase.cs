using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BossBase : MonoBehaviour
{
    public enum BossState
    {
        IDLE,//�ҋ@
        APPROACH,//�ڋ�
        RECESSION,//���
        NOMAL_END,

        LIGHT_ATTACK,//��U��
        HEAVY_ATTACK,//���U��
        LONG_RANGE_ATTACK,//�������U��

        ATTACK_END
    }

    [Tooltip("�̗�"), SerializeField] protected int _hitPoint;
    [Tooltip("�U����"), SerializeField] protected int _offensive_Power;
    [Tooltip("�v���C���[�ɑ΂���m�b�N�o�b�N��"), SerializeField] protected Vector2 _playerKnockBackPower;

    //�v���C���[�̃R���|�[�l���g
    GameObject _playerObjedt;
    protected Transform _playerPos;
    protected PlayerBasicInformation _playerBasicInformation;
    protected Rigidbody2D _playersRigidBody2D;
    protected PlayerMoveManager _playerMoveManager;

    //���g�̃R���|�[�l���g
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rigidBody2d;
    protected Animator _animator;

    //�e�p�����[�^
    /// <summary> ���̃L�����������Ă������ </summary>
    protected bool _isRight;

    /// <summary> �퓬�����ǂ����֘A </summary>
    private bool _previousFrameIsBattle = false;
    private bool _isBattle = false;

    //�N�[���^�C���֘A
    /// <summary> ���݂̃N�[���^�C�� </summary>
    protected bool _isCoolTimeNow = false;
    [SerializeField] protected float _coolTimeValue = 0f;
    /// <summary> �{�X�U����̃N�[���^�C�� </summary>
    Dictionary<BossState, float> _bossAttackCoolTime = new Dictionary<BossState, float>();

    [Tooltip("�퓬�J�n�܂ł̋���"), SerializeField] private Vector2 _fightStartDistance;
    [Tooltip("�퓬��~�܂ł̋���"), SerializeField] private Vector2 _fightStopDistance;

    //�F�ύX�p
    bool _isColorChange = false;
    float _colorChangeTime = 0.1f;
    float _colorChangeTimeValue = 0;

    /// <summary> �m�b�N�o�b�N�֘A </summary>
    bool _isKnockBackNow;
    float _knockBackModeTime = 0f;

    //�����̏��
    protected BossState _nowState;

    protected void InitBoss()
    {
        //�v���C���[�̃R���|�[�l���g���擾
        _playerObjedt = GameObject.Find("ChibiRobo");
        _playerBasicInformation = _playerObjedt.GetComponent<PlayerBasicInformation>();
        _playerPos = _playerObjedt.GetComponent<Transform>();
        _playersRigidBody2D = _playerObjedt.GetComponent<Rigidbody2D>();
        _playerMoveManager = _playerObjedt.GetComponent<PlayerMoveManager>();

        //���g�̃R���|�[�l���g���擾
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    /// <summary> �p�����Update�Ŏ��s���ׂ��֐� </summary>
    protected void CommonUpdateBoss()
    {
        DoFight();//�키���ǂ���
        CommonBattleStart();//�퓬�J�n���̂ݎ��s
        BattleStart();//�Ǝ��̐퓬�J�n���̏���
        CommonBattleExit();//�퓬�I�����̂ݎ��s
        BattleExit();//�Ǝ��̐퓬�I�����̏���
        if (_isBattle)
        {
            CangeState();
            UpdateBoss();
        }
    }

    /// <summary> �����𑪂�A�퓬���邩�ǂ������f���� </summary>
    void DoFight()
    {
        //�O�t���[���̏�Ԃ�ۑ�����
        _previousFrameIsBattle = _isBattle;
        //�v���C���[�Ƃ̋����𑪂鏈���������ɏ���
        Vector2 difference = _playerPos.position - transform.position;
        float diffX = Mathf.Abs(difference.x);
        float diffY = Mathf.Abs(difference.y);
        //���������ȏ�߂Â�����_isFight��True�ɂ���
        if (diffX < 7.5f && diffY < 5f)
        {
            _isBattle = true;
        }
        //���������ȏ㗣�ꂽ��_isFight��False�ɂ���
        if (diffX > 15f || diffY > 10f)
        {
            _isBattle = false;
        }
    }

    /// <summary> �{�X�̃X�e�[�g���A�K�v�ɉ����ĕύX����B�v Over ride </summary>
    virtual protected void CangeState() { }

    /// <summary> �{�X�̏����B�v Over ride </summary>
    virtual protected void UpdateBoss() { }


    /// <summary> �v���C���[�ƐڐG�����Ƃ��ɌĂ΂�� </summary>
    void HitPlayer()//�v���C���[�̗̑͂����炵�A�m�b�N�o�b�N������B
    {
        //�v���C���[��HitPoint�����炷
        _playerBasicInformation._playerHitPoint -= _offensive_Power;
        _playersRigidBody2D.velocity = Vector2.zero;
        //�v���C���[���m�b�N�o�b�N����
        if (_isRight)
        {
            _playersRigidBody2D.AddForce((Vector2.right + Vector2.up) * _playerKnockBackPower, ForceMode2D.Impulse);
        }
        else
        {
            _playersRigidBody2D.AddForce((Vector2.left + Vector2.up) * _playerKnockBackPower, ForceMode2D.Impulse);
        }
    }

    /// <summary> �v���C���[�̍U���ɐڐG�����Ƃ��ɌĂ΂�� </summary>
    void HitPlayerAttack(int damage)//�����̗̑͂����炷�B
    {
        //���g�̗̑͂����炵�A0.1�b�����F��Ԃɕς���B
        _hitPoint -= damage;
        _isColorChange = true;
        _colorChangeTimeValue = _colorChangeTime;
        StartCoroutine(KnockBackMode());
    }

    //�m�b�N�o�b�N�p�̃R�[�h�B
    IEnumerator KnockBackMode()
    {
        _isKnockBackNow = true;
        yield return new WaitForSeconds(_knockBackModeTime);
        _isKnockBackNow = false;
    }

    /// <summary> �퓬�J�n���̏��� </summary>
    protected bool CommonBattleStart()
    {
        //_isBattle��false����true�ɂȂ����t���[���̂ݎ��s����
        if (_previousFrameIsBattle == false && _isBattle == true)
        {
            //�퓬�J�n���Ɏ��s�����������������ɏ���
            return true;
        }
        return false;
    }

    /// <summary> �퓬�I�����̏��� </summary>
    protected bool CommonBattleExit()
    {
        //_isBattle��true����false�ɂȂ����t���[���̂ݎ��s����
        if (_previousFrameIsBattle == true && _isBattle == false)
        {
            //�퓬�I�����Ɏ��s�����������������ɏ���
            return true;
        }
        return false;
    }

    virtual protected void BattleStart() { }

    virtual protected void BattleExit() { }
}
