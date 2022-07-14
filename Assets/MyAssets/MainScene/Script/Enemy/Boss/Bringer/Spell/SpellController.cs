using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellController : MonoBehaviour
{
    //�v���C���[�̃R���|�[�l���g
    GameObject _playerObjedt;
    protected Transform _playerPos;
    protected PlayerBasicInformation _playerBasicInformation;
    protected Rigidbody2D _playersRigidBody2D;
    protected PlayerMoveManager _playerMoveManager;

    //���g�̃R���|�[�l���g
    Animator _animator;
    BoxCollider2D _collider2D;
    SpriteRenderer _spriteRenderer;

    //�e�X�e�[�^�X
    [Tooltip("�U����"), SerializeField] protected int _offensivePower;
    [Tooltip("�v���C���[�ɑ΂���m�b�N�o�b�N��"), SerializeField] protected Vector2 _playerKnockBackPower;
    [Tooltip("����"), SerializeField] float _height;
    [Tooltip("�X�y����������ׂ��ʒu"), SerializeField] protected Vector2 _fallPos;


    void Start()
    {
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<BoxCollider2D>();
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
        transform.position = _fallPos;
        //������ݒ�:�e�̌������擾
        _spriteRenderer.flipX = transform.parent.GetComponent<SpriteRenderer>().flipX;
    }

    /// <summary> �X�y���������̏��� </summary>
    public void PlaySpell()
    {
        //�A�j���[�V�������Đ�
        _animator.SetTrigger("Spell");

        //�ʒu���v���C���[�̓���ɒ���
        _fallPos = _playerPos.position + (Vector3.up * _height);
    }


    //�A�j���[�V�����C�x���g����Ăяo��
    /// <summary> ����������u�Ԃ̏����B�R���C�_�[���I���ɂ��� </summary>
    public void LightningFalls()
    {
        //�R���C�_�[��On�ɂ���
        _collider2D.enabled = true;
    }

    /// <summary> ����������Ƃ��̏����B�R���C�_�[���I�t�ɂ��� </summary>
    public void LightningDisappears()
    {
        _collider2D.enabled = false;
    }

    /// <summary> �X�y���I�����̏��� </summary>
    public void EndSpell()
    {
        gameObject.SetActive(false);
    }

    //�v���C���[����Ăяo��
    public void HitPlayer()//�v���C���[�̗̑͂����炵�A�m�b�N�o�b�N������B
    {
        //�v���C���[��HitPoint�����炷
        PlayerStatusManager.Instance.PlayerHealthPoint -= _offensivePower;
        _playersRigidBody2D.velocity = Vector2.zero;
        //�v���C���[���m�b�N�o�b�N����
        if (_spriteRenderer.flipX)
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
}
