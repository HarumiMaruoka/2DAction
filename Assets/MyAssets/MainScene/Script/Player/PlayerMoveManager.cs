using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveManager : MonoBehaviour
{
    //�p�����[�^
    [Header("�e�X�e�[�^�X")]
    [Tooltip("���ړ����x"), SerializeField]
    float _moveSpeedX;
    public float MoveSpeedX { get => _moveSpeedX; }

    [Tooltip("�_�b�V�����̉����x"), SerializeField]
    float _moveDashSpeed;
    public float MoveDashSpeed { get => _moveDashSpeed; }

    [Tooltip("�W�����v��"), SerializeField]
    float _jumpPower;
    public float JumpPower { get => _jumpPower; }

    [Tooltip("�z�o�[���̏㏸��"), SerializeField]
    float _hoverPower;
    public float HoverPower { get => _hoverPower; }

    [Tooltip("�z�o�[���̃K�\���������"), SerializeField]
    float _gasConsumptionValue;
    public float GasConsumptionValue { get => _gasConsumptionValue; }


    [Tooltip("�z�o�[�p�̃K�\�����񕜗�"), SerializeField]
    float _gasRecoveryValue;
    public float GasRecoveryValue { get => _gasRecoveryValue; }

    [Tooltip("��q�̏��~���x"), SerializeField]
    float _climbSpeed;
    public float ClimbSpeed { get => _climbSpeed; }

    [Tooltip("���ړ����x�̌�����"), SerializeField]
    float _decelerationRateX;
    public float DecelerationRateX { get => _decelerationRateX; }

    [Tooltip("�X���C�f�B���O���x"), SerializeField]
    float _slidingSpeed;
    public float SlidingSpeed { get => _slidingSpeed; }

    //�R���|�[�l���g
    Rigidbody2D _rigidBody2D;
    InputManager _inputManager;
    SpriteRenderer _spriteRendere;
    PlayerStateManagement _playerStateManagement;
    PlayerBasicInformation _playerBasicInformation;
    Animator _animator;

    //���̃t���[���ŉ������
    Vector2 _newForce;
    Vector2 _newImpulse;
    Vector2 _newVelocity;

    /// <summary> �W�����v�ł��邩:�X�y�[�X�L�[�́A�W�����v�ƃX���C�f�B���O�̋@�\�����˂�� </summary>
    bool _canJump = false;
    //�v���C���[�������Ă������
    bool _isRigth = false;
    //��q��o��邩�ǂ���
    bool _isClimb = false;
    //�d��:��q��o���Ă���Ƃ��͏d�͂�0�ɂ��邽��
    float _gravity;
    //�W�����v�������ǂ����̊m�F
    public bool _isJump { get; private set; } = false;

    //�E�ɉ������邩����p
    [Tooltip("�E�ɉ������Ȃ�������p"), SerializeField]
    private Vector3 _overLapBoxOffsetRight;
    //���ɉ������邩����p
    [Tooltip("���ɉ������Ȃ�������p"), SerializeField]
    private Vector3 _overLapBoxOffsetLeft;
    //��L�̃T�C�Y
    [Tooltip("��L�̃T�C�Y"), SerializeField]
    private Vector2 _overLapBoxSizeVertical;
    /// <summary> gizmo�\�� </summary>
    [SerializeField]
    LayerMask _layerMaskCheckLR;
    /// <summary> gizmo�\�� </summary>
    [SerializeField] bool _isGizmo = false;

    //Jump�֘A
    [SerializeField]
    private Vector2 _overLapBoxCenter;
    [SerializeField]
    private Vector2 _overLapBoxSize;
    [SerializeField]
    LayerMask _layerMaskGround;

    float _groundCheckPositionY = -0.725f;

    // �|�[�Y�p
    float _angularVelocity;
    Vector2 _velocity;

    //===== Unity���b�Z�[�W =====//
    void Start()
    {
        //�R���|�[�l���g�̏�����
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _inputManager = GetComponent<InputManager>();
        _spriteRendere = GetComponent<SpriteRenderer>();
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();
        _playerStateManagement = GetComponent<PlayerStateManagement>();
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

    void OnEnable()
    {
        GameManager.OnPause += OnPause;
        GameManager.OnResume += OnResume;
    }
    void OnDisable()
    {
        GameManager.OnPause -= OnPause;
        GameManager.OnResume -= OnResume;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_isGizmo)
        {
            //�E��gizmo
            Gizmos.DrawCube(_overLapBoxOffsetRight + transform.position, _overLapBoxSizeVertical);
            //����gizmo
            Gizmos.DrawCube(_overLapBoxOffsetLeft + transform.position, _overLapBoxSizeVertical);

            Gizmos.DrawCube(_overLapBoxCenter, _overLapBoxSize);
        }
    }
    /// <summary>
    /// �|�[�Y����
    /// </summary>
    void OnPause()
    {
        _playerStateManagement._isPause = true;

        // Rigidbody2D���~������B
        _angularVelocity = _rigidBody2D.angularVelocity;
        _velocity = _rigidBody2D.velocity;
        _rigidBody2D.Sleep();
        _rigidBody2D.simulated = false;
    }
    /// <summary>
    /// �|�[�Y��������
    /// </summary>
    void OnResume() 
    {
        _playerStateManagement._isPause = false;

        // Rigidbody2D�̒�~����������B
        _rigidBody2D.simulated = true;
        _rigidBody2D.WakeUp();
        _rigidBody2D.angularVelocity = _angularVelocity;
        _rigidBody2D.velocity = _velocity;
    }


    //===== private���\�b�h =====//
    /// <summary>
    /// �ړ��̃��C�����\�b�h <br/>
    /// </summary>
    void Move()
    {
        //������͂̏�����
        _newForce = Vector2.zero;
        _newImpulse = Vector2.zero;
        _newVelocity = Vector2.zero;

        if (_playerStateManagement._isMove &&
            !_playerStateManagement._isDead &&
            !_playerStateManagement._isPause)
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
            if (!GetIsGround())
            {
                _newForce.x *= DecelerationRateX;
            }

            //���ۂɗ͂�������
            _rigidBody2D.AddForce(_newImpulse * 10f, ForceMode2D.Impulse);
            _rigidBody2D.AddForce(_newForce * 10f * Time.deltaTime * 100f, ForceMode2D.Force);
            if (Mathf.Approximately(_newImpulse.x, 0f) && Mathf.Approximately(_newImpulse.y, 0f))
            {
                //_newVelocity *= PlayerStatusManager.Instance.ConsequentialPlayerStatus._moveSpeed * 0.01f;
                _newVelocity *=
                    PlayerStatusManager.Instance.BaseStatus._moveSpeed +
                    PlayerStatusManager.Instance.Equipment_RisingValue._moveSpeed * 0.005f;
                _rigidBody2D.velocity = (Vector2.right * _newVelocity) + (Vector2.up * _rigidBody2D.velocity.y);
            }
        }
    }

    /// <summary> ���ړ��̏��� </summary>
    void MoveHorizontal()
    {
        //���ړ��̏���
        if (_inputManager._inputHorizontal < 0 && !BodyContactLeft())//���ɉ����Ȃ���Ύ��s�ł���
        {
            _newVelocity += new Vector2(_inputManager._inputHorizontal * MoveSpeedX, 0f);
        }
        //�E�ړ��̏���
        if (_inputManager._inputHorizontal > 0 && !BodyContactRight())//�E�ɉ����Ȃ���Ύ��s�ł���
        {
            _newVelocity += new Vector2(_inputManager._inputHorizontal * MoveSpeedX, 0f);
        }
    }
    /// <summary> �_�b�V�������� </summary>
    void Dash()
    {
        //LeftShift�L�[�ŉ���
        if (_inputManager._inputLeftShift)
        {
            _newVelocity *= _moveDashSpeed;
        }
    }
    /// <summary> �X���C�f�B���O���� </summary>
    void Sliding()
    {
        _canJump = true;
        //�ڒn����S�L�[�Ŏ��s�\
        if (_inputManager._inputVertical < 0 && GetIsGround())
        {
            //��L�������N���A���������ŁA�X�y�[�X�L�[�������ꂽ�ꍇ�X���C�f�B���O����I
            if (_inputManager._inputJumpDown)
            {
                _newImpulse += _isRigth ? new Vector2(_slidingSpeed, 0f) : new Vector2(-_slidingSpeed, 0f);
                _canJump = false;//�W�����v�͎��s���Ȃ�
                _playerStateManagement._isSlidingNow = true;//���݃X���C�f�B���O���ł��邱�Ƃ�\��
            }
        }
    }
    /// <summary> �W�����v���� </summary>
    void Jump()
    {
        //�ڒn���X�y�[�X�L�[�ŃW�����v
        if (GetIsGround() && _inputManager._inputJumpDown && _canJump)
        {
            _newImpulse += new Vector2(0f, JumpPower);
            _isJump = true;//���̃t���[���̓W�����v�ł��邱�Ƃ�\���B���̕ϐ������݂��闝�R:�z�o�[���Ă��܂��ׁB
        }
        else if (_isJump)
        {
            _isJump = false;
        }
    }
    /// <summary> ��q�̏��~���� </summary>
    void Climb()
    {
        //��q�����~���鎞�̏���
        if (_isClimb)
        {
            //�c�̓��͂����鎞
            if (_inputManager._inputVertical != 0 && !(_inputManager._inputFire1 || _inputManager._inputFire2))
            {
                //���ɓ��͂����Ƃ�
                //��ڒn��ԂȂ�~����
                if (_inputManager._inputVertical < 0 && !GetIsGround())
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
    /// <summary> �z�o�[������ </summary>
    void Hover()
    {
        //�z�o�[�̏����B
        if (_playerStateManagement._playerState == PlayerStateManagement.PlayerState.HOVER)
        {
            //�z�o�[�p�̗͂�0���傫�����
            if (_playerBasicInformation._hoverValue > 0f)
            {
                //�㏸�����A�K�X�������
                _playerBasicInformation._hoverValue -= Time.deltaTime * _gasConsumptionValue;
                _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, _hoverPower);
            }
        }
        //�z�o�[���ĂȂ��Ƃ��̏����B
        else
        {
            //�̗͂��ő�ł����
            if (_playerBasicInformation.MaxHealthForHover <= _playerBasicInformation._hoverValue)
            {
                _playerBasicInformation._hoverValue = _playerBasicInformation.MaxHealthForHover;
            }
            //�̗͂��ő�łȂ����
            else
            {
                _playerBasicInformation._hoverValue += Time.deltaTime * _gasRecoveryValue;
            }
        }
    }

    /// <summary> �ڐG������ </summary>
    /// <param name="collision"> �ڐG�Ώ� </param>
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
    /// <summary> �ڐG�ΏۂƗ��ꂽ���̏��� </summary>
    /// <param name="collision"> �ڐG�Ώ� </param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        //��q�ƐڐG���Ă���Ƃ��̏���
        if (collision.tag == "Ladder")
        {
            _isClimb = false;
            _rigidBody2D.gravityScale = _gravity;
        }
    }
    /// <summary> �v���C���[�̉E�ɉ������邩 </summary>
    bool BodyContactRight()
    {
        //�E�ɉ������邩���肷��
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _overLapBoxOffsetRight + transform.position,
            _overLapBoxSizeVertical,
            0f,
            _layerMaskCheckLR);
        //���������true��Ԃ�
        if (collision.Length != 0)
        {
            return true;
        }
        return false;
    }
    /// <summary> �v���C���[�̍��ɉ������邩 </summary>
    bool BodyContactLeft()
    {
        //���ɉ������邩���肷��
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _overLapBoxOffsetLeft + transform.position,
            _overLapBoxSizeVertical,
            0f,
            _layerMaskCheckLR);
        //���������true��Ԃ�
        if (collision.Length != 0)
        {
            return true;
        }
        return false;
    }
    /// <summary> �ڒn���� </summary>
    /// <returns> �ڒn���Ă���� true,�����łȂ���� false ��Ԃ��B </returns>
    public bool GetIsGround()
    {
        _overLapBoxCenter = transform.position + new Vector3(0f, _groundCheckPositionY, 0);
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _overLapBoxCenter,
            _overLapBoxSize,
            0f,
            _layerMaskGround);

        if (collision.Length != 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
