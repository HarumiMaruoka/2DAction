using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�v���C���[�̃A�j���[�V�����Ɋւ���N���X
public class ChangePlayerState : MonoBehaviour
{
    //���̃N���X�ŗ��p����A���g�̊e�R���|�[�l���g
    Animator anim;
    Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    Jump_Script jump_script;

    //�s���\���H
    public bool isMove;
    public bool isHitEnemy;


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
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jump_script = GetComponent<Jump_Script>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //�s���\���H
        isMove = true;
        isHitEnemy = false;

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
        //Run animation

        //���͂��󂯕t����
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (!_isDead)
        {
            //���ړ�
            if (v == 0 && isMove)
            {
                if (h < 0)
                {
                    isEnd_of_move_left = false;
                    isLeft = true;
                    isRigth = false;
                    spriteRenderer.flipX = true;
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
                    spriteRenderer.flipX = false;
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

            if (v > 0)//�r�n�C���h�Ɍ����鏈��
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

            //�V���b�g
            float fire2 = Input.GetAxisRaw("Fire2");

            if (Input.GetButton("Fire1"))
            {
                if (!jump_script.GetIsGround())
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
            if (jump_script.GetIsGround())
            {
                _isFrySingleShot = false;
            }
            if (fire2 == 1)
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
            if (rigidbody2D.velocity.y > 0 && !jump_script.GetIsGround())//��ڒn���㏸��
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
            else if (jump_script.GetIsGround())//�ڒn��
            {
                _isJumpAnim = false;
                _isHover = false;
            }

            //�~��
            if (rigidbody2D.velocity.y < 0 && !jump_script.GetIsGround())//��ڒn�����~��
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
            else if(jump_script.GetIsGround())
            {
                _isFallAnim = false;
                _isHover = false;
            }

            //����ꂽ���̏���
            if (isHitEnemy)
            {
                isHitEnemy = false;
                isMove = false;
                anim.Play("Beaten");
                anim.SetTrigger("Blinking");
                _isBeatenAnim = true;
            }
        }
        //�|���ꂽ�Ƃ�
        if (_isDead)
        {
            anim.Play("Killed");
        }

        //Animetion Set
        SetAnim();
    }

    void SetAnim()
    {
        anim.SetBool("isShot", _isShotAnim);
        anim.SetBool("isRun", _isRunAnim);
        anim.SetBool("isJump", _isJumpAnim);
        anim.SetBool("isFall", _isFallAnim);
        anim.SetBool("isFront", _isFrontAnim);
        anim.SetBool("isBehind", _isBehindAnim);
        anim.SetBool("isSingelShot", _isSingleShot);
        anim.SetBool("isFrySingleShot", _isFrySingleShot);
        anim.SetBool("isDash", _isDash);
        anim.SetBool("isBeaten", _isBeatenAnim);
        anim.SetBool("isHover", _isHover);
    }

    public void ChibiRoboComeback()
    {
        isMove = true;
        _isBeatenAnim = false;
    }
}
