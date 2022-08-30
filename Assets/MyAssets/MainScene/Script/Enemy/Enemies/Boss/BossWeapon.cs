using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Boss�̍U�����Ǘ�����R���|�[�l���g : 
/// �R���C�_�[�������q�I�u�W�F�N�g�Ɏ�������B
/// �������AEnemyBase���p������������������ȁH
/// </summary>
public class BossWeapon : MonoBehaviour
{
    //<=========== �����o�[�ϐ� ===========>//
    //�v���C���[�̃R���|�[�l���g
    GameObject _playerObject;
    protected Transform _playerPos;
    protected PlayerBasicInformation _playerBasicInformation;
    protected Rigidbody2D _playersRigidBody2D;
    protected PlayerMoveManager _playerMoveManager;

    //���g�̃R���|�[�l���g
    Animator _animator;
    Collider2D _collider2D;
    SpriteRenderer _spriteRenderer;

    //�e�X�e�[�^�X
    [Tooltip("�U����"), SerializeField] protected int _offensive_Power;
    [Tooltip("�v���C���[�ɑ΂���m�b�N�o�b�N��"), SerializeField] protected Vector2 _playerKnockBackPower;

    [Tooltip("�R���C�_�[�̃I�t�Z�b�g"), SerializeField] Vector2 _offset;

    [Tooltip("Player�ɐݒ肳�ꂽTag�̖��O"), SerializeField] string _playerTagName = "";

    bool _isInitialized = false;


    //<=========== Unity���b�Z�[�W ===========>//
    void Start()
    {
        if (!(_isInitialized = Initialize_BossAttack()))
        {
            Debug.LogError($"�������Ɏ��s���܂����B{gameObject.name}");
        }
        gameObject.SetActive(false);
    }
    void Update()
    {
        if (transform.parent.GetComponent<SpriteRenderer>().flipX)
        {
            _collider2D.offset = new Vector3(_offset.x, _offset.y);
        }
        else
        {
            _collider2D.offset = new Vector3(-_offset.x, _offset.y);
        }
    }

    //<=========== private�����o�[�֐� ===========>//
    /// <summary> BossAttack�̏������֐� </summary>
    /// <returns> �������ɐ��������� true ��Ԃ��B </returns>
    bool Initialize_BossAttack()
    {
        if (!BossAttackInitialize_Get_PlayerComponents()) return false;
        if (!BossAttackInitialize_Get_ThisGameObjectComponents()) return false;
        return true;
    }
    /// <summary> �v���C���[�ɃA�^�b�`����Ă���R���|�[�l���g���擾����B </summary>
    /// <returns> ���������� true ��Ԃ��B </returns>
    bool BossAttackInitialize_Get_PlayerComponents()
    {
        //�v���C���[�̃R���|�[�l���g���擾
        _playerObject = GameObject.FindGameObjectWithTag(_playerTagName);
        if (_playerObject == null)
        {
            Debug.LogError($"PlayerGameObject�̎擾�Ɏ��s���܂����BPlayer�ɐݒ肳�ꂽTag�� {_playerTagName} �ł����H");
            return false;
        }

        _playerBasicInformation = _playerObject.GetComponent<PlayerBasicInformation>();
        if (_playerBasicInformation == null)
        {
            Debug.LogError($"Player�ɃA�^�b�`���ꂽPlayerBasicInformation�R���|�[�l���g�̎擾�Ɏ��s���܂��� : {gameObject.name}");
            return false;
        }

        _playerPos = _playerObject.GetComponent<Transform>();
        if (_playerPos == null)
        {
            Debug.LogError($"Player�ɃA�^�b�`���ꂽTransform�R���|�[�l���g�̎擾�Ɏ��s���܂��� : {gameObject.name}");
            return false;
        }

        _playersRigidBody2D = _playerObject.GetComponent<Rigidbody2D>();
        if (_playersRigidBody2D == null)
        {
            Debug.LogError($"Player�ɃA�^�b�`���ꂽRigidbody2D�R���|�[�l���g�̎擾�Ɏ��s���܂��� : {gameObject.name}");
            return false;
        }

        _playerMoveManager = _playerObject.GetComponent<PlayerMoveManager>();
        if (_playerMoveManager == null)
        {
            Debug.LogError($"Player�ɃA�^�b�`���ꂽPlayerMoveManager�R���|�[�l���g�̎擾�Ɏ��s���܂��� : {gameObject.name}");
            return false;
        }

        return true;
    }
    /// <summary> ���̃Q�[���I�u�W�F�N�g�ɃA�^�b�`����Ă���R���|�[�l���g���擾����B </summary>
    /// <returns> ���������� true ��Ԃ��B </returns>
    bool BossAttackInitialize_Get_ThisGameObjectComponents()
    {
        _collider2D = GetComponent<Collider2D>();
        if (_collider2D == null)
        {
            Debug.LogError($"Collider�R���|�[�l���g�̎擾�Ɏ��s���܂����B : {gameObject.name}");
            return false;
        }
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogWarning($"SpriteRenderer�R���|�[�l���g�̎擾�Ɏ��s���܂����B \n ���̃N���X��SpriteRenderer�R���|�[�l���g���g�p���܂����H : �I�u�W�F�N�g�� \"{gameObject.name}\"");
        }
        _animator = GetComponent<Animator>();
        if (_animator == null)
        {
            Debug.LogWarning($"Animator�R���|�[�l���g�̎擾�Ɏ��s���܂����B \n ���̃N���X��Animator�R���|�[�l���g���g�p���܂����H : �I�u�W�F�N�g�� \" {gameObject.name}\"");
        }
        return true;
    }




    //<=========== public�����o�[�֐� ===========>//
    //�v���C���[����Ăяo��
    public void HitPlayer()//�v���C���[�̗̑͂����炵�A�m�b�N�o�b�N������B
    {
        //�v���C���[��HitPoint�����炷
        PlayerStatusManager.Instance.PlayerHealthPoint -= _offensive_Power;
        _playersRigidBody2D.velocity = Vector2.zero;
        //�v���C���[���m�b�N�o�b�N����
        if (transform.parent.GetComponent<SpriteRenderer>().flipX)
        {
            _playersRigidBody2D.velocity = Vector2.zero;
            _playersRigidBody2D.AddForce((Vector2.right + Vector2.up) * _playerKnockBackPower, ForceMode2D.Impulse);
        }
        else
        {
            _playersRigidBody2D.velocity = Vector2.zero;
            _playersRigidBody2D.AddForce((Vector2.left + Vector2.up) * _playerKnockBackPower, ForceMode2D.Impulse);
        }
    }

    //<=== �ȉ��A�j���[�V�����C�x���g����Ăяo���H������̌��\�O�ŖY��Ă��܂����B�v�m�F�B ===>//
    public void OnCollider()
    {
        _collider2D.enabled = true;
    }

    public void OffCollider()
    {
        _collider2D.enabled = false;
    }
}
