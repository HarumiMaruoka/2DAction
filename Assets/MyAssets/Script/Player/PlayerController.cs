using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//プレイヤーの移動に関するクラス
public class PlayerController : MonoBehaviour
{
    //各パラメータ
    public float move_speed_x;
    float previous_move_speed;
    const float dash_speed = 1.6f;
    public float jump_power;

    //コンポーネント
    Rigidbody2D _rigidbody2D;
    Jump_Script _jumpScript;
    SpriteRenderer _spriteRenderer;
    Animator _animator;
    PlayerAnimationManagement _playerAnimationManagement;
    PlayerBasicInformation _playerBasicInformation;

    //武器のプレハブ
    [SerializeField] GameObject _burrettPrefab;

    [Tooltip("右クリック攻撃 一撃目のプレハブ")]
    [SerializeField] GameObject _slashPrefabOne;

    [Tooltip("右クリック攻撃 二撃目のプレハブ")]
    [SerializeField] GameObject _slashPrefabTwo;

    GameObject _temporary;//仮の入れ物

    //[SerializeField] GameObject _slashPrefabTow;
    float _burrettCoolTime;
    float _slashCoolTimeOne;
    float _slashCoolTimeTow;

    //ホバー関連
    [SerializeField] float _hoverPower;//ホバー時の上昇力
    [SerializeField] float _gasValue;//1フレーム単位のガスの消費量

    //スライディング関連
    [SerializeField] float _slidingPower;
    public bool _isSlidingNow;

    //梯子昇降関連
    [Tooltip("梯子を登るスピード")]
    [SerializeField] float _climbSpeed;
    bool _isclimb;
    float _climbGravityScale = 0;
    float _notClimbGravityScale = 12;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _jumpScript = GetComponent<Jump_Script>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _playerAnimationManagement = GetComponent<PlayerAnimationManagement>();
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();
        _burrettCoolTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerAnimationManagement._isMove)//行動可能であれば実行する
        {
            //入力を取得する
            float h = Input.GetAxisRaw("Horizontal");//横方向
            float v = Input.GetAxisRaw("Vertical");//縦方向

            //横移動
            if (v == 0 && h != 0)//縦の入力がある時は横移動できない
            {
                if (Input.GetButton("Dash"))
                {
                    _rigidbody2D.AddForce(Vector2.right * h * move_speed_x * Time.deltaTime * dash_speed, ForceMode2D.Force);
                }
                else
                {
                    _rigidbody2D.AddForce(Vector2.right * h * move_speed_x * Time.deltaTime, ForceMode2D.Force);
                }
            }

            //スライディング
            Sliding(v);

            //ジャンプ
            if (v == 0 && !_isSlidingNow)//縦入力がある時、あるいはスライディング中は実行できない
            {
                _jumpScript.Jump(jump_power);
            }

            //ホバー
            HoverManagement();

            //ショット
            if (v == 0 && !_playerAnimationManagement._isHover)//縦の入力がある時は打てない、ホバー中も打てない
            {
                if (_burrettCoolTime < 0f)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        Instantiate(_burrettPrefab, Vector3.zero, Quaternion.identity);
                        _burrettCoolTime = 0.15f;
                    }
                }
                else if (_burrettCoolTime >= 0f)
                {
                    _burrettCoolTime -= Time.deltaTime;
                }
            }

            //スラッシュ
            if (v == 0 && !_playerAnimationManagement._isHover)//縦の入力がある時は打てない、ホバー中も打てない
            {
                //二撃目発動が発動できるなら発動する
                if (_slashCoolTimeTow < 0f && _slashCoolTimeOne > 0f)
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        Destroy(_temporary);
                        Instantiate(_slashPrefabTwo, Vector3.zero, Quaternion.identity);
                        _slashCoolTimeTow = 0.35f;
                        _slashCoolTimeOne = 0.35f;
                    }
                }

                //一撃目
                if (_slashCoolTimeOne < 0f)
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        _temporary = Instantiate(_slashPrefabOne, Vector3.zero, Quaternion.identity);
                        _slashCoolTimeOne = 0.35f;
                    }
                }

                //クールタイム解消
                if (_slashCoolTimeOne >= 0f)
                {
                    _slashCoolTimeOne -= Time.deltaTime;
                }
                if (_slashCoolTimeTow >= 0f)
                {
                    _slashCoolTimeTow -= Time.deltaTime;
                }
            }

            //梯子昇降
            Climb(v, h);
        }
    }

    /// <summary> ホバー管理用関数 </summary>
    void HoverManagement()//ホバー管理
    {
        //ホバー中の処理。スライディング中ではないときに実行できる。
        if (_playerAnimationManagement._isHover && !_isSlidingNow)
        {
            //ホバー用体力が0より大きければ
            if (_playerBasicInformation._hoverValue > 0f)
            {
                //上昇処理、ガスを消費する
                _rigidbody2D.velocity = Vector2.up * _hoverPower;
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

    /// <summary> スライディング </summary>
    void Sliding(float inputVertical)
    {
        //下入力がある時かつ、接地状態で、スペースキーを押すことでスライディング！
        if (inputVertical < 0 && _jumpScript.GetIsGround())
        {
            if (Input.GetButtonDown("Jump"))
            {
                //左に向いている時
                if (_spriteRenderer.flipX)
                {
                    _rigidbody2D.AddForce(Vector2.left * _slidingPower, ForceMode2D.Impulse);
                }
                //右に向いている時
                else
                {
                    _rigidbody2D.AddForce(Vector2.right * _slidingPower, ForceMode2D.Impulse);
                }
                _isSlidingNow = true;
                _playerAnimationManagement._isMove = false;
            }
        }
    }

    /// <summary> アニメーションイベントから呼び出す、スライディング終了用関数 </summary>
    public void SlidingStop()
    {
        _isSlidingNow = false;
        _playerAnimationManagement._isMove = true;
        _animator.SetTrigger("SlidingOff");
    }

    /// <summary> 梯子昇降管理用関数 </summary>
    void Climb(float v, float h)
    {
        //梯子昇降中
        if (_isclimb)
        {
            _rigidbody2D.gravityScale = _climbGravityScale;
            _rigidbody2D.velocity = new Vector2(h * move_speed_x * Time.deltaTime, _climbSpeed * v);
        }
        else
        {
            _rigidbody2D.gravityScale = _notClimbGravityScale;
        }
        if (_jumpScript.GetIsGround())
        {
            _rigidbody2D.gravityScale = _notClimbGravityScale;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //梯子と接触しているときの処理
        if (collision.tag == "Ladder")
        {
            //梯子を登る場合
            if (Input.GetAxisRaw("Vertical") != 0)
            {
                _isclimb = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //梯子と接触しているときの処理
        if (collision.tag == "Ladder")
        {
            _isclimb = false;
        }
    }
}
