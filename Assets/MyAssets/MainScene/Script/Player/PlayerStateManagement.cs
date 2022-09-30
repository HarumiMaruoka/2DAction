﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステートを管理するコンポーネント
/// </summary>
public class PlayerStateManagement : MonoBehaviour
{
    public enum PlayerState
    {
        // Move
        IDLE,//通常
        RUN,//歩き
        DASH,//ダッシュ
        JUMP,//ジャンプ
        HOVER,//ホバー
        FALL,//落下
        SLIDING,//スライディング
        CLIMB,//梯子昇降

        // Attack
        SHOT,//ショット
        RUN_SHOT,//歩きショット
        DASH_SHOT,//ダッシュショット
        JUMP_SHOT,//空中ショット
        CLIMB_SHOT,//梯子昇降中ショット

        // ダメージを喰らった時
        DAMAGE,
        //倒された時 / 死
        DIE,
    }
    /// <summary>  現在のステート  </summary>
    public PlayerState _playerState { get; private set; } = PlayerState.IDLE;
    /// <summary>  入力管理コンポーネント  </summary>
    InputManager _inputManager;
    /// <summary>  速度で判定する際に使用する  </summary>
    Rigidbody2D _rigidBody2D;
    /// <summary>  接地判定で判定する際に使用する </summary>
    PlayerMoveManager _playerMoveManager;
    /// <summary> 特定の条件でアニメーションの速度を変更する </summary>
    PlayerAnimationManager _newAnimationManagement;
    SpriteRenderer _spriteRenderer;
    AudioSource _hitEnemySound;

    bool _isClimbContact = false;

    bool _isAttack = false;

    public bool _isHitEnemy { get; set; }
    bool _beforeFrameIsHitEnemy;
    public bool _isDead { get; set; }
    public bool _isMove { get; set; }
    public bool _isPause { get; set; }

    bool _isHoverMode;

    public bool _isSlidingNow { get; set; }

    IEnumerator _stopProcessingCoroutine = default;
    [Tooltip("スライディングの時間"), SerializeField] float _slidingTime;

    void Start()
    {
        _inputManager = GetComponent<InputManager>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _newAnimationManagement = GetComponent<PlayerAnimationManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerMoveManager = GetComponent<PlayerMoveManager>();
        _hitEnemySound = GetComponent<AudioSource>();

        _isMove = true;
        _isPause = false;
    }
    void Update()
    {
        UpdateState();
    }

    private void OnEnable()
    {
        GameManager.OnPause += OnPause;
        GameManager.OnResume += OnResume;
    }
    private void OnDestroy()
    {
        GameManager.OnPause -= OnPause;
        GameManager.OnResume -= OnResume;
    }

    void OnPause()
    {
        if (_stopProcessingCoroutine != null)
        {
            StopCoroutine(_stopProcessingCoroutine);
        }
    }
    void OnResume()
    {
        if (_stopProcessingCoroutine != null)
        {
            StartCoroutine(_stopProcessingCoroutine);
        }
    }

    void UpdateState()
    {
        if (PlayerStatusManager.Instance.PlayerHealthPoint <= 0)
        {
            _isDead = true;
            _isMove = false;
        }

        if (!_isDead)
        {
            if (_isMove)
            {
                //Idle状態で初期化する
                //(何もなければプレイヤーはIdle状態)
                _playerState = PlayerState.IDLE;

                //状態を変更する
                MoveManage();
                AttackManage();
                Sliding();
            }
            OtherActionManage();
        }
        //移動できるかどうか
        if (_isSlidingNow || _isHitEnemy)
        {
            _isMove = false;
        }
        else
        {
            _isMove = true;
        }
        if (_hitEnemySound.time > 0.8f)
        {
            _hitEnemySound.Stop();
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
            //var localScale = transform.localScale;
            //if (_inputManager._inputHorizontal > 0f && localScale.x < 0f)
            //{
            //    localScale.x *= -1f;
            //}
            //else if (_inputManager._inputHorizontal < 0f && localScale.x > 0f)
            //{
            //    localScale.x *= -1f;
            //}
            //transform.localScale = localScale;
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
        if (_rigidBody2D.velocity.y > 0 && !_playerMoveManager.GetIsGround())//非接地かつ上昇中
        {
            _playerState = PlayerState.JUMP;
        }
    }

    void Fall()
    {
        if (_rigidBody2D.velocity.y < 0 && !_playerMoveManager.GetIsGround())//非接地かつ落下中
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
        if (_inputManager._inputJumpUp || _playerMoveManager.GetIsGround())
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
            _stopProcessingCoroutine = StopProcessing(_slidingTime);
            StartCoroutine(_stopProcessingCoroutine);
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
                _playerState = PlayerState.CLIMB;
                _newAnimationManagement._climbAnimSpeed = 1f;
            }
            //縦の入力がなくなった時の処理
            else
            {
                //アニメーションを一時停止する
                _playerState = PlayerState.CLIMB;
                _newAnimationManagement._climbAnimSpeed = 0f;
            }
            //着地したとき
            if (_playerMoveManager.GetIsGround())
            {
                _playerState = PlayerState.IDLE;
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

    private void OnTriggerExit2D(Collider2D collision)
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
            _playerState = PlayerState.DAMAGE;
        }
    }

    void Killed()
    {
        if (_isDead)
        {
            _playerState = PlayerState.DIE;
        }
    }

    //アニメーションイベントから呼び出す
    public void ChibiRoboComeback()
    {
        _isHitEnemy = false;
        _isHoverMode = false;
        _hitEnemySound.Stop();
    }

    IEnumerator StopProcessing(float stopTime)
    {
        float timer = 0f;
        _isSlidingNow = true;
        while (timer < stopTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        _isSlidingNow = false;
        _stopProcessingCoroutine = null;
    }
}
