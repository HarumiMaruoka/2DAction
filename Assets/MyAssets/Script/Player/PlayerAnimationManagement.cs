using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManagement : MonoBehaviour
{
    //���̃N���X�ŗ��p����A���g�̊e�R���|�[�l���g
    Animator _anim;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    Jump_Script _jumpScript;

    //�s���\���H
    public bool _isMove;//�s���s�\�̎���false�ɂȂ�B
    public bool _isHitEnemy;//�G�ɉ���ꂽ����true�ɂȂ�B

    //�����Ă������
    public bool isRigth;
    public bool isLeft;
    bool isFront; //�O����
    bool isBehind;//�����

    bool isEnd_of_move_rigth;
    bool isEnd_of_move_left;

    bool _isShotExit;

    bool _isCharge_now;
    bool _isMoveStop;
    public bool _isDead;//�|���ꂽ���ǂ���

    bool _isShotAnim;
    bool _isRunAnim;
    bool _isJumpAnim;
    bool _isFallAnim;
    bool _isFrontAnim;
    bool _isBehindAnim;
    bool _isSingleShot;
    bool _isFrySingleShot;
    bool _isDash;
    bool _isBeatenAnim;//����ꂽ���̃A�j���[�V����
    public bool _isHover;//�z�o�[���Ă��邩�ǂ���


    // Start is called before the first frame update
    //�e�ϐ��̏�����
    void Start()
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jumpScript = GetComponent<Jump_Script>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        //�s���\���H
        _isMove = true;
        _isHitEnemy = false;

        //�����Ă������
        isRigth = true;
        isLeft = false;
        isFront = false;
        isBehind = false;

        //�ړ��I������
        isEnd_of_move_rigth = true;
        isEnd_of_move_left = true;

        //Charge���Ă��邩�ǂ���
        _isCharge_now = false;

        _isShotAnim = false;
        _isRunAnim = false;
        _isJumpAnim = false;
        _isFallAnim = false;
        _isFrontAnim = false;
        _isBehindAnim = false;
        _isSingleShot = false;
        _isFrySingleShot = false;
        _isDash = false;
        _isBeatenAnim = false;

        _isShotExit = false;
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
                if (h < 0)
                {
                    isEnd_of_move_left = false;
                    isLeft = true;
                    isRigth = false;
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
                //(�E�ړ����ł����)
                if (h > 0)
                {
                    _spriteRenderer.flipX = false;
                    isEnd_of_move_rigth = false;
                    _isRunAnim = true;
                    isLeft = false;
                    isRigth = true;
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

            //�c����
            if (v < 0)//�t�����g�Ɍ����鏈��
            {
                _isFrontAnim = true;
                isFront = false;
                _isRunAnim = false;

            }
            else if (!isFront)
            {
                isFront = true;
                _isFrontAnim = false;
            }

            //�r�n�C���h�Ɍ����鏈��
            if (v > 0)
            {
                _isBehindAnim = true;
                isBehind = false;
                _isRunAnim = false;

            }
            else if (!isBehind)
            {
                isBehind = true;
                _isBehindAnim = false;
            }

            //�V���b�g mouseButton0 ��������Ă���Ԏ��s�����
            if (Input.GetButton("Fire1"))
            {
                if (!_jumpScript.GetIsGround())
                {
                    _isFrySingleShot = true;
                }
                else
                {
                    _isShotAnim = true;
                }
                _isShotExit = true;
            }
            else if (_isShotExit)
            {
                _isFrySingleShot = false;
                _isShotAnim = false;
            }

            //�X���b�V�� mouseButton1 ��������Ă���Ԏ��s�����
            if (_jumpScript.GetIsGround())
            {
                _isFrySingleShot = false;
            }
            if (Input.GetButton("Fire2"))
            {
                _isShotAnim = true;
                _isCharge_now = true;
            }
            else if (_isCharge_now)
            {
                _isCharge_now = false;
                _isShotAnim = false;
            }

            //�W�����v
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
            else if(_jumpScript.GetIsGround())
            {
                _isFallAnim = false;
                _isHover = false;
            }

            //����ꂽ���̏���
            if (_isHitEnemy)
            {
                _isHitEnemy = false;
                _isMove = false;
                _anim.Play("Beaten");
                _anim.SetTrigger("Blinking");
                _isBeatenAnim = true;
            }
        }

        //�|���ꂽ�Ƃ�
        if (_isDead)
        {
            _anim.Play("Killed");
        }

        //Animetion Set
        SetAnim();
    }

    //�A�j���[�V�����p�ϐ���ݒ肷��֐��B���t���[���Ă΂��B
    void SetAnim()
    {
        _anim.SetBool("isShot", _isShotAnim);
        _anim.SetBool("isRun", _isRunAnim);
        _anim.SetBool("isJump", _isJumpAnim);
        _anim.SetBool("isFall", _isFallAnim);
        _anim.SetBool("isFront", _isFrontAnim);
        _anim.SetBool("isBehind", _isBehindAnim);
        _anim.SetBool("isSingelShot", _isSingleShot);
        _anim.SetBool("isFrySingleShot", _isFrySingleShot);
        _anim.SetBool("isDash", _isDash);
        _anim.SetBool("isBeaten", _isBeatenAnim);
        _anim.SetBool("isHover", _isHover);
    }

    //�v���C���[���s���s�\��Ԃ��畜�A���鎞�̏����B�G�ɉ���ꂽ���̃A�j���[�V�����C�x���g����Ă΂��B
    public void ChibiRoboComeback()
    {
        _isMove = true;
        _isBeatenAnim = false;
    }
}
