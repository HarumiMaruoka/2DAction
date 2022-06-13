using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerStateManagement : MonoBehaviour
{
    public enum PlayerState
    {
        //Move
        IDLE,//�ʏ�
        RUN,//����
        DASH,//�_�b�V��
        JUMP,//�W�����v
        HOVER,//�z�o�[
        FALL,//����
        SLIDING,//�X���C�f�B���O
        CLIMB,//��q���~

        //Attack
        SHOT,//�V���b�g
        RUN_SHOT,//�����V���b�g
        DASH_SHOT,//�_�b�V���V���b�g
        JUMP_SHOT,//�󒆃V���b�g
        CLIMB_SHOT,//��q���~���V���b�g

        //Be killed
        BEATEN,//����
        KILLED,//�|���ꂽ��
    }

    public PlayerState _playerState { get; private set; } = PlayerState.IDLE;
    InputManager _inputManager;
    Rigidbody2D _rigidBody2D;
    JumpScript _jumpScript;
    Animator _animator;
    PlayerBasicInformation _playerBasicInformation;
    PlayerMoveManager _playerMoveManager;
    NewAnimationManagement _newAnimationManagement;
    SpriteRenderer _spriteRenderer;

    bool _isClimbContact = false;

    bool _isAttack = false;

    public bool _isHitEnemy { get; set; }
    public bool _isDead { get; set; }
    public bool _isMove { get; set; }

    bool _isHoverMode;

    public bool _isSlidingNow { get; set; }

    void Start()
    {
        _inputManager = GetComponent<InputManager>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _jumpScript = GetComponent<JumpScript>();
        _animator = GetComponent<Animator>();
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();
        _newAnimationManagement = GetComponent<NewAnimationManagement>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerMoveManager = GetComponent<PlayerMoveManager>();

        _isMove = true;
    }


    void Update()
    {
        UpdateState();
    }

    void UpdateState()
    {
        if (_playerBasicInformation._playerHitPoint <= 0)
        {
            _isDead = true;
        }
        if (!_isDead)
        {
            if (_isMove)
            {
                //Idle��Ԃŏ���������
                //(�����Ȃ���΃v���C���[��Idle���)
                _playerState = PlayerState.IDLE;

                //��Ԃ�ύX����
                MoveManage();
                AttackManage();
                OtherActionManage();
            }
            //�X���C�f�B���O�́A_isMove�������̂ŏ��O
            Sliding();
        }
        Killed();
    }

    void MoveManage()
    {
        Run();
        Jump();
        Dash();
        Fall();
        Hover();
        Climb();
    }

    void AttackManage()
    {
        if (_inputManager._inputFire1 || _inputManager._inputFire2)
        {
            _isAttack = true;
        }
        else
        {
            _isAttack = false;
        }

        Shot();
        RunShot();
        DashShot();
        JumpShot();
        ClimbShot();
    }

    void OtherActionManage()
    {
        Beaten();
    }

    void Run()
    {
        if (_inputManager._inputHorizontal != 0)
        {
            _playerState = PlayerState.RUN;
            _spriteRenderer.flipX = _inputManager._inputHorizontal < 0 ? true : false;
        }
    }

    void Dash()
    {
        if (_playerState == PlayerState.RUN && _inputManager._inputLeftShift)
        {
            _playerState = PlayerState.DASH;
        }
    }

    void Jump()
    {
        if (_rigidBody2D.velocity.y > 0 && !_jumpScript.GetIsGround())//��ڒn���㏸��
        {
            _playerState = PlayerState.JUMP;
        }
    }

    void Fall()
    {
        if (_rigidBody2D.velocity.y < 0 && !_jumpScript.GetIsGround())//��ڒn��������
        {
            _playerState = PlayerState.FALL;
        }
    }

    void Hover()
    {
        if ((_playerState == PlayerState.JUMP || _playerState == PlayerState.FALL) && !_playerMoveManager._isJump)
        {
            if (_inputManager._inputJumpDown)
            {
                _isHoverMode = true;
            }
        }
        if (_inputManager._inputJumpUp || _jumpScript.GetIsGround())
        {
            _isHoverMode = false;
        }

        if (_isHoverMode)
        {
            _playerState = PlayerState.HOVER;
        }
    }

    void Sliding()
    {
        if (_isSlidingNow)
        {
            _playerState = PlayerState.SLIDING;
        }
    }

    void Climb()
    {
        //��q�����~���鎞�̏���
        if (_isClimbContact)
        {
            //�c�̓��͂����鎞
            if (_inputManager._inputVertical != 0)
            {
                //���~��
                if (_inputManager._inputVertical != 0)
                {
                    _playerState = PlayerState.CLIMB;
                    _newAnimationManagement._ClimbSpeed = 1f;
                }
            }
            //�c�̓��͂��Ȃ��Ȃ������̏���
            else
            {
                //�A�j���[�V�������ꎞ��~����
                _playerState = PlayerState.CLIMB;
                _newAnimationManagement._ClimbSpeed = 0f;
            }
            //���n�����Ƃ�
            if (_jumpScript.GetIsGround())
            {
                _playerState = PlayerState.IDLE;
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
                _isClimbContact = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //��q�ƐڐG���Ă���Ƃ��̏���
        if (collision.tag == "Ladder")
        {
            _isClimbContact = false;
        }
    }

    void Shot()
    {
        if (_playerState == PlayerState.IDLE && _isAttack)
        {
            _playerState = PlayerState.SHOT;
        }
    }

    void RunShot()
    {
        if (_playerState == PlayerState.RUN && _isAttack)
        {
            _playerState = PlayerState.RUN_SHOT;
        }
    }

    void DashShot()
    {
        if (_playerState == PlayerState.DASH && _isAttack)
        {
            _playerState = PlayerState.DASH_SHOT;
        }
    }

    void JumpShot()
    {
        if ((_playerState == PlayerState.JUMP || _playerState == PlayerState.FALL) && _isAttack)
        {
            _playerState = PlayerState.JUMP_SHOT;
        }
    }

    void ClimbShot()
    {
        if (_playerState == PlayerState.CLIMB && _isAttack)
        {
            _playerState = PlayerState.CLIMB_SHOT;
        }
    }

    void Beaten()
    {
        if (_isHitEnemy)
        {
            _playerState = PlayerState.BEATEN;
            _isMove = false;
        }
    }

    void Killed()
    {
        if (_isDead)
        {
            _playerState = PlayerState.KILLED;
        }
    }

    //�A�j���[�V�����C�x���g����Ăяo��
    public void ChibiRoboComeback()
    {
        _isHitEnemy = false;
        _isMove = true;
        _isHoverMode = false;
    }

    //���̊֐��̓A�j���[�V�����C�x���g����Ăяo��
    /// <summary> �X���C�f�B���O�I���֐� </summary>
    public void SlidingStop()
    {
        _isMove = true;
        _isSlidingNow = false;
    }
}
