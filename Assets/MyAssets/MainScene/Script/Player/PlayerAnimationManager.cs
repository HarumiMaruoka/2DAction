using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのアニメーションを制御するコンポーネント
/// </summary>
public class PlayerAnimationManager : MonoBehaviour
{
    //===== フィールド =====//
    /// <summary> PlayerにアタッチされたAnimatorコンポーネント </summary>
    Animator _animator;
    /// <summary> プレイヤーのステートを管理するコンポーネント </summary>
    PlayerStateManagement _playerStateManagement;

    //=== アニメーターに渡すフィールド ===//
    // Move
    bool _isRun;
    bool _isDash;
    bool _isJump;
    bool _isHover;
    bool _isFall;
    bool _isSliding;
    bool _isClimb;

    //Attack
    bool _isShot;
    bool _isRunShot;
    bool _isJumpShot;
    bool _isDashShot;
    bool _isClimbShot;

    //Be killed
    bool _isDamage;
    bool _isDie;

    //===== プロパティ =====//
    /// <summary>
    /// 梯子昇降中のアニメーションスピード
    /// </summary>
    public float _climbAnimSpeed { get; set; }

    void Start()
    {
        _animator = GetComponent<Animator>();
        _playerStateManagement = GetComponent<PlayerStateManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        InitAnimParamAll();
        ChangeAnimation();
        SetAnim();
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

    void OnPause()
    {
        _animator.speed = Constants.PAUSE_ANIM_SPEED;
    }
    void OnResume()
    {
        _animator.speed = Constants.NOMAL_ANIM_SPEED;
    }

    void InitAnimParamAll()
    {
        _isRun = false;
        _isDash = false;
        _isJump = false;
        _isHover = false;
        _isFall = false;
        _isSliding = false;
        _isClimb = false;

        _isShot = false;
        _isRunShot = false;
        _isJumpShot = false;
        _isDashShot = false;
        _isClimbShot = false;

        _isDamage = false;
        _isDie = false;
    }
    /// <summary>
    /// ステートによって特定の値を切り替える
    /// </summary>
    void ChangeAnimation()
    {
        switch (_playerStateManagement._playerState)
        {
            case PlayerStateManagement.PlayerState.RUN: _isRun = true; break;
            case PlayerStateManagement.PlayerState.DASH: _isDash = true; break;
            case PlayerStateManagement.PlayerState.JUMP: _isJump = true; break;
            case PlayerStateManagement.PlayerState.HOVER: _isHover = true; break;
            case PlayerStateManagement.PlayerState.FALL: _isFall = true; break;
            case PlayerStateManagement.PlayerState.SLIDING: _isSliding = true; break;
            case PlayerStateManagement.PlayerState.CLIMB: _isClimb = true; break;

            case PlayerStateManagement.PlayerState.SHOT: _isShot = true; break;
            case PlayerStateManagement.PlayerState.RUN_SHOT: _isRunShot = true; break;
            case PlayerStateManagement.PlayerState.JUMP_SHOT: _isJumpShot = true; break;
            case PlayerStateManagement.PlayerState.DASH_SHOT: _isDashShot = true; break;
            case PlayerStateManagement.PlayerState.CLIMB_SHOT: _isClimbShot = true; break;

            case PlayerStateManagement.PlayerState.DAMAGE: _isDamage = true; break;
            case PlayerStateManagement.PlayerState.DIE: _isDie = true; break;
        }
    }
    /// <summary>
    /// アニメーターに値を渡す
    /// </summary>
    void SetAnim()
    {
        _animator.SetBool("isRun", _isRun);
        _animator.SetBool("isDash", _isDash);
        _animator.SetBool("isJump", _isJump);
        _animator.SetBool("isHover", _isHover);
        _animator.SetBool("isFall", _isFall);
        _animator.SetBool("isSliding", _isSliding);
        _animator.SetBool("isClimb", _isClimb);
        _animator.SetFloat("ClimbSpeed", _climbAnimSpeed);

        _animator.SetBool("isShot", _isShot);
        _animator.SetBool("isRunShot", _isRunShot);
        _animator.SetBool("isJumpShot", _isJumpShot);
        _animator.SetBool("isDashShot", _isDashShot);
        _animator.SetBool("isClimbShot", _isClimbShot);

        _animator.SetBool("isBeaten", _isDamage);
        _animator.SetBool("isKilled", _isDie);
    }
}
