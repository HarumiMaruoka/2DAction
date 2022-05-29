using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//プレイヤーの移動に関するクラス
public class PlayerController : MonoBehaviour
{
    public float move_speed_x;
    float previous_move_speed;
    const float dash_speed = 1.6f;
    public float jump_power;
    Rigidbody2D _rigidbody2D;
    Jump_Script jump_script;
    ChangePlayerState change_player_state;

    [SerializeField] GameObject _burrettPrefab;
    [SerializeField] GameObject _slashPrefabOne;
    //[SerializeField] GameObject _slashPrefabTow;
    float _burrettCoolTime;
    float _slashCoolTime;

    [SerializeField] float _hoverPower;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        jump_script = GetComponent<Jump_Script>();
        change_player_state = GetComponent<ChangePlayerState>();
        _burrettCoolTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (change_player_state._isMove)//行動可能であれば実行する
        {
            //入力を取得する
            float h = Input.GetAxisRaw("Horizontal");//横方向
            float v = Input.GetAxisRaw("Vertical");//縦方向

            //横移動
            if (v == 0)//縦の入力がある時は横移動できない
            {
                if (h != 0)
                {
                    if (Input.GetButton("Dash"))
                    {
                        _rigidbody2D.AddForce(Vector2.right * h * move_speed_x * Time.deltaTime * dash_speed, ForceMode2D.Force);
                    }
                    else
                    {
                        //rigidbody2D.AddForceでの移動
                        _rigidbody2D.AddForce(Vector2.right * h * move_speed_x * Time.deltaTime, ForceMode2D.Force);

                        //velocityで移動する版
                        //rigidbody2D.velocity = Vector2.right * h * move_speed_x * dash_speed;


                    }
                }
            }

            //ジャンプ
            jump_script.Jump(jump_power);

            //ホバー
            if (change_player_state._isHover)
            {
                _rigidbody2D.velocity = Vector2.up * _hoverPower;
            }

            //ショット
            if (v == 0 && !change_player_state._isHover)//縦の入力がある時は打てない、ホバー中も打てない
            {
                //ショット
                if (_burrettCoolTime <= 0f)
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
            if (v == 0 && !change_player_state._isHover)//縦の入力がある時は打てない、ホバー中も打てない
            {
                if (_slashCoolTime <= 0f)
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        Instantiate(_slashPrefabOne, Vector3.zero, Quaternion.identity);
                        _slashCoolTime = 0.25f;
                    }
                }
                else
                {
                    if (_slashCoolTime >= 0f)
                    {
                        _slashCoolTime -= Time.deltaTime;
                    }
                }
            }
        }
    }
}
