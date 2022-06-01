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

    [Tooltip("ホバー中のガソリン消費")]
    [SerializeField] float _gasValue;
    public float GasValue { get => _gasValue; }

    [Tooltip("梯子の昇降速度")]
    [SerializeField] float _climbSpeed;
    public float ClimbSpeed { get => _climbSpeed; }

    [Tooltip("梯子昇降中の横移動速度の減速率")]
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
    PlayerAnimationManagement _playerAnimationManagement;
    PlayerBasicInformation _playerBasicInformation;
    Animator _animator;

    //このフレームで加える力
    Vector2 _newForce;
    Vector2 _newImpulse;

    //ジャンプできるか:スペースキーはジャンプとスライディングの機能を兼ねる為
    bool _canJump = false;
    //プレイヤーが向いている方向
    bool _isRigth = false;
    //梯子を登れるかどうか
    bool _isClimb = false;

    //重力
    float _gravity;


    void Start()
    {
        //コンポーネントの初期化
        _rigidBody2D = GetComponent<Rigidbody2D>();
        _inputManager = GetComponent<InputManager>();
        _jumpScript = GetComponent<JumpScript>();
        _spriteRendere = GetComponent<SpriteRenderer>();
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();
        _playerAnimationManagement = GetComponent<PlayerAnimationManagement>();
        _animator = GetComponent<Animator>();
        _gravity = _rigidBody2D.gravityScale;

        //各変数の初期化
        _newForce = Vector2.zero;
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
        //プレイヤーが向いている方向を取得
        _isRigth = !_spriteRendere.flipX;


        if (_playerAnimationManagement._isMove)
        {
            MoveHorizontal();
            Dash();
            Sliding();
            Jump();
            Climb();
            Hover();

            //Debug.Log(newVelocity);
            //実際に力を加える
            _rigidBody2D.AddForce(_newImpulse * 10f, ForceMode2D.Impulse);
            _rigidBody2D.AddForce(_newForce * 10f * Time.deltaTime * 100f, ForceMode2D.Force);
            //Debug.Log("newImpulse" + _newImpulse);
            //Debug.Log("newForce" + _newForce);
        }
    }

    void MoveHorizontal()
    {
        //縦の入力がある時は実行できない
        if (!(_inputManager._inputVertical != 0))
        {
            _newForce += new Vector2(_inputManager._inputHorizontal * MoveSpeedX, 0f);
        }
    }

    void Dash()
    {
        //LeftShiftキーで加速
        if (_inputManager._inputLeftShift)
        {
            _newForce *= _moveDashSpeed;
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
                _canJump = false;//スライディングする場合はジャンプできない
                _playerAnimationManagement._isMove = false;
            }
        }
    }

    void Jump()
    {
        //接地かつスペースキーでジャンプ
        if (_jumpScript.GetIsGround() && _inputManager._inputJumpDown && _canJump)
        {
            _newImpulse += new Vector2(0f, JumpPower);
        }
    }

    void Climb()
    {
        if (_isClimb)
        {
            if (_inputManager._inputVertical != 0)
            {
                //_newForce += new Vector2(0f, ClimbSpeed * _inputManager._inputVertical);
                _rigidBody2D.velocity = Vector2.up * ClimbSpeed * _inputManager._inputVertical;
                _newForce.x *= DecelerationRateX;
                _rigidBody2D.gravityScale = _gravity;
            }
            else
            {
                _rigidBody2D.velocity = Vector2.zero;
                _rigidBody2D.gravityScale = 0f;
            }
        }
    }

    void Hover()
    {
        //ホバー中の処理。スライディング中ではないときに実行できる。
        if (_playerAnimationManagement._isHover)
        {
            //ホバー用体力が0より大きければ
            if (_playerBasicInformation._hoverValue > 0f)
            {
                //上昇処理、ガスを消費する
                _newForce += Vector2.up * _hoverPower;
                _playerBasicInformation._hoverValue -= Time.deltaTime * _gasValue;
            }
        }
        //ホバーしてないときの処理
        else
        {
            //体力が最大であれば
            if (_playerBasicInformation._maxHealthForHover <= _playerBasicInformation._hoverValue)
            {
                _playerBasicInformation._hoverValue = _playerBasicInformation._maxHealthForHover;
            }
            //体力が最大ではなければ
            else
            {
                _playerBasicInformation._hoverValue += Time.deltaTime * _gasValue;
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

    private void OnTriggerExit2D(Collider2D collision)
    {
        //梯子と接触しているときの処理
        if (collision.tag == "Ladder")
        {
            _isClimb = false;
            _rigidBody2D.gravityScale = _gravity;
        }
    }

    //この関数はアニメーションイベントから呼び出す
    /// <summary> スライディング終了関数 </summary>
    public void SlidingStop()
    {
        _playerAnimationManagement._isMove = true;
        _animator.SetTrigger("SlidingOff");
    }
}
