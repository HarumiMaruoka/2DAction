using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManagement : MonoBehaviour
{
    //���̃N���X�ŗ��p����A���g�̊e�R���|�[�l���g
    Animator _animator;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    JumpScript _jumpScript;
    InputManager _inputManager;

    //�s���\���H
    public bool _isMove { get; set; }//�s���s�\�̎���false�ɂȂ�B
    public bool _isHitEnemy;//�G�ɉ���ꂽ����true�ɂȂ�B

    bool isEnd_of_move_rigth;
    bool isEnd_of_move_left;

    public bool _isDead { get; set; }//�|���ꂽ���ǂ���

    //�A�j���[�V�����Ǘ��p�ϐ�
    bool _isAttack;
    bool _isAttack2;
    bool _isRunAnim;
    bool _isJumpAnim;
    bool _isFallAnim;
    bool _isFrontAnim;
    bool _isBehindAnim;
    bool _isFryAttack;
    bool _isFryAttack2;
    bool _isDash;
    bool _isBeatenAnim;//����ꂽ���̃A�j���[�V����
    public bool _isHover;//�z�o�[���Ă��邩�ǂ���
    public bool _isClimb;//��q�����~���Ă��邩�ǂ���


    // Start is called before the first frame update
    //�e�ϐ��̏�����
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jumpScript = GetComponent<JumpScript>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inputManager = GetComponent<InputManager>();

        //�s���\���H
        _isMove = true;
        _isHitEnemy = false;

        //�ړ��I������
        isEnd_of_move_rigth = true;
        isEnd_of_move_left = true;

        _isAttack = false;
        _isAttack2 = false;
        _isRunAnim = false;
        _isJumpAnim = false;
        _isFallAnim = false;
        _isFrontAnim = false;
        _isBehindAnim = false;
        _isFryAttack = false;
        _isFryAttack2 = false;
        _isDash = false;
        _isBeatenAnim = false;
        _isClimb = false;

        _isDead = false;
    }


    // Update is called once per frame
    void Update()
    {
        //���͂��󂯕t����
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (!_isDead)
        {
            //���ړ�
            if (v == 0 && _isMove)
            {
                //���������ړ����������ł���Έȉ������s����
                //(���ړ����̏ꍇ)
                if (h < 0)
                {
                    isEnd_of_move_left = false;
                    _spriteRenderer.flipX = true;
                    _isRunAnim = true;
                    if (Input.GetButton("Dash"))
                    {
                        _isDash = true;
                    }
                    else
                    {
                        _isDash = false;
                    }

                }
                //���ړ����������ꂽ��
                else if (!isEnd_of_move_left)
                {
                    _isDash = false;
                    isEnd_of_move_left = true;
                    _isRunAnim = false;
                }

                //���������ړ����������ł���Έȉ������s����
                //(�E�ړ����̏ꍇ)
                if (h > 0)
                {
                    _spriteRenderer.flipX = false;
                    isEnd_of_move_rigth = false;
                    _isRunAnim = true;
                    if (Input.GetButton("Dash"))
                    {
                        _isDash = true;
                    }
                    else
                    {
                        _isDash = false;
                    }
                }
                else if (!isEnd_of_move_rigth)
                {
                    _isDash = false;
                    isEnd_of_move_rigth = true;
                    _isRunAnim = false;
                }
            }
            if (Input.GetButtonUp("Dash"))
            {
                _isDash = false;
            }

            if (_inputManager._inputVertical != 0)
            {
                _isRunAnim = false;
                _isDash = false;
            }

            //�X���C�f�B���O
            SlidingAnim();

            //�V���b�g mouseButton0 ��������Ă���Ԏ��s�����
            if (Input.GetButton("Fire1"))
            {
                if (_jumpScript.GetIsGround())
                {
                    _isAttack = true;
                }
                else
                {
                    _isFryAttack = true;
                }
            }
            else
            {
                _isFryAttack = false;
                _isAttack = false;
            }

            //�X���b�V�� mouseButton1 ��������Ă���Ԏ��s�����
            if (Input.GetButton("Fire2"))
            {
                if (_jumpScript.GetIsGround())
                {
                    _isAttack2 = true;
                }
                else
                {
                    _isFryAttack2 = true;
                }
            }
            else
            {
                _isFryAttack2 = false;
                _isAttack2 = false;
            }

            _isAttack = (_isAttack2 || _isAttack);
            _isFryAttack = (_isFryAttack2 || _isFryAttack);

            //�W�����v/�z�o�[
            if (_rigidbody2D.velocity.y > 0 && !_jumpScript.GetIsGround())//��ڒn���㏸��
            {
                _isJumpAnim = true;
                //�z�o�[����ꍇ
                if (Input.GetButtonDown("Jump"))
                {
                    _isHover = true;
                }
                else if (Input.GetButtonUp("Jump"))
                {
                    _isHover = false;
                }
            }
            else if (_jumpScript.GetIsGround())//�ڒn��
            {
                _isJumpAnim = false;
                _isHover = false;
            }

            //�~��
            if (_rigidbody2D.velocity.y < 0 && !_jumpScript.GetIsGround())//��ڒn�����~��
            {
                _isFallAnim = true;
                //�z�o�[����ꍇ
                if (Input.GetButtonDown("Jump"))
                {
                    _isHover = true;
                }
                else if (Input.GetButtonUp("Jump"))
                {
                    _isHover = false;
                }
            }
            else if (_jumpScript.GetIsGround())
            {
                _isFallAnim = false;
                _isHover = false;
            }

            //����ꂽ���̏���
            if (_isHitEnemy)
            {
                _isHitEnemy = false;
                _isMove = false;
                _animator.Play("Beaten");
                _animator.SetTrigger("Blinking");
                _isBeatenAnim = true;
            }

            //��q���~
            ClimbAnim();
        }

        //�|���ꂽ�Ƃ�
        if (_isDead)
        {
            _animator.Play("Killed");
        }

        //Animetion Set
        SetAnim();
    }

    //�A�j���[�V�����p�ϐ���ݒ肷��֐��B���t���[���Ă΂��B
    void SetAnim()
    {
        _animator.SetBool("isAttack", _isAttack);
        _animator.SetBool("isRun", _isRunAnim);
        _animator.SetBool("isJump", _isJumpAnim);
        _animator.SetBool("isFall", _isFallAnim);
        _animator.SetBool("isFront", _isFrontAnim);
        _animator.SetBool("isBehind", _isBehindAnim);
        _animator.SetBool("isFlyAttack", _isFryAttack);
        _animator.SetBool("isDash", _isDash);
        _animator.SetBool("isBeaten", _isBeatenAnim);
        _animator.SetBool("isHover", _isHover);
        _animator.SetBool("isClimb", _isClimb);
    }

    //�v���C���[���s���s�\��Ԃ��畜�A���鎞�̏����B�G�ɉ���ꂽ���̃A�j���[�V�����C�x���g����Ă΂��B
    public void ChibiRoboComeback()
    {
        _isMove = true;
        _isBeatenAnim = false;
    }

    /// <summary> �X���C�f�B���O </summary>
    void SlidingAnim()
    {
        //�����͂����鎞���A�ڒn��ԂŁA�X�y�[�X�L�[���������ƂŃX���C�f�B���O�I
        if (Input.GetAxisRaw("Vertical") < 0 && _jumpScript.GetIsGround())
        {
            if (Input.GetButtonDown("Jump"))
            {
                _animator.Play("Sliding");
            }
        }
    }

    /// <summary> ��q�̓o��~�� </summary>
    void ClimbAnim()
    {
        //��q�����~���鎞�̏���
        if (_isClimb)
        {
            //�c�̓��͂����鎞
            if (_inputManager._inputVertical != 0)
            {
                //���~��
                if (_inputManager._inputVertical != 0)
                {
                    _isJumpAnim = false;
                    _isFallAnim = false;
                    _isClimb = true;
                    _animator.SetFloat("ClimbSpeed", 1f);
                }
                //���n�����Ƃ�
                if (_jumpScript.GetIsGround())
                {
                    _isClimb = false;
                }
            }
            //�c�̓��͂��Ȃ��Ȃ������̏���
            else
            {
                //�A�j���[�V�������ꎞ��~����
                _animator.SetFloat("ClimbSpeed", 0f);
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
                _animator.Play("Climb");
            }
        }
    }

    //�R���C�_�[���ڐG���Ă���Ƃ��̏���
    private void OnTriggerExit2D(Collider2D collision)
    {
        //��q���痣�ꂽ�Ƃ��̏���
        if (collision.tag == "Ladder")
        {
            _isClimb = false;
        }
    }

}
