using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveManager : MonoBehaviour
{
    //�p�����[�^
    [Tooltip("���ړ����x")]
    [SerializeField] float _moveSpeedX;
    public float MoveSpeedX { get => _moveSpeedX; }

    [Tooltip("�_�b�V�����̉����x")]
    [SerializeField] float _moveDashSpeed;
    public float MoveDashSpeed { get => _moveDashSpeed; }

    [Tooltip("�W�����v��")]
    [SerializeField] float _jumpPower;
    public float JumpPower { get => _jumpPower; }

    [Tooltip("�z�o�[���̏㏸��")]
    [SerializeField] float _hoverPower;
    public float HoverPower { get => _hoverPower; }

    [Tooltip("�z�o�[���̃K�\��������")]
    [SerializeField] float _gasValue;
    public float GasValue { get => _gasValue; }

    [Tooltip("��q�̏��~���x")]
    [SerializeField] float _climbSpeed;
    public float ClimbSpeed { get => _climbSpeed; }

    [Tooltip("���ړ����x�̌�����")]
    [SerializeField] float _decelerationRateX;
    public float DecelerationRateX { get => _decelerationRateX; }

    [Tooltip("�X���C�f�B���O���x")]
    [SerializeField] float _slidingSpeed;
    public float SlidingSpeed { get => _slidingSpeed; }

    //�R���|�[�l���g
    Rigidbody2D _rigidBody2D;
    InputManager _inputManager;
    JumpScript _jumpScript;
    SpriteRenderer _spriteRendere;
    NewPlayerStateManagement _newPlayerStateManagement;
    PlayerBasicInformation _playerBasicInformation;
    Animator _animator;

    //���̃t���[���ŉ������
    Vector2 _newForce;
    public Vector2 _newImpulse;
    Vector2 _newVelocity;

    //�W�����v�ł��邩:�X�y�[�X�L�[�̓W�����v�ƃX���C�f�B���O�̋@�\�����˂��
    bool _canJump = false;
    //�v���C���[�������Ă������
    bool _isRigth = false;
    //��q��o��邩�ǂ���
    bool _isClimb = false;
    //�d��:��q��o���Ă���Ƃ��͏d�͂�0�ɂ��邽��
    float _gravity;
    //�W�����v�������ǂ����̊m�F
    public bool _isJump { get; private set; } = false;


    void Start()
    {
        //�R���|�[�l���g�̏�����
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _inputManager = GetComponent<InputManager>();
        _jumpScript = GetComponent<JumpScript>();
        _spriteRendere = GetComponent<SpriteRenderer>();
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();
        _newPlayerStateManagement = GetComponent<NewPlayerStateManagement>();
        _animator = GetComponent<Animator>();
        _gravity = _rigidBody2D.gravityScale;

        //�e�ϐ��̏�����
        _newForce = Vector2.zero;
        _newImpulse = Vector2.zero;
        _newVelocity = Vector2.zero;
        //�v���C���[�������Ă���������擾
        _isRigth = !_spriteRendere.flipX;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {

        //������͂̏�����
        _newForce = Vector2.zero;
        _newImpulse = Vector2.zero;
        _newVelocity = Vector2.zero;
        if (_newPlayerStateManagement._isMove && !_newPlayerStateManagement._isDead)
        {
            //�v���C���[�������Ă���������擾
            _isRigth = !_spriteRendere.flipX;

            //�ړ�����
            MoveHorizontal();
            Dash();
            Sliding();
            Jump();
            Climb();
            Hover();

            //�󒆂ł͉��ړ����x���x���Ȃ�
            if (!_jumpScript.GetIsGround())
            {
                _newForce.x *= DecelerationRateX;
            }

            //���ۂɗ͂�������
            _rigidBody2D.AddForce(_newImpulse * 10f, ForceMode2D.Impulse);
            _rigidBody2D.AddForce(_newForce * 10f * Time.deltaTime * 100f, ForceMode2D.Force);
            if (Mathf.Approximately(_newImpulse.x, 0f) && Mathf.Approximately(_newImpulse.y, 0f))
            {
                _rigidBody2D.velocity = new Vector2(_newVelocity.x, _rigidBody2D.velocity.y);
            }

        }
    }

    void MoveHorizontal()
    {
        //�c�̓��͂����鎞�͎��s�ł��Ȃ�
        if (!(_inputManager._inputVertical != 0))
        {
            _newVelocity += new Vector2(_inputManager._inputHorizontal * MoveSpeedX, 0f);
        }
    }

    void Dash()
    {
        //LeftShift�L�[�ŉ���
        if (_inputManager._inputLeftShift)
        {
            _newVelocity *= _moveDashSpeed;
        }
    }

    void Sliding()
    {
        _canJump = true;
        //�ڒn����S�L�[�Ŏ��s�\
        if (_inputManager._inputVertical < 0 && _jumpScript.GetIsGround())
        {
            //��L�������N���A���������ŁA�X�y�[�X�L�[�������ꂽ�ꍇ�X���C�f�B���O����I
            if (_inputManager._inputJumpDown)
            {
                _newImpulse += _isRigth ? new Vector2(_slidingSpeed, 0f) : new Vector2(-_slidingSpeed, 0f);
                _canJump = false;//�X���C�f�B���O����ꍇ�̓W�����v�ł��Ȃ�
                _newPlayerStateManagement._isMove = false;
            }
        }
    }

    void Jump()
    {
        //�ڒn���X�y�[�X�L�[�ŃW�����v
        if (_jumpScript.GetIsGround() && _inputManager._inputJumpDown && _canJump)
        {
            Debug.Log("Jump");
            _newImpulse += new Vector2(0f, JumpPower);
            _isJump = true;
        }
        else if (_isJump)
        {
            _isJump = false;
        }
    }

    void Climb()
    {
        //��q�����~���鎞�̏���
        if (_isClimb)
        {
            //�c�̓��͂����鎞
            if (_inputManager._inputVertical != 0)
            {
                //���ɓ��͂����Ƃ�
                //��ڒn��ԂȂ�~����
                if (_inputManager._inputVertical < 0 && !_jumpScript.GetIsGround())
                {
                    _rigidBody2D.velocity = Vector2.up * ClimbSpeed * _inputManager._inputVertical;
                }
                //��ɓ��͂����Ƃ�
                else if (_inputManager._inputVertical > 0)
                {
                    _rigidBody2D.velocity = Vector2.up * ClimbSpeed * _inputManager._inputVertical;
                }
            }
            //�c�̓��͂��Ȃ��Ȃ������̏���
            else
            {
                _rigidBody2D.velocity = Vector2.zero;
                _rigidBody2D.gravityScale = 0f;
            }
        }
    }

    void Hover()
    {
        //�z�o�[�̏����B
        if (_newPlayerStateManagement._playerState == NewPlayerStateManagement.PlayerState.HOVER)
        {
            //�z�o�[�p�̗͂�0���傫�����
            if (_playerBasicInformation._hoverValue > 0f)
            {
                //�㏸�����A�K�X�������
                _playerBasicInformation._hoverValue -= Time.deltaTime * _gasValue;
                _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, _hoverPower);
            }
        }
        //�z�o�[���ĂȂ��Ƃ��̏����B
        else
        {
            //�̗͂��ő�ł����
            if (_playerBasicInformation._maxHealthForHover <= _playerBasicInformation._hoverValue)
            {
                _playerBasicInformation._hoverValue = _playerBasicInformation._maxHealthForHover;
            }
            //�̗͂��ő�ł͂Ȃ����
            else
            {
                _playerBasicInformation._hoverValue += Time.deltaTime * _gasValue;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //��q�ƐڐG���Ă���Ƃ��̏���
        if (collision.tag == "Ladder")
        {
            //��q��o��ꍇ
            if (_inputManager._inputVertical != 0)
            {
                _isClimb = true;
            }
        }
    }

    //�R���C�_�[���ڐG���Ă���Ƃ��̏���
    private void OnTriggerExit2D(Collider2D collision)
    {
        //��q�ƐڐG���Ă���Ƃ��̏���
        if (collision.tag == "Ladder")
        {
            _isClimb = false;
            _rigidBody2D.gravityScale = _gravity;
        }
    }

    //���̊֐��̓A�j���[�V�����C�x���g����Ăяo��
    /// <summary> �X���C�f�B���O�I���֐� </summary>
    public void SlidingStop()
    {
        _newPlayerStateManagement._isMove = true;
        _animator.SetTrigger("SlidingOff");
    }
}
