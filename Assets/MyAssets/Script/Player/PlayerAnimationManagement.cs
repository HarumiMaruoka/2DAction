using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManagement : MonoBehaviour
{
    //このクラスで利用する、自身の各コンポーネント
    Animator _animator;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    JumpScript _jumpScript;
    InputManager _inputManager;

    //行動可能か？
    public bool _isMove { get; set; }//行動不能の時はfalseになる。
    public bool _isHitEnemy;//敵に殴られた時にtrueになる。

    bool isEnd_of_move_rigth;
    bool isEnd_of_move_left;

    public bool _isDead { get; set; }//倒されたかどうか

    //アニメーション管理用変数
    bool _isAttack;
    bool _isAttack2;
    bool _isRunAnim;
    bool _isJumpAnim;
    bool _isFallAnim;
    bool _isFrontAnim;
    bool _isBehindAnim;
    bool _isFryAttack;
    bool _isFryAttack2;
    bool _isDash;
    bool _isBeatenAnim;//殴られた時のアニメーション
    public bool _isHover;//ホバーしているかどうか
    public bool _isClimb;//梯子を昇降しているかどうか


    // Start is called before the first frame update
    //各変数の初期化
    void Start()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jumpScript = GetComponent<JumpScript>();
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _inputManager = GetComponent<InputManager>();

        //行動可能か？
        _isMove = true;
        _isHitEnemy = false;

        //移動終了判定
        isEnd_of_move_rigth = true;
        isEnd_of_move_left = true;

        _isAttack = false;
        _isAttack2 = false;
        _isRunAnim = false;
        _isJumpAnim = false;
        _isFallAnim = false;
        _isFrontAnim = false;
        _isBehindAnim = false;
        _isFryAttack = false;
        _isFryAttack2 = false;
        _isDash = false;
        _isBeatenAnim = false;
        _isClimb = false;

        _isDead = false;
    }


    // Update is called once per frame
    void Update()
    {
        //入力を受け付ける
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (!_isDead)
        {
            //横移動
            if (v == 0 && _isMove)
            {
                //水平方向移動向きが正であれば以下を実行せよ
                //(左移動中の場合)
                if (h < 0)
                {
                    isEnd_of_move_left = false;
                    _spriteRenderer.flipX = true;
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
                //左移動が解除されたら
                else if (!isEnd_of_move_left)
                {
                    _isDash = false;
                    isEnd_of_move_left = true;
                    _isRunAnim = false;
                }

                //水平方向移動向きが正であれば以下を実行せよ
                //(右移動中の場合)
                if (h > 0)
                {
                    _spriteRenderer.flipX = false;
                    isEnd_of_move_rigth = false;
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

            if (_inputManager._inputVertical != 0)
            {
                _isRunAnim = false;
                _isDash = false;
            }

            //スライディング
            SlidingAnim();

            //ショット mouseButton0 が押されている間実行される
            if (Input.GetButton("Fire1"))
            {
                if (_jumpScript.GetIsGround())
                {
                    _isAttack = true;
                }
                else
                {
                    _isFryAttack = true;
                }
            }
            else
            {
                _isFryAttack = false;
                _isAttack = false;
            }

            //スラッシュ mouseButton1 が押されている間実行される
            if (Input.GetButton("Fire2"))
            {
                if (_jumpScript.GetIsGround())
                {
                    _isAttack2 = true;
                }
                else
                {
                    _isFryAttack2 = true;
                }
            }
            else
            {
                _isFryAttack2 = false;
                _isAttack2 = false;
            }

            _isAttack = (_isAttack2 || _isAttack);
            _isFryAttack = (_isFryAttack2 || _isFryAttack);

            //ジャンプ/ホバー
            if (_rigidbody2D.velocity.y > 0 && !_jumpScript.GetIsGround())//非接地かつ上昇中
            {
                _isJumpAnim = true;
                //ホバーする場合
                if (Input.GetButtonDown("Jump"))
                {
                    _isHover = true;
                }
                else if (Input.GetButtonUp("Jump"))
                {
                    _isHover = false;
                }
            }
            else if (_jumpScript.GetIsGround())//接地中
            {
                _isJumpAnim = false;
                _isHover = false;
            }

            //降下
            if (_rigidbody2D.velocity.y < 0 && !_jumpScript.GetIsGround())//非接地かつ下降中
            {
                _isFallAnim = true;
                //ホバーする場合
                if (Input.GetButtonDown("Jump"))
                {
                    _isHover = true;
                }
                else if (Input.GetButtonUp("Jump"))
                {
                    _isHover = false;
                }
            }
            else if (_jumpScript.GetIsGround())
            {
                _isFallAnim = false;
                _isHover = false;
            }

            //殴られた時の処理
            if (_isHitEnemy)
            {
                _isHitEnemy = false;
                _isMove = false;
                _animator.Play("Beaten");
                _animator.SetTrigger("Blinking");
                _isBeatenAnim = true;
            }

            //梯子昇降
            ClimbAnim();
        }

        //倒されたとき
        if (_isDead)
        {
            _animator.Play("Killed");
        }

        //Animetion Set
        SetAnim();
    }

    //アニメーション用変数を設定する関数。毎フレーム呼ばれる。
    void SetAnim()
    {
        _animator.SetBool("isAttack", _isAttack);
        _animator.SetBool("isRun", _isRunAnim);
        _animator.SetBool("isJump", _isJumpAnim);
        _animator.SetBool("isFall", _isFallAnim);
        _animator.SetBool("isFront", _isFrontAnim);
        _animator.SetBool("isBehind", _isBehindAnim);
        _animator.SetBool("isFlyAttack", _isFryAttack);
        _animator.SetBool("isDash", _isDash);
        _animator.SetBool("isBeaten", _isBeatenAnim);
        _animator.SetBool("isHover", _isHover);
        _animator.SetBool("isClimb", _isClimb);
    }

    //プレイヤーが行動不能状態から復帰する時の処理。敵に殴られた時のアニメーションイベントから呼ばれる。
    public void ChibiRoboComeback()
    {
        _isMove = true;
        _isBeatenAnim = false;
    }

    /// <summary> スライディング </summary>
    void SlidingAnim()
    {
        //下入力がある時かつ、接地状態で、スペースキーを押すことでスライディング！
        if (Input.GetAxisRaw("Vertical") < 0 && _jumpScript.GetIsGround())
        {
            if (Input.GetButtonDown("Jump"))
            {
                _animator.Play("Sliding");
            }
        }
    }

    /// <summary> 梯子の登り降り </summary>
    void ClimbAnim()
    {
        //梯子を昇降する時の処理
        if (_isClimb)
        {
            //縦の入力がある時
            if (_inputManager._inputVertical != 0)
            {
                //昇降中
                if (_inputManager._inputVertical != 0)
                {
                    _isJumpAnim = false;
                    _isFallAnim = false;
                    _isClimb = true;
                    _animator.SetFloat("ClimbSpeed", 1f);
                }
                //着地したとき
                if (_jumpScript.GetIsGround())
                {
                    _isClimb = false;
                }
            }
            //縦の入力がなくなった時の処理
            else
            {
                //アニメーションを一時停止する
                _animator.SetFloat("ClimbSpeed", 0f);
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
                _animator.Play("Climb");
            }
        }
    }

    //コライダーが接触しているときの処理
    private void OnTriggerExit2D(Collider2D collision)
    {
        //梯子から離れたときの処理
        if (collision.tag == "Ladder")
        {
            _isClimb = false;
        }
    }

}
