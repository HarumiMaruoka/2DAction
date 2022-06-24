using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManagement : MonoBehaviour
{
    Animator _animator;
    PlayerStateManagement _newPlayerStateManagement;

    //Move
    bool _isRun;
    bool _isDash;
    bool _isJump;
    bool _isHover;
    bool _isFall;
    bool _isSliding;
    bool _isClimb;
    public float _ClimbSpeed { get; set; }

    //Attack
    bool _isShot;
    bool _isRunShot;
    bool _isJumpShot;
    bool _isDashShot;
    bool _isClimbShot;

    //Be killed
    bool _isBeaten;
    bool _isKilled;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _newPlayerStateManagement = GetComponent<PlayerStateManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        SetAnimFalse();
        ChangeAnimation();
        SetAnim();
    }

    void SetAnimFalse()
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

        _isBeaten = false;
        _isKilled = false;
    }

    void ChangeAnimation()
    {
        switch (_newPlayerStateManagement._playerState)
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
            case PlayerStateManagement.PlayerState.DASH_SHOT: _isDashShot = true;break;
            case PlayerStateManagement.PlayerState.CLIMB_SHOT: _isClimbShot = true; break;

            case PlayerStateManagement.PlayerState.BEATEN: _isBeaten = true; break;
            case PlayerStateManagement.PlayerState.KILLED: _isKilled = true; break;
        }
    }

    void SetAnim()
    {
        _animator.SetBool("isRun", _isRun);
        _animator.SetBool("isDash", _isDash);
        _animator.SetBool("isJump", _isJump);
        _animator.SetBool("isHover", _isHover);
        _animator.SetBool("isFall", _isFall);
        _animator.SetBool("isSliding", _isSliding);
        _animator.SetBool("isClimb", _isClimb);
        _animator.SetFloat("ClimbSpeed", _ClimbSpeed);

        _animator.SetBool("isShot", _isShot);
        _animator.SetBool("isRunShot", _isRunShot);
        _animator.SetBool("isJumpShot", _isJumpShot);
        _animator.SetBool("isDashShot", _isDashShot);
        _animator.SetBool("isClimbShot", _isClimbShot);

        _animator.SetBool("isBeaten", _isBeaten);
        _animator.SetBool("isKilled", _isKilled);
    }
}
