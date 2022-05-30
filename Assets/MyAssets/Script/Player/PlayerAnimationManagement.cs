using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationManagement : MonoBehaviour
{
    //このクラスで利用する、自身の各コンポーネント
    Animator _anim;
    Rigidbody2D _rigidbody2D;
    SpriteRenderer _spriteRenderer;
    Jump_Script _jumpScript;

    //行動可能か？
    public bool _isMove;//行動不能の時はfalseになる。
    public bool _isHitEnemy;//敵に殴られた時にtrueになる。

    //向いている向き
    public bool isRigth;
    public bool isLeft;
    bool isFront; //前向き
    bool isBehind;//後向き

    bool isEnd_of_move_rigth;
    bool isEnd_of_move_left;

    bool _isShotExit;

    bool _isCharge_now;
    bool _isMoveStop;
    public bool _isDead;//倒されたかどうか

    bool _isShotAnim;
    bool _isRunAnim;
    bool _isJumpAnim;
    bool _isFallAnim;
    bool _isFrontAnim;
    bool _isBehindAnim;
    bool _isSingleShot;
    bool _isFrySingleShot;
    bool _isDash;
    bool _isBeatenAnim;//殴られた時のアニメーション
    public bool _isHover;//ホバーしているかどうか


    // Start is called before the first frame update
    //各変数の初期化
    void Start()
    {
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _jumpScript = GetComponent<Jump_Script>();
        _rigidbody2D = GetComponent<Rigidbody2D>();

        //行動可能か？
        _isMove = true;
        _isHitEnemy = false;

        //向いている向き
        isRigth = true;
        isLeft = false;
        isFront = false;
        isBehind = false;

        //移動終了判定
        isEnd_of_move_rigth = true;
        isEnd_of_move_left = true;

        //Chargeしているかどうか
        _isCharge_now = false;

        _isShotAnim = false;
        _isRunAnim = false;
        _isJumpAnim = false;
        _isFallAnim = false;
        _isFrontAnim = false;
        _isBehindAnim = false;
        _isSingleShot = false;
        _isFrySingleShot = false;
        _isDash = false;
        _isBeatenAnim = false;

        _isShotExit = false;
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
                if (h < 0)
                {
                    isEnd_of_move_left = false;
                    isLeft = true;
                    isRigth = false;
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
                //(右移動中であれば)
                if (h > 0)
                {
                    _spriteRenderer.flipX = false;
                    isEnd_of_move_rigth = false;
                    _isRunAnim = true;
                    isLeft = false;
                    isRigth = true;
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

            //縦向き
            if (v < 0)//フロントに向ける処理
            {
                _isFrontAnim = true;
                isFront = false;
                _isRunAnim = false;

            }
            else if (!isFront)
            {
                isFront = true;
                _isFrontAnim = false;
            }

            //ビハインドに向ける処理
            if (v > 0)
            {
                _isBehindAnim = true;
                isBehind = false;
                _isRunAnim = false;

            }
            else if (!isBehind)
            {
                isBehind = true;
                _isBehindAnim = false;
            }

            //ショット mouseButton0 が押されている間実行される
            if (Input.GetButton("Fire1"))
            {
                if (!_jumpScript.GetIsGround())
                {
                    _isFrySingleShot = true;
                }
                else
                {
                    _isShotAnim = true;
                }
                _isShotExit = true;
            }
            else if (_isShotExit)
            {
                _isFrySingleShot = false;
                _isShotAnim = false;
            }

            //スラッシュ mouseButton1 が押されている間実行される
            if (_jumpScript.GetIsGround())
            {
                _isFrySingleShot = false;
            }
            if (Input.GetButton("Fire2"))
            {
                _isShotAnim = true;
                _isCharge_now = true;
            }
            else if (_isCharge_now)
            {
                _isCharge_now = false;
                _isShotAnim = false;
            }

            //ジャンプ
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
            else if(_jumpScript.GetIsGround())
            {
                _isFallAnim = false;
                _isHover = false;
            }

            //殴られた時の処理
            if (_isHitEnemy)
            {
                _isHitEnemy = false;
                _isMove = false;
                _anim.Play("Beaten");
                _anim.SetTrigger("Blinking");
                _isBeatenAnim = true;
            }
        }

        //倒されたとき
        if (_isDead)
        {
            _anim.Play("Killed");
        }

        //Animetion Set
        SetAnim();
    }

    //アニメーション用変数を設定する関数。毎フレーム呼ばれる。
    void SetAnim()
    {
        _anim.SetBool("isShot", _isShotAnim);
        _anim.SetBool("isRun", _isRunAnim);
        _anim.SetBool("isJump", _isJumpAnim);
        _anim.SetBool("isFall", _isFallAnim);
        _anim.SetBool("isFront", _isFrontAnim);
        _anim.SetBool("isBehind", _isBehindAnim);
        _anim.SetBool("isSingelShot", _isSingleShot);
        _anim.SetBool("isFrySingleShot", _isFrySingleShot);
        _anim.SetBool("isDash", _isDash);
        _anim.SetBool("isBeaten", _isBeatenAnim);
        _anim.SetBool("isHover", _isHover);
    }

    //プレイヤーが行動不能状態から復帰する時の処理。敵に殴られた時のアニメーションイベントから呼ばれる。
    public void ChibiRoboComeback()
    {
        _isMove = true;
        _isBeatenAnim = false;
    }
}
