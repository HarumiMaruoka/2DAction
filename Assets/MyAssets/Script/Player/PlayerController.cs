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
    PlayerAnimationManagement _playerAnimationManagement;
    PlayerBasicInformation _playerBasicInformation;

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

    [SerializeField] float _hoverPower;

    [SerializeField] float _gasValue;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        jump_script = GetComponent<Jump_Script>();
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

            //ジャンプ
            if (v == 0)
            {
                jump_script.Jump(jump_power);
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

            v = 100f;
        }
    }

    /// <summary> ホバー管理用関数 </summary>
    void HoverManagement()//ホバー管理
    {
        //ホバーしているときの処理
        if (_playerAnimationManagement._isHover)
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
}
