using UnityEngine;
using System.Collections.Generic;

public class BossBase : MonoBehaviour
{
    public enum BossState
    {
        IDLE,//�ҋ@
        APPROACH,//�ڋ�
        RECESSION,//���

        HEAVY_ATTACK,//���U��
        LIGHT_ATTACK,//��U��
        LONG_RANGE_ATTACK,//�������U��

        END
    }

    protected struct CoolTimeRandomValue
    {
        float minValue;
        float maxValue;
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
    private bool _isBattle = false;
    /// <summary> ���݂̃N�[���^�C�� </summary>
    private float _coolTimeValue = 0f;
    /// <summary> ���݂̃N�[���^�C�� </summary>
    [Tooltip("�퓬�J�n�܂ł̋���"), SerializeField] private Vector2 _fightStartDistance;
    [Tooltip("�퓬��~�܂ł̋���"), SerializeField] private Vector2 _fightStopDistance;
    //�F�ύX�p
    bool _isColorChange = false;
    float _colorChangeTime = 0;
    /// <summary> �{�X�U����̃N�[���^�C�� </summary>
    Dictionary<BossState, float> _bossAttackCoolTime = new Dictionary<BossState, float>();


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
    protected void BossUpdate()
    {
        
    }

    /// <summary> �����𑪂�A�퓬���邩�ǂ������f���� </summary>
    void DoFight()
    {
        //�v���C���[�Ƃ̋����𑪂鏈���������ɏ���

        //���������ȏ�߂Â�����_isFight��True�ɂ���

        //���������ȏ㗣�ꂽ��_isFight��False�ɂ���
    }

    /// <summary> �{�X�̃X�e�[�g���A�K�v�ɉ����ĕύX���� </summary>
    void CangeState()
    {

    }


    /// <summary> �v���C���[�ƐڐG�����Ƃ��ɌĂ΂�� </summary>
    void HitPlayer()//�v���C���[�̗̑͂����炵�A�m�b�N�o�b�N������B
    {

    }

    /// <summary> �v���C���[�̍U���ɐڐG�����Ƃ��ɌĂ΂�� </summary>
    void HitPlayerAttack()//�����̗̑͂����炵�A�K�v�ł���΃m�b�N�o�b�N����B
    {

    }

    /// <summary> �퓬�J�n���̏��� </summary>
    void BattleStart()
    {

    }

    /// <summary> �퓬�I�����̏��� </summary>
    void BattleExit()
    {

    }
}
