using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Enemy�̊��N���X </summary>
public class EnemyBase : MonoBehaviour
{
    //<============= �����o�[�ϐ� =============>//
    //�G�l�~�[���ʂ̃X�e�[�^�X
    [SerializeField] protected EnemyStatus _status;
    public EnemyStatus Status { get => _status; }

    /// <summary> �h���b�v�i�Ɗm���̏����i�[����ϐ��B </summary>
    [Header("�h���b�v�i�Ɗm��"), SerializeField]
    protected DropItemAndProbability[] _dropItemAndProbabilities;

    //���̃G�l�~�[�������Ă������
    protected bool _isRight;

    //�v���C���[�̃R���|�[�l���g
    protected GameObject _player;
    protected Transform _playerPos;
    protected PlayerBasicInformation _playerBasicInformation;
    protected Rigidbody2D _playersRigidBody2D;
    protected PlayerMoveManager _playerMoveManager;

    //���g�̃R���|�[�l���g
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rigidBody2d;

    //�F�ύX�p
    /// <summary> �F��ς��邩�ǂ��� </summary>
    protected bool _isColorChange = false;
    /// <summary> �F��ύX���鎞�� </summary>
    const float _colorChangeTime = 0.1f;
    /// <summary> �F��ύX���鎞�� : ���݂̎c�莞�� </summary>
    protected float _colorChangeTimeValue = 0;

    //�m�b�N�o�b�N�֘A
    /// <summary> �m�b�N�o�b�N���� </summary>
    float _knockBackModeTime = 0f;
    protected bool _isMove = true;


