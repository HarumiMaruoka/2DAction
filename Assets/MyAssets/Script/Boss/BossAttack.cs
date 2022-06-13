using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    //�v���C���[�̃R���|�[�l���g
    GameObject _playerObjedt;
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


    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        //�v���C���[�̃R���|�[�l���g���擾
        _playerObjedt = GameObject.Find("ChibiRobo");
        _playerBasicInformation = _playerObjedt.GetComponent<PlayerBasicInformation>();
        _playerPos = _playerObjedt.GetComponent<Transform>();
        _playersRigidBody2D = _playerObjedt.GetComponent<Rigidbody2D>();
        _playerMoveManager = _playerObjedt.GetComponent<PlayerMoveManager>();

        gameObject.SetActive(false);
    }

    void Update()
    { 
        if (transform.parent.GetComponent<SpriteRenderer>().flipX)
        {
            _collider2D.offset = new Vector3(_offset.x,_offset.y);
        }
        else
        {
            _collider2D.offset = new Vector3(-_offset.x, _offset.y);
        }
    }

    //�v���C���[����Ăяo��
    public void HitPlayer()//�v���C���[�̗̑͂����炵�A�m�b�N�o�b�N������B
    {
        //�v���C���[��HitPoint�����炷
        _playerBasicInformation._playerHitPoint -= _offensive_Power;
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

    public void OnCollider()
    {
        _collider2D.enabled = true;
    }

    public void OffCollider()
    {
        _collider2D.enabled = false;
    }
}
