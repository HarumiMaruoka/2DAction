using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //コンポーネント
    InputManager _inputManager;
    PlayerStateManagement _playerStateManagement;

    //バレット関連
    [SerializeField] float _bulletCoolTime = 0.15f;
    float _bulletCoolTimeValue = -1f;
    [SerializeField] GameObject _bulletPrefab;

    //スラッシュ関連
    GameObject _slashOne;
    GameObject _slashTwo;
    [SerializeField] float _slashCoolTime = 0.35f;
    float _slashCoolTimeValueOne = -1f;
    float _slashCoolTimeValueTwo = -1f;

    void Start()
    {
        _inputManager = GetComponent<InputManager>();
        _playerStateManagement = GetComponent<PlayerStateManagement>();
        _slashOne = transform.GetChild(0).gameObject;
        _slashTwo = transform.GetChild(1).gameObject;
    }


    void Update()
    {
        if (!_playerStateManagement._isDead && _playerStateManagement._isMove)
        {
            Fire1();
            Fire2();
        }
    }

    void Fire1()
    {
        //左クリックでショット
        if (!(_playerStateManagement._playerState == PlayerStateManagement.PlayerState.HOVER))//ホバー中は打てない
        {
            if (_bulletCoolTimeValue < 0f)
            {
                if (_inputManager._inputFire1)
                {
                    Instantiate(_bulletPrefab, Vector3.zero, Quaternion.identity);
                    _bulletCoolTimeValue = _bulletCoolTime;
                }
            }
            else if (_bulletCoolTime >= 0f)
            {
                _bulletCoolTimeValue -= Time.deltaTime;
            }
        }
    }

    void Fire2()
    {
        //右クリックでスラッシュ
        if (!(_playerStateManagement._playerState == PlayerStateManagement.PlayerState.HOVER))//ホバー中も打てない
        {
            //二撃目発動が発動できるなら発動する
            if (_slashCoolTimeValueTwo < 0f && _slashCoolTimeValueOne > 0f)
            {
                if (_inputManager._inputFire2Down)
                {
                    _slashTwo.SetActive(true);
                    _slashCoolTimeValueTwo = _slashCoolTime;
                    _slashCoolTimeValueOne = _slashCoolTime;
                }
            }

            //一撃目
            if (_slashCoolTimeValueOne < 0f)
            {
                if (_inputManager._inputFire2Down)
                {
                    _slashOne.SetActive(true);
                    _slashCoolTimeValueOne = _slashCoolTime;
                }
            }

            //クールタイム解消
            if (_slashCoolTimeValueOne >= 0f)
            {
                _slashCoolTimeValueOne -= Time.deltaTime;
            }
            if (_slashCoolTimeValueTwo >= 0f)
            {
                _slashCoolTimeValueTwo -= Time.deltaTime;
            }
        }

    }
}