    //<============== protected�����o�[�֐� ==============>//
    /// <summary> �S�G�l�~�[�ŋ��ʂ�Enemy�̏������֐��B�p�����Start�֐��ŌĂяo���B </summary>
    /// <returns> ���������� true ��Ԃ��B </returns>
    protected bool Initialize_Enemy()
    {
        if (!EnemyInitialize_Get_PlayerComponents())
        {
            Debug.LogError($"�������Ɏ��s���܂����B : {gameObject.name}");
            return false;
        }
        if (!EnemyInitialize_Get_ThisGameObjectComponents())
        {
            Debug.LogError($"�������Ɏ��s���܂����B : {gameObject.name}");
            return false;
        }

        return true;
    }
    protected virtual void Update()
    {
        Update_Enemy();
    }
    //�S�G�l�~�[�ŋ��ʂ�Enemy��Update�֐��B�p�����Update�֐��ŌĂяo��
    protected void Update_Enemy()
    {
        //�F��ς���K�v������Ες���
        if (_isColorChange)
        {
            _spriteRenderer.color = Color.red;
        }
        //�F�����ɖ߂�
        else
        {
            _spriteRenderer.color = new Color(255, 255, 255, 255);
        }
    }
    protected void StartKnockBack(float knockBackPower)
    {
        _rigidBody2d.velocity = Vector2.zero;
        _rigidBody2d.AddForce((Vector2.up + Vector2.right) * knockBackPower, ForceMode2D.Impulse);
    }
    //<============= private�֐� =============>//
    //***** �������֘A *****//
    /// <summary> �v���C���[�ɃA�^�b�`����Ă���R���|�[�l���g���擾����B </summary>
    /// <returns> ���������� true ��Ԃ��B </returns>
    bool EnemyInitialize_Get_PlayerComponents()
    {
        //�v���C���[�̏����擾
        _player = GameObject.Find("ChibiRobo");
        if (_player == null)
        {
            Debug.LogError($"Player���擾�ł��܂���ł����B : {gameObject.name}");
            return false;
        }
        _playerBasicInformation = _player.GetComponent<PlayerBasicInformation>();
        if (_playerBasicInformation == null)
        {
            Debug.LogError($"Player��PlayerBasicInformation�R���|�[�l���g���擾�ł��܂���ł����B : {gameObject.name}");
            return false;
        }
        _playerPos = _player.GetComponent<Transform>();
        if (_playerPos == null)
        {
            Debug.LogError($"Player��Transform�R���|�[�l���g���擾�ł��܂���ł����B : {gameObject.name}");
            return false;
        }
        _playersRigidBody2D = _player.GetComponent<Rigidbody2D>();
        if (_playersRigidBody2D == null)
        {
            Debug.LogError($"Player��_playersRigidBody2D�R���|�[�l���g���擾�ł��܂���ł����B : {gameObject.name}");
            return false;
        }
        _playerMoveManager = _player.GetComponent<PlayerMoveManager>();
        if (_playerMoveManager == null)
        {
            Debug.LogError($"Player��PlayerMoveManager�R���|�[�l���g���擾�ł��܂���ł����B : {gameObject.name}");
            return false;
        }

        return true;
    }
    /// <summary> ���̃Q�[���I�u�W�F�N�g�ɃA�^�b�`����Ă���R���|�[�l���g���擾����B </summary>
    /// <returns> ���������� true ��Ԃ��B </returns>
    bool EnemyInitialize_Get_ThisGameObjectComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer�̎擾�Ɏ��s���܂����B : {gameObject.name}");
            return false;
        }
        _rigidBody2d = GetComponent<Rigidbody2D>();
        if (_rigidBody2d == null)
        {
            Debug.LogError($"Rigidbody2D�̎擾�Ɏ��s���܂����B : {gameObject.name}");
            return false;
        }

        return true;
    }


    //<============= public�����o�[�֐� =============>//
    //***** �U���q�b�g�֘A *****//
    /// <summary> �v���C���[����̍U������ : �m�b�N�o�b�N������ </summary>
    /// <param name="playerOffensivePower"> �_���[�W�� </param>
    public void HitPlayerAttack(float playerOffensivePower)
    {
        //���g�̗̑͂����炵�A��莞�� �F���_���p�̐F�ɕς���B
        _status._hitPoint -= playerOffensivePower;//�̗͂��Ȃ��Ȃ������̏���
        if (_status._hitPoint <= 0)
        {
            //�̗͂��Ȃ��Ȃ�������ł���
            Destroy(this.gameObject);
        }
        StartCoroutine(ColorChange());
        _colorChangeTimeValue = _colorChangeTime;
    }
    /// <summary> �v���C���[����̍U������ : �m�b�N�o�b�N�L��� </summary>
    /// <param name="playerOffensivePower"> �_���[�W�� </param>
    /// <param name="knockBackTimer"> �m�b�N�o�b�N���� </param>
    public void HitPlayerAttack(float playerOffensivePower, float knockBackTimer, float knockBackPower)
    {
        //���g�̗̑͂����炵�A0.1�b�����F��Ԃɕς���B
        _status._hitPoint -= playerOffensivePower;
        if (_status._hitPoint <= 0)
        {
            //�̗͂��Ȃ��Ȃ�������ł���
            Destroy(this.gameObject);
        }
        StartCoroutine(ColorChange());
        _colorChangeTimeValue = _colorChangeTime;

        //�w�肳�ꂽ���Ԃ����ړ����~����B
        _knockBackModeTime = (knockBackTimer - _status._weight) > 0f ? (knockBackTimer - _status._weight) : 0f;
        StartCoroutine(MoveStop());
        //�m�b�N�o�b�N�����B
        StartKnockBack((knockBackPower - _status._weight) > 0f ? knockBackPower - _status._weight : 0f);
    }
    /// <summary> �v���C���[�ɑ΂���U������ : �I�[�o�[���C�h�� </summary>
    public virtual void HitPlayer()
    {
        //�v���C���[��HitPoint�����炷
        PlayerStatusManager.Instance.PlayerHealthPoint -= _status._offensivePower;
        _playersRigidBody2D.velocity = Vector2.zero;
        //�v���C���[���m�b�N�o�b�N����
        if (_isRight)
        {
            _playersRigidBody2D.AddForce((Vector2.right + Vector2.up) * _status._blowingPower, ForceMode2D.Impulse);
        }
        else
        {
            _playersRigidBody2D.AddForce((Vector2.left + Vector2.up) * _status._blowingPower, ForceMode2D.Impulse);
        }
    }

    //<============= �R���[�`�� =============>//
    /// <summary> �m�b�N�o�b�N���s�p�R���[�`���B : �m�b�N�o�b�N�����ǂ�����\���ϐ�����莞�� true �ɂ���B </summary>
    IEnumerator MoveStop()
    {
        _isMove = false;
        yield return new WaitForSeconds(_knockBackModeTime);
        _isMove = true;
    }
    /// <summary> �v���C���[����U�����󂯂�ƁA�w�肳�ꂽ���Ԃ����F���ς��B </summary>
    IEnumerator ColorChange()
    {
        _isColorChange = true;
        yield return new WaitForSeconds(_colorChangeTime);
        _isColorChange = false;
    }

    //<============= ���z�֐� =============>//
    /// <summary> Enemy�ړ��p�֐� : �I�[�o�[���C�h�� </summary>
    protected virtual void Move() { }
}
/// <summary> Enemy�̃X�e�[�^�X��\���^ </summary>
[System.Serializable]
public struct EnemyStatus
{
    /// <summary> �̗� </summary>
    public float _hitPoint;
    /// <summary> �U���� </summary>
    public float _offensivePower;
    /// <summary> ������΂���/���� </summary>
    public Vector2 _blowingPower;
    /// <summary> ������тɂ���/�d�� </summary>
    public float _weight;

    /// <summary> EnemyStatus�̃R���X�g���N�^ </summary>
    /// <param name="hitPoint"> �̗� </param>
    /// <param name="offensivePower"> �U���� </param>
    /// <param name="blowingPowerUp"> ������΂��� : ������̈З� </param>
    /// <param name="blowingPowerRight"> ������΂��� : �E�����̈З� </param>
    /// <param name="weight"> �d�� : ������тɂ��� </param>
    public EnemyStatus(int hitPoint = 1, int offensivePower = 1, int blowingPowerUp = 1, int blowingPowerRight = 1, float weight = 1f)
    {
        _hitPoint = hitPoint;
        _offensivePower = offensivePower;
        _blowingPower = Vector2.up * blowingPowerUp + Vector2.right * blowingPowerRight;
        _weight = weight;
    }
}

/// <summary>
/// ���̌^�́A�|����A�󂹂���̂����ׂ��\���́B
/// ���Ƃ��A�C�e���A���Ƃ������A�h���b�v���ϐ��������o�[�Ɏ��B
/// </summary>
[System.Serializable]
public struct DropItemAndProbability
{
    /// <summary> ���Ƃ��A�C�e�� </summary>
    Item.ItemID _itemID;
    /// <summary> ���Ƃ����� </summary>
    EquipmentDataBase.EquipmentID _equipmentID;
    /// <summary> �h���b�v��(%) </summary>
    float _probability;
}