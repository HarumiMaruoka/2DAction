using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//プレイヤーのアニメーションに関するクラス
public class ChangePlayerState : MonoBehaviour
{
    //このクラスで利用する、自身の各コンポーネント
    Animator anim;
    Rigidbody2D rigidbody2D;
    SpriteRenderer spriteRenderer;
    Jump_Script jump_script;

    //行動可能か？
    public bool isMove;
    public bool isHitEnemy;


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
    void Start()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        jump_script = GetComponent<Jump_Script>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //行動可能か？
        isMove = true;
        isHitEnemy = false;

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
        //Run animation

        //入力を受け付ける
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (!_isDead)
        {
            //横移動
            if (v == 0 && isMove)
            {
                if (h < 0)
                {
                    isEnd_of_move_left = false;
                    isLeft = true;
                    isRigth = false;
                    spriteRenderer.flipX = true;
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
                    spriteRenderer.flipX = false;
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

            if (v > 0)//ビハインドに向ける処理
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

            //ショット
            float fire2 = Input.GetAxisRaw("Fire2");

            if (Input.GetButton("Fire1"))
            {
                if (!jump_script.GetIsGround())
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
            if (jump_script.GetIsGround())
            {
                _isFrySingleShot = false;
            }
            if (fire2 == 1)
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
            if (rigidbody2D.velocity.y > 0 && !jump_script.GetIsGround())//非接地かつ上昇中
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
            else if (jump_script.GetIsGround())//接地中
            {
                _isJumpAnim = false;
                _isHover = false;
            }

            //降下
            if (rigidbody2D.velocity.y < 0 && !jump_script.GetIsGround())//非接地かつ下降中
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
            else if(jump_script.GetIsGround())
            {
                _isFallAnim = false;
                _isHover = false;
            }

            //殴られた時の処理
            if (isHitEnemy)
            {
                isHitEnemy = false;
                isMove = false;
                anim.Play("Beaten");
                anim.SetTrigger("Blinking");
                _isBeatenAnim = true;
            }
        }
        //倒されたとき
        if (_isDead)
        {
            anim.Play("Killed");
        }

        //Animetion Set
        SetAnim();
    }

    void SetAnim()
    {
        anim.SetBool("isShot", _isShotAnim);
        anim.SetBool("isRun", _isRunAnim);
        anim.SetBool("isJump", _isJumpAnim);
        anim.SetBool("isFall", _isFallAnim);
        anim.SetBool("isFront", _isFrontAnim);
        anim.SetBool("isBehind", _isBehindAnim);
        anim.SetBool("isSingelShot", _isSingleShot);
        anim.SetBool("isFrySingleShot", _isFrySingleShot);
        anim.SetBool("isDash", _isDash);
        anim.SetBool("isBeaten", _isBeatenAnim);
        anim.SetBool("isHover", _isHover);
    }

    public void ChibiRoboComeback()
    {
        isMove = true;
        _isBeatenAnim = false;
    }
}
