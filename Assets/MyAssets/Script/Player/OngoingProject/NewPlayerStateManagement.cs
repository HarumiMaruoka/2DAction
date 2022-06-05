using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayerStateManagement : MonoBehaviour
{
    public enum PlayerState
    {
        //Move
        IDLE,//通常
        RUN,//歩き
        DASH,//ダッシュ
        JUMP,//ジャンプ
        HOVER,//ホバー
        FALL,//落下
        SLIDING,//スライディング
        CLIMB,//梯子昇降

        //Attack
        SHOT,//ショット
        RUN_SHOT,//歩きショット
        DASH_SHOT,//ダッシュショット
        JUMP_SHOT,//空中ショット
        CLIMB_SHOT,//梯子昇降中ショット

        //Be killed
        BEATEN,//やられ
        KILLED,//倒された時
    }

    public PlayerState _playerState { get; private set; } = PlayerState.IDLE;
    InputManager _inputManager;
    Rigidbody2D _rigidBody2D;
    JumpScript _jumpScript;
    Animator _animator;
    PlayerBasicInformation _playerBasicInformation;
    NewAnimationManagement _newAnimationManagement;
    SpriteRenderer _spriteRenderer;

    bool _isClimbContact = false;

    bool _isAttack = false;

    public bool _isHitEnemy { get; set; }
    public bool _isDead { get; set; }
    public bool _isMove { get; set; }

    bool _isHoverMode;

    bool _isSlidingNow;

    void Start()
    {
        _inputManager = GetComponent<InputManager>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _jumpScript = GetComponent<JumpScript>();
        _animator = GetComponent<Animator>();
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();
        _newAnimationManagement = GetComponent<NewAnimationManagement>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

        _isMove = true;
    }


    void Update()
    {
        UpdateState();
    }

    void UpdateState()
    {
        if (_isMove)
        {
            //Idle状態で初期化する
            //(何もなければプレイヤーはIdle状態)
            _playerState = PlayerState.IDLE;

            //状態を変更する
            MoveManage();
            AttackManage();
            OtherActionManage();
        }
    }

    void MoveManage()
    {
        Run();
        Dash();
        Jump();
        Fall();
        Hover();
        Sliding();
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
        Killed();
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
        if (_playerState == PlayerState.RUN &&
            _inputManager._inputLeftShift)
        {
            _playerState = PlayerState.DASH;
        }
    }

    void Jump()
    {
        if (_rigidBody2D.velocity.y > 0 && !_jumpScript.GetIsGround())//非接地かつ上昇中
        {
            _playerState = PlayerState.JUMP;
        }
    }

    void Fall()
    {
        if (_rigidBody2D.velocity.y < 0 && !_jumpScript.GetIsGround())//非接地かつ落下中
        {
            _playerState = PlayerState.FALL;
        }
    }

    void Hover()
    {
        if ((_playerState == PlayerState.JUMP || _playerState == PlayerState.FALL) && _inputManager._inputJumpDown)
        {
            _isHoverMode = true;
        }
        if (_playerState == PlayerState.HOVER && _inputManager._inputJumpDown)
        {
            _isHoverMode = true;
        }
        else
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
        //下入力がある時かつ、接地状態で、スペースキーを押すことでスライディング！
        if (_inputManager._inputVertical < 0 && _jumpScript.GetIsGround())
        {
            if (_inputManager._inputJumpDown)
            {
                _playerState = PlayerState.SLIDING;
            }
        }
        if (_isSlidingNow)
        {
            _playerState = PlayerState.SLIDING;
        }
    }

    void Climb()
    {
        //梯子を昇降する時の処理
        if (_isClimbContact)
        {
            //縦の入力がある時
            if (_inputManager._inputVertical != 0)
            {
                //昇降中
                if (_inputManager._inputVertical != 0)
                {
                    //Debug.Log("昇降中");
                    _playerState = PlayerState.CLIMB;
                    _newAnimationManagement._ClimbSpeed = 1f;
                }
                //着地したとき
                if (_jumpScript.GetIsGround())
                {
                    //Debug.Log("着地！");
                    _playerState = PlayerState.IDLE;
                }
            }
            //縦の入力がなくなった時の処理
            else
            {
                //Debug.Log("梯子アニメーション停止！");
                //アニメーションを一時停止する
                _newAnimationManagement._ClimbSpeed = 0f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //梯子と接触しているときの処理
        if (collision.tag == "Ladder")
        {
            //梯子を登る場合
            if (_inputManager._inputVertical != 0)
            {
                _isClimbContact = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //梯子と接触しているときの処理
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

    //アニメーションイベントから呼び出す
    public void ChibiRoboComeback()
    {
        _isHitEnemy = false;
        _isMove = true;
    }
}
