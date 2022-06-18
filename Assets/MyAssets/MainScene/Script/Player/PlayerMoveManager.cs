using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveManager : MonoBehaviour
{
    //パラメータ
    [Tooltip("横移動速度")]
    [SerializeField] float _moveSpeedX;
    public float MoveSpeedX { get => _moveSpeedX; }

    [Tooltip("ダッシュ時の加速度")]
    [SerializeField] float _moveDashSpeed;
    public float MoveDashSpeed { get => _moveDashSpeed; }

    [Tooltip("ジャンプ力")]
    [SerializeField] float _jumpPower;
    public float JumpPower { get => _jumpPower; }

    [Tooltip("ホバー時の上昇力")]
    [SerializeField] float _hoverPower;
    public float HoverPower { get => _hoverPower; }

    [Tooltip("ホバー中のガソリン消費量")]
    [SerializeField] float _gasConsumptionValue;
    public float GasConsumptionValue { get => _gasConsumptionValue; }


    [Tooltip("ホバー用のガソリン回復量")]
    [SerializeField] float _gasRecoveryValue;
    public float GasRecoveryValue { get => _gasRecoveryValue; }

    [Tooltip("梯子の昇降速度")]
    [SerializeField] float _climbSpeed;
    public float ClimbSpeed { get => _climbSpeed; }

    [Tooltip("横移動速度の減速率")]
    [SerializeField] float _decelerationRateX;
    public float DecelerationRateX { get => _decelerationRateX; }

    [Tooltip("スライディング速度")]
    [SerializeField] float _slidingSpeed;
    public float SlidingSpeed { get => _slidingSpeed; }

    //コンポーネント
    Rigidbody2D _rigidBody2D;
    InputManager _inputManager;
    JumpScript _jumpScript;
    SpriteRenderer _spriteRendere;
    NewPlayerStateManagement _newPlayerStateManagement;
    PlayerBasicInformation _playerBasicInformation;
    Animator _animator;

    //このフレームで加える力
    Vector2 _newForce;
    public Vector2 _newImpulse;
    Vector2 _newVelocity;

    /// <summary> ジャンプできるか:スペースキーは、ジャンプとスライディングの機能を兼ねる為 </summary>
    bool _canJump = false;
    //プレイヤーが向いている方向
    bool _isRigth = false;
    //梯子を登れるかどうか
    bool _isClimb = false;
    //重力:梯子を登っているときは重力を0にするため
    float _gravity;
    //ジャンプしたかどうかの確認
    public bool _isJump { get; private set; } = false;

    //右に何があるか判定用
    [Tooltip("右に何かがないか判定用"), SerializeField]
    private Vector3 _overLapBoxOffsetRight;
    //左に何があるか判定用
    [Tooltip("左に何かがないか判定用"), SerializeField]
    private Vector3 _overLapBoxOffsetLeft;
    //上記のサイズ
    [Tooltip("上記のサイズ"), SerializeField]
    private Vector2 _overLapBoxSizeVertical;
    /// <summary> gizmo表示 </summary>
    [SerializeField]
    LayerMask _layerMaskCheckLR;
    /// <summary> gizmo表示 </summary>
    [SerializeField] bool _isGizmo = false;

    void Start()
    {
        //コンポーネントの初期化
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _inputManager = GetComponent<InputManager>();
        _jumpScript = GetComponent<JumpScript>();
        _spriteRendere = GetComponent<SpriteRenderer>();
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();
        _newPlayerStateManagement = GetComponent<NewPlayerStateManagement>();
        _animator = GetComponent<Animator>();
        _gravity = _rigidBody2D.gravityScale;

        //各変数の初期化
        _newForce = Vector2.zero;
        _newImpulse = Vector2.zero;
        _newVelocity = Vector2.zero;
        //プレイヤーが向いている方向を取得
        _isRigth = !_spriteRendere.flipX;
    }

    void Update()
    {
        Move();
    }

    void Move()
    {
        //加える力の初期化
        _newForce = Vector2.zero;
        _newImpulse = Vector2.zero;
        _newVelocity = Vector2.zero;
        if (_newPlayerStateManagement._isMove && !_newPlayerStateManagement._isDead)
        {
            //プレイヤーが向いている方向を取得
            _isRigth = !_spriteRendere.flipX;

            //移動処理
            MoveHorizontal();
            Dash();
            Sliding();
            Jump();
            Climb();
            Hover();

            //空中では横移動速度が遅くなる
            if (!_jumpScript.GetIsGround())
            {
                _newForce.x *= DecelerationRateX;
            }

            //実際に力を加える
            _rigidBody2D.AddForce(_newImpulse * 10f, ForceMode2D.Impulse);
            _rigidBody2D.AddForce(_newForce * 10f * Time.deltaTime * 100f, ForceMode2D.Force);
            if (Mathf.Approximately(_newImpulse.x, 0f) && Mathf.Approximately(_newImpulse.y, 0f))
            {
                _rigidBody2D.velocity = new Vector2(_newVelocity.x, _rigidBody2D.velocity.y);
            }

        }
    }

    /// <summary> 横移動の処理 </summary>
    void MoveHorizontal()
    {
        //縦の入力がある時は実行できない
        if (!(_inputManager._inputVertical != 0))
        {
            //左移動の処理
            if (_inputManager._inputHorizontal < 0 && !BodyContactLeft())//左に何もなければ実行できる
            {
                _newVelocity += new Vector2(_inputManager._inputHorizontal * MoveSpeedX, 0f);
            }
            //右移動の処理
            if (_inputManager._inputHorizontal > 0 && !BodyContactRight())//右に何もなければ実行できる
            {
                _newVelocity += new Vector2(_inputManager._inputHorizontal * MoveSpeedX, 0f);
            }
        }
    }

    void Dash()
    {
        //LeftShiftキーで加速
        if (_inputManager._inputLeftShift)
        {
            _newVelocity *= _moveDashSpeed;
        }
    }

    void Sliding()
    {
        _canJump = true;
        //接地かつSキーで実行可能
        if (_inputManager._inputVertical < 0 && _jumpScript.GetIsGround())
        {
            //上記条件をクリアしたうえで、スペースキーが押された場合スライディングする！
            if (_inputManager._inputJumpDown)
            {
                _newImpulse += _isRigth ? new Vector2(_slidingSpeed, 0f) : new Vector2(-_slidingSpeed, 0f);
                _canJump = false;
                _newPlayerStateManagement._isMove = false;
                _newPlayerStateManagement._isSlidingNow = true;
            }
        }
    }

    void Jump()
    {
        //接地かつスペースキーでジャンプ
        if (_jumpScript.GetIsGround() && _inputManager._inputJumpDown && _canJump)
        {
            _newImpulse += new Vector2(0f, JumpPower);
            _isJump = true;
        }
        else if (_isJump)
        {
            _isJump = false;
        }
    }

    void Climb()
    {
        //梯子を昇降する時の処理
        if (_isClimb)
        {
            //縦の入力がある時
            if (_inputManager._inputVertical != 0 && !(_inputManager._inputFire1 || _inputManager._inputFire2))
            {
                //下に入力したとき
                //非接地状態なら降りれる
                if (_inputManager._inputVertical < 0 && !_jumpScript.GetIsGround())
                {
                    _rigidBody2D.velocity = Vector2.up * ClimbSpeed * _inputManager._inputVertical;
                }
                //上に入力したとき
                else if (_inputManager._inputVertical > 0)
                {
                    _rigidBody2D.velocity = Vector2.up * ClimbSpeed * _inputManager._inputVertical;
                }
            }
            //縦の入力がなくなった時の処理
            else
            {
                _rigidBody2D.velocity = Vector2.zero;
                _rigidBody2D.gravityScale = 0f;
            }
        }
    }

    void Hover()
    {
        //ホバーの処理。
        if (_newPlayerStateManagement._playerState == NewPlayerStateManagement.PlayerState.HOVER)
        {
            //ホバー用体力が0より大きければ
            if (_playerBasicInformation._hoverValue > 0f)
            {
                //上昇処理、ガスを消費する
                _playerBasicInformation._hoverValue -= Time.deltaTime * _gasConsumptionValue;
                _rigidBody2D.velocity = new Vector2(_rigidBody2D.velocity.x, _hoverPower);
            }
        }
        //ホバーしてないときの処理。
        else
        {
            //体力が最大であれば
            if (_playerBasicInformation.MaxHealthForHover <= _playerBasicInformation._hoverValue)
            {
                _playerBasicInformation._hoverValue = _playerBasicInformation.MaxHealthForHover;
            }
            //体力が最大でなければ
            else
            {
                _playerBasicInformation._hoverValue += Time.deltaTime * _gasRecoveryValue;
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
                _isClimb = true;
            }
        }
    }

    //コライダーが接触しているときの処理
    private void OnTriggerExit2D(Collider2D collision)
    {
        //梯子と接触しているときの処理
        if (collision.tag == "Ladder")
        {
            _isClimb = false;
            _rigidBody2D.gravityScale = _gravity;
        }
    }

    /// <summary> プレイヤーの右に何かあるか </summary>
    bool BodyContactRight()
    {
        //右に何かあるか判定する
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _overLapBoxOffsetRight + transform.position,
            _overLapBoxSizeVertical,
            0f,
            _layerMaskCheckLR);
        //何かあればtrueを返す
        if (collision.Length != 0)
        {
            return true;
        }
        return false;
    }


    /// <summary> プレイヤーの左に何かあるか </summary>
    bool BodyContactLeft()
    {
        //左に何かあるか判定する
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _overLapBoxOffsetLeft + transform.position,
            _overLapBoxSizeVertical,
            0f,
            _layerMaskCheckLR);
        //何かあればtrueを返す
        if (collision.Length != 0)
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_isGizmo)
        {
            //右のgizmo
            Gizmos.DrawCube(_overLapBoxOffsetRight + transform.position, _overLapBoxSizeVertical);
            //左のgizmo
            Gizmos.DrawCube(_overLapBoxOffsetLeft + transform.position, _overLapBoxSizeVertical);
        }
    }
}
