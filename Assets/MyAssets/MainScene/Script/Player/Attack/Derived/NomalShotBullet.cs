using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 基本射撃攻撃コンポーネント </summary>
public class NomalShotBullet : PlayerWeaponBase
{
    //<======= メンバー変数 =======>//
    [Header("弾の速度"), SerializeField] float _bulletSpeed = 10f;

    /// <summary> 基本射撃攻撃クラス独自の初期化処理 </summary>
    protected override void WeaponInit()
    {
        base.WeaponInit();
        //IDを設定する。
        _myWeaponID = (int)ArmParts.WeaponID.WeaponID_00_NomalShotAttack;
        //PressTypeを設定する。
        _isPressType = false;
    }

    /// <summary> Fireボタンが押されたときの処理 : デリゲート変数に登録し呼び出す。 </summary>
    public override void Run_FireProcess()
    {
        //このコンポーネントがアタッチされているゲームオブジェクトをアクティブにする。
        gameObject.SetActive(true);
        //アニメーションを再生する。 : アニメーションパラメータを設定する。
        _animator.SetInteger("WeaponID", _myWeaponID);
        //プレイヤーが向いている方向に一直線に飛んで行く
        _rigidBody2D.velocity = Vector2.right * (PlayerStatusManager.Instance.IsRight ? _bulletSpeed : -_bulletSpeed);
    }

    /// <summary> オブジェクトがアクティブになった時の処理 </summary>
    private void OnEnable()
    {
        OnEnable_ThisWeapon();
    }

    /// <summary> オブジェクトが非アクティブになった時の処理 </summary>
    private void OnDisable()
    {

    }
}
