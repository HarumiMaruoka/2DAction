using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

/// <summary> ���̃N���X�̓v���C���[�̃X�e�[�^�X���Ǘ�����B </summary>
public class PlayerStatusManager : MonoBehaviour
{
    //<======== ���̃N���X�Ŏg�p����^ ========>//
    [System.Serializable]
    public struct PlayerStatus
    {
        /// <summary> ���O </summary>
        public string _playerName;
        /// <summary> �ő�̗� </summary>
        public float _maxHp;
        /// <summary> �ő�X�^�~�i </summary>
        public float _maxStamina;
        /// <summary> �ߋ����U���� </summary>
        public float _shortRangeAttackPower;
        /// <summary> �������U���� </summary>
        public float _longRangeAttackPower;
        /// <summary> �h��� </summary>
        public float _defensePower;
        /// <summary> �ړ����x </summary>
        public float _moveSpeed;
        /// <summary> ������тɂ��� </summary>
        public float _difficultToBlowOff;


        static public PlayerStatus Zero = new PlayerStatus();

        //�R���X�g���N�^
        public PlayerStatus(string name = "", float hp = 0f, float stamina = 0f, float shortAttackPow = 0f, float longAttackPow = 0f, float defensePow = 0f, float moveSpeed = 0f, float difficultToBlowOff = 0f)
        {
            _playerName = name;
            _maxHp = hp;
            _maxStamina = stamina;
            _shortRangeAttackPower = shortAttackPow;
            _longRangeAttackPower = longAttackPow;
            _defensePower = defensePow;
            _moveSpeed = moveSpeed;
            _difficultToBlowOff = difficultToBlowOff;
        }
        public static PlayerStatus _zero
        {
            get => new PlayerStatus();
        }

        /// <summary> PlayerStatus�^ : +���Z�q�̃I�[�o�[���[�h </summary>
        public static PlayerStatus operator +(PlayerStatus p1, PlayerStatus p2)
        {
            p1._maxHp += p2._maxHp;
            p1._maxStamina += p2._maxStamina;
            p1._shortRangeAttackPower += p2._shortRangeAttackPower;
            p1._longRangeAttackPower += p2._longRangeAttackPower;
            p1._defensePower += p2._defensePower;
            p1._moveSpeed += p2._moveSpeed;
            p1._difficultToBlowOff += p2._difficultToBlowOff;
            return p1;
        }
        /// <summary> PlayerStatus�^ : -���Z�q�̃I�[�o�[���[�h </summary>
        public static PlayerStatus operator -(PlayerStatus p1, PlayerStatus p2)
        {
            p1._maxHp -= p2._maxHp;
            p1._maxStamina -= p2._maxStamina;
            p1._shortRangeAttackPower -= p2._shortRangeAttackPower;
            p1._longRangeAttackPower -= p2._longRangeAttackPower;
            p1._defensePower -= p2._defensePower;
            p1._moveSpeed -= p2._moveSpeed;
            p1._difficultToBlowOff -= p2._difficultToBlowOff;
            return p1;
        }
    }
    //<===== �V���O���g���p�^�[���֘A =====>//
    //�C���X�^���X
    private static PlayerStatusManager _instance;
    //�C���X�^���X�͓ǂݎ���p
    public static PlayerStatusManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PlayerManager._instance��null�ł��B");
            }
            return _instance;
        }
    }
    //�v���C�x�[�g�ȃR���X�g���N�^���`����B
    private PlayerStatusManager() { }

    // <=========== �����o�[�ϐ� ===========> //
    /// <summary> ��b�X�e�[�^�X </summary>
    [Header("�v���C���[�̊�b�X�e�[�^�X"), SerializeField] PlayerStatus _baseStatus;
    /// <summary> ��b�X�e�[�^�X </summary>
    public PlayerStatus BaseStatus { get => _baseStatus; set => _baseStatus = value; }
    /// <summary> �������̏㏸�l </summary>
    [Header("�m�F�p : �������̏㏸�l"), SerializeField] PlayerStatus _equipment_RisingValue;
    /// <summary> �������̏㏸�l </summary>
    public PlayerStatus Equipment_RisingValue { get => _equipment_RisingValue; set => _equipment_RisingValue = value; }
    /// <summary> ���x�����̏㏸�l </summary>
    [Header("�m�F�p : ���x�����̏㏸�l"), SerializeField] PlayerStatus _level_RisingValue;
    /// <summary> ���x�����̏㏸�l </summary>
    public PlayerStatus Level_RisingValue { get=> _level_RisingValue; set=> _level_RisingValue=value; }
    /// <summary> ���̑�(�A�C�e���g�p���̈ꎞ�I�ȏ㏸�l��)�̏㏸�l </summary>
    [Header("���̑�(�A�C�e���g�p���̈ꎞ�I�ȏ㏸�l��)�̏㏸�l"), SerializeField] PlayerStatus _other_RisingValue;
    /// <summary> ���̑�(�A�C�e���g�p���̈ꎞ�I�ȏ㏸�l��)�̏㏸�l </summary>
    public PlayerStatus Other_RisingValue { get=> _other_RisingValue; set=> _other_RisingValue=value; }
    /// <summary> ���X�����v�����A�ŏI�I�ȃX�e�[�^�X </summary>
    public PlayerStatus ConsequentialPlayerStatus { get => _baseStatus + Equipment_RisingValue + Level_RisingValue + Other_RisingValue; }

    [Header("�v���C���[�̌��݂̗̑�"), SerializeField] float _playerHealthPoint;
    /// <summary> �v���C���[�̌��݂̗̑� </summary>
    public float PlayerHealthPoint { get => _playerHealthPoint; set => _playerHealthPoint = value; }

    [Header("�v���C���[�̌��݂̃X�^�~�i"), SerializeField] float _playerStamina;
    /// <summary> �v���C���[�̌��݂̃X�^�~�i </summary>
    public float PlayerStamina { get => _playerStamina; set => _playerStamina = value; }

    /// <summary> �v���C���[�������Ă������ </summary>
    private bool _isRight;
    /// <summary> �v���C���[�������Ă������ </summary>
    public bool IsRight { get => _isRight; }

    private void Awake()
    {
        //�����C���X�^���X���ݒ肳��Ă��Ȃ������玩�g��������
        if (_instance == null)
        {
            _instance = this;
        }
        //�������ɑ��݂���ꍇ�́A���̃I�u�W�F�N�g��j������B
        else if (_instance != null)
        {
            Destroy(this);
        }
        //���̃I�u�W�F�N�g�́A�V�[�����ׂ��ł��f�X�g���C���Ȃ��B
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Debug.Log("PlayerManager�̏������ɐ������܂����B");
    }

    //Update�ł̓e�X�g�ŃA�C�e�����������Z�[�u�����胍�[�h�����肷�鏈���������Ă���B
    private void Update()
    {
        //�v���C���[�������Ă��������ۑ�����
        if (Input.GetAxisRaw("Horizontal") < 0)
        {
            _isRight = true;
        }
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            _isRight = false;
        }
    }
}
