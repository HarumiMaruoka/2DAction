using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�v���C���[�̈ړ��Ɋւ���N���X
public class PlayerController : MonoBehaviour
{
    //�e�p�����[�^
    public float move_speed_x;
    float previous_move_speed;
    const float dash_speed = 1.6f;
    public float jump_power;

    //�R���|�[�l���g
    Rigidbody2D _rigidbody2D;
    Jump_Script _jumpScript;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    PlayerAnimationManagement _playerAnimationManagement;
    PlayerBasicInformation _playerBasicInformation;

    //����̃v���n�u
    [SerializeField] GameObject _burrettPrefab;

    [Tooltip("�E�N���b�N�U�� �ꌂ�ڂ̃v���n�u")]
    [SerializeField] GameObject _slashPrefabOne;

    [Tooltip("�E�N���b�N�U�� �񌂖ڂ̃v���n�u")]
    [SerializeField] GameObject _slashPrefabTwo;

    GameObject _temporary;//���̓��ꕨ

    //[SerializeField] GameObject _slashPrefabTow;
    float _burrettCoolTime;
    float _slashCoolTimeOne;
    float _slashCoolTimeTow;

    //�z�o�[�֘A
    [SerializeField] float _hoverPower;//�z�o�[���̏㏸��
    [SerializeField] float _gasValue;//1�t���[���P�ʂ̃K�X�̏����

    //�X���C�f�B���O�֘A
    [SerializeField] float _slidingPower;
    public bool _isSlidingNow;

    //��q���~�֘A
    [Tooltip("��q��o��X�s�[�h")]
    [SerializeField] float _climbSpeed;
    bool _isclimb;
    float _climbGravityScale = 0;
    float _notClimbGravityScale = 12;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _jumpScript = GetComponent<Jump_Script>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerAnimationManagement = GetComponent<PlayerAnimationManagement>();
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();
        _burrettCoolTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerAnimationManagement._isMove)//�s���\�ł���Ύ��s����
        {
            //���͂��擾����
            float h = Input.GetAxisRaw("Horizontal");//������
            float v = Input.GetAxisRaw("Vertical");//�c����

            //���ړ�
            if (v == 0 && h != 0)//�c�̓��͂����鎞�͉��ړ��ł��Ȃ�
            {
                if (Input.GetButton("Dash"))
                {
                    _rigidbody2D.AddForce(Vector2.right * h * move_speed_x * Time.deltaTime * dash_speed, ForceMode2D.Force);
                }
                else
                {
                    _rigidbody2D.AddForce(Vector2.right * h * move_speed_x * Time.deltaTime, ForceMode2D.Force);
                }
            }

            //�X���C�f�B���O
            Sliding(v);

            //�W�����v
            if (v == 0 && !_isSlidingNow)//�c���͂����鎞�A���邢�̓X���C�f�B���O���͎��s�ł��Ȃ�
            {
                _jumpScript.Jump(jump_power);
            }

            //�z�o�[
            HoverManagement();

            //�V���b�g
            if (v == 0 && !_playerAnimationManagement._isHover)//�c�̓��͂����鎞�͑łĂȂ��A�z�o�[�����łĂȂ�
            {
                if (_burrettCoolTime < 0f)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        Instantiate(_burrettPrefab, Vector3.zero, Quaternion.identity);
                        _burrettCoolTime = 0.15f;
                    }
                }
                else if (_burrettCoolTime >= 0f)
                {
                    _burrettCoolTime -= Time.deltaTime;
                }
            }

            //�X���b�V��
            if (v == 0 && !_playerAnimationManagement._isHover)//�c�̓��͂����鎞�͑łĂȂ��A�z�o�[�����łĂȂ�
            {
                //�񌂖ڔ����������ł���Ȃ甭������
                if (_slashCoolTimeTow < 0f && _slashCoolTimeOne > 0f)
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        Destroy(_temporary);
                        Instantiate(_slashPrefabTwo, Vector3.zero, Quaternion.identity);
                        _slashCoolTimeTow = 0.35f;
                        _slashCoolTimeOne = 0.35f;
                    }
                }

                //�ꌂ��
                if (_slashCoolTimeOne < 0f)
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        _temporary = Instantiate(_slashPrefabOne, Vector3.zero, Quaternion.identity);
                        _slashCoolTimeOne = 0.35f;
                    }
                }

                //�N�[���^�C������
                if (_slashCoolTimeOne >= 0f)
                {
                    _slashCoolTimeOne -= Time.deltaTime;
                }
                if (_slashCoolTimeTow >= 0f)
                {
                    _slashCoolTimeTow -= Time.deltaTime;
                }
            }

            //��q���~
            Climb(v, h);
        }
    }

    /// <summary> �z�o�[�Ǘ��p�֐� </summary>
    void HoverManagement()//�z�o�[�Ǘ�
    {
        //�z�o�[���̏����B�X���C�f�B���O���ł͂Ȃ��Ƃ��Ɏ��s�ł���B
        if (_playerAnimationManagement._isHover && !_isSlidingNow)
        {
            //�z�o�[�p�̗͂�0���傫�����
            if (_playerBasicInformation._hoverValue > 0f)
            {
                //�㏸�����A�K�X�������
                _rigidbody2D.velocity = Vector2.up * _hoverPower;
                _playerBasicInformation._hoverValue -= Time.deltaTime * _gasValue;
            }
        }
        //�z�o�[���ĂȂ��Ƃ��̏���
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

    /// <summary> �X���C�f�B���O </summary>
    void Sliding(float inputVertical)
    {
        //�����͂����鎞���A�ڒn��ԂŁA�X�y�[�X�L�[���������ƂŃX���C�f�B���O�I
        if (inputVertical < 0 && _jumpScript.GetIsGround())
        {
            if (Input.GetButtonDown("Jump"))
            {
                //���Ɍ����Ă��鎞
                if (_spriteRenderer.flipX)
                {
                    _rigidbody2D.AddForce(Vector2.left * _slidingPower, ForceMode2D.Impulse);
                }
                //�E�Ɍ����Ă��鎞
                else
                {
                    _rigidbody2D.AddForce(Vector2.right * _slidingPower, ForceMode2D.Impulse);
                }
                _isSlidingNow = true;
                _playerAnimationManagement._isMove = false;
            }
        }
    }

    /// <summary> �A�j���[�V�����C�x���g����Ăяo���A�X���C�f�B���O�I���p�֐� </summary>
    public void SlidingStop()
    {
        _isSlidingNow = false;
        _playerAnimationManagement._isMove = true;
        _animator.SetTrigger("SlidingOff");
    }

    /// <summary> ��q���~�Ǘ��p�֐� </summary>
    void Climb(float v, float h)
    {
        //��q���~��
        if (_isclimb)
        {
            _rigidbody2D.gravityScale = _climbGravityScale;
            _rigidbody2D.velocity = new Vector2(h * move_speed_x * Time.deltaTime, _climbSpeed * v);
        }
        else
        {
            _rigidbody2D.gravityScale = _notClimbGravityScale;
        }
        if (_jumpScript.GetIsGround())
        {
            _rigidbody2D.gravityScale = _notClimbGravityScale;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //��q�ƐڐG���Ă���Ƃ��̏���
        if (collision.tag == "Ladder")
        {
            //��q��o��ꍇ
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                _isclimb = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //��q�ƐڐG���Ă���Ƃ��̏���
        if (collision.tag == "Ladder")
        {
            _isclimb = false;
        }
    }
}
