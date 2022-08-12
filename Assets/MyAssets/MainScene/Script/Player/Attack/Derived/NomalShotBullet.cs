using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 基本射撃攻撃コンポーネント </summary>
public class NomalShotBullet : PlayerWeaponBase
{
    //<======= メンバー変数 =======>//
    [Header("弾の速度"), SerializeField] float _bulletSpeed = 10f;
    [Header("射撃のインターバル"), SerializeField] float _interval = 1f;
    [Header("撃てるかどうかの真偽値"), SerializeField] bool _isFire = true;
    [Header("弾のプレハブ"), SerializeField] GameObject _bulletPrefab;

    protected override void WeaponInit()
    {
        //親(プレイヤー)のトランスフォームを取得する。
        _playerPos = transform.parent;
        //武器IDを設定する。
        _myWeaponID = (int)ArmParts.WeaponID.WeaponID_00_NomalShotAttack;
        //PressTypeを設定する。
        _isPressType = false;
    }

    private void Start()
    {
        WeaponInit();
    }

    /// <summary> Fireボタンが押されている間の処理 : この関数は、デリゲート変数に登録し呼び出す。 </summary>
    public override void Run_FireProcess()
    {
        //位置を更新する。
        UpdatePosition();
        //発射処理を行う。
        if (_isFire && _bulletPrefab)
        {
            Instantiate(_bulletPrefab, transform.position, Quaternion.identity);
            StartCoroutine(WaitForInterval());
        }
    }

    /// <summary> インターバルを待つ。 </summary>
    IEnumerator WaitForInterval()
    {
        _isFire = false;
        yield return new WaitForSeconds(_interval);
        _isFire = true;
    }
}
