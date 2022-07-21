using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

/// <summary> ���̃N���X�̓v���C���[�̃X�e�[�^�X���Ǘ�����B </summary>
public class PlayerStatusManager : MonoBehaviour
{
    //���̃N���X�̓V���O���g���p�^�[�����g�p�������̂ł���B
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

    /// <summary> �v���C���[�̖��O </summary>
    [SerializeField] string _playerName;
    public string PlayerName { get => _playerName; }
    //�e�p�����[�^
    /// <summary> �v���C���[�̍ő�̗� </summary>
    [Header("�v���C���[�̍ő�̗�"), SerializeField] float _maxPlayerHealthPoint;
    /// <summary> �v���C���[�̍ő�̗� </summary>
    public float PlayerMaxHealthPoint { get => _maxPlayerHealthPoint; set { Debug.Log("�v���C���[�̍ő�̗͂̒l��ύX���܂���"); _maxPlayerHealthPoint = value; } }
    /// <summary> �v���C���[�̗̑� </summary>
    [Header("�v���C���[�̗̑�"), SerializeField] float _playerHealthPoint;
    /// <summary> �v���C���[�̗̑� </summary>
    public float PlayerHealthPoint { get => _playerHealthPoint; set { Debug.Log("�v���C���[�̗̑͂̒l��ύX���܂���"); _playerHealthPoint = value; } }
    /// <summary> �v���C���[�̍ő�X�^�~�i </summary>
    [Header("�v���C���[�̍ő�X�^�~�i"), SerializeField] float _playerMaxStamina;
    /// <summary> �v���C���[�̍ő�X�^�~�i </summary>
    public float PlayerMaxStamina { get => _playerMaxStamina; set => _playerMaxStamina = value; }
    /// <summary> �v���C���[�̃X�^�~�i </summary>
    [Header("�v���C���[�̃X�^�~�i"), SerializeField] float _playerStamina;
    /// <summary> �v���C���[�̃X�^�~�i </summary>
    public float PlayerStamina { get => _playerStamina; set => _playerStamina = value; }
    /// <summary> �v���C���[�̋ߋ����U���� </summary>
    [Header("�v���C���[�̋ߋ����U����"), SerializeField] float _playerShortRangeAttackPower;
    /// <summary> �v���C���[�̋ߋ����U���� </summary>
    public float PlayerShortRangeAttackPower { get => _playerShortRangeAttackPower; set { Debug.Log("�U���͂̒l��ύX���܂���"); _playerShortRangeAttackPower = value; } }
    /// <summary> �v���C���[�̉������U���� </summary>
    [Header("�v���C���[�̉������U����"), SerializeField] float _playerLongRangeAttackPower;
    /// <summary> �v���C���[�̉������U���� </summary>
    public float PlayerLongRangeAttackPower { get => _playerLongRangeAttackPower; set { Debug.Log("�U���͂̒l��ύX���܂���"); _playerLongRangeAttackPower = value; } }
    /// <summary> �v���C���[�̖h��� </summary>
    [Header("�v���C���[�̖h���"), SerializeField] float _playerDefensePower;
    /// <summary> �v���C���[�̖h��� </summary>
    public float PlayerDefensePower { get => _playerDefensePower; set { Debug.Log("�h��͂̒l��ύX���܂���"); _playerDefensePower = value; } }
    /// <summary> �v���C���[�̈ړ��� </summary>
    [Header("�v���C���[�̈ړ���"), SerializeField] float _playerMoveSpeed;
    /// <summary> �v���C���[�̈ړ��� </summary>
    public float PlayerMoveSpeed { get => _playerMoveSpeed; set { Debug.Log("�v���C���[�̈ړ��͂̒l��ύX���܂���"); _playerMoveSpeed = value; } }
    /// <summary> �v���C���[�̐�����тɂ��� </summary>
    [Header("�v���C���[�̐�����тɂ���"), SerializeField] float _playerDifficultToBlowOff;
    /// <summary> �v���C���[�̐�����тɂ��� </summary>
    public float PlayerDifficultToBlowOff { get => _playerDifficultToBlowOff; set { Debug.Log("�v���C���[�̈ړ��͂̒l��ύX���܂���"); _playerDifficultToBlowOff = value; } }

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
