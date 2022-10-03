using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーのステートを管理するコンポーネント
/// </summary>
public class PlayerStateManagement : MonoBehaviour
{
    /// <summary>
    /// プレイヤーが表現できるステートの一覧
    /// </summary>
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

    //===== インスペクタ変数 =====//
    [Tooltip("スライディングの時間"), SerializeField] float _slidingTime;

    //===== フィールド =====//
    /// <summary>  
    /// Playerにアタッチされたコンポーネント。<br/>
    /// 入力管理コンポーネント。
    /// </summary>
    InputManager _inputManager = default;
    /// <summary>  
    /// Playerにアタッチされたコンポーネント。<br/>
    /// 速度で判定する際に使用する。
    /// </summary>
    Rigidbody2D _rigidBody2D = default;
    /// <summary> 
    /// Playerにアタッチされたコンポーネント。<br/>
    /// 接地判定で判定する際に使用する。
    /// </summary>
    PlayerMoveManager _playerMoveManager = default;
    /// <summary> 
    /// Playerにアタッチされたコンポーネント。<br/>
    /// 特定の条件でアニメーションの速度を変更する際に使用する。
    /// </summary>
    PlayerAnimationManager _newAnimationManagement = default;
    /// <summary> 
    /// Playerにアタッチされたコンポーネント。<br/>
    /// 絵の向きを反転させる為に使用する。
    /// </summary>
    SpriteRenderer _spriteRenderer = default;
    /// <summary>
    /// Playerにアタッチされたコンポーネント。<br/>
    /// 敵に接触した際にヒットSEを鳴らす為に使用する。
    /// </summary>
    AudioSource _audioSource = default;

    /// <summary>
    /// 梯子に接触しているかどうかを表す値
    /// </summary>
    bool _isClimbContact = false;
    /// <summary>
    /// 攻撃中かどうか表す値
    /// </summary>
    bool _isAttack = false;
    /// <summary>
    /// ホバー中かどうかを表す値
    /// </summary>
    bool _isHoverMode = false;

    //===== プロパティ =====//
    /// <summary>  現在のステートを表す値  </summary>
    public PlayerState _playerState { get; private set; } = PlayerState.IDLE;
    /// <summary>
    /// Enimy/EnemyAttackに接触したかどうか
    /// </summary>
    public bool IsHitEnemy { get; set; } = false;
    /// <summary>
    /// プレイヤーの体力があるかどうか / 死んでいるかどうか
    /// </summary>
    public bool IsDead { get; set; } = false;
    /// <summary>
    /// 移動できるかどうかの値
    /// </summary>
    public bool IsMove { get; set; } = true;
    /// <summary>
    /// ポーズ中かどうかを表す値
    /// </summary>
    public bool IsPause { get; set; } = false;
    /// <summary>
    /// スライディング中かどうか表す値
    /// </summary>
    public bool IsSlidingNow { get; set; } = false;

    IEnumerator _stopProcessingCoroutine = default;

    void Start()
    {
        _inputManager = GetComponent<InputManager>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _newAnimationManagement = GetComponent<PlayerAnimationManager>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerMoveManager = GetComponent<PlayerMoveManager>();
        _audioSource = GetComponent<AudioSource>();
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
            IsDead = true;
            IsMove = false;
        }

        if (!IsDead)
        {
            if (IsMove)
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
        if (IsSlidingNow || IsHitEnemy)
        {
            IsMove = false;
        }
        else
        {
            IsMove = true;
        }
        if (_audioSource.time > 0.8f)
        {
            _audioSource.Stop();
        }
        Die();
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
        Damage();
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
        if (IsSlidingNow)
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

    void Damage()
    {
        if (IsHitEnemy)
        {
            _playerState = PlayerState.DAMAGE;
        }
    }

    void Die()
    {
        if (IsDead)
        {
            _playerState = PlayerState.DIE;
        }
    }

    //アニメーションイベントから呼び出す
    public void ChibiRoboComeback()
    {
        IsHitEnemy = false;
        _isHoverMode = false;
        _audioSource.Stop();
    }

    IEnumerator StopProcessing(float stopTime)
    {
        float timer = 0f;
        IsSlidingNow = true;
        while (timer < stopTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        IsSlidingNow = false;
        _stopProcessingCoroutine = null;
    }
}
