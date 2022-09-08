using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 通常射撃攻撃クラス
/// </summary>
public class BasicShooting : FireBehavior
{
    //<===== メンバー変数 =====>//
    [Header("弾のプレハブ"), SerializeField] GameObject _bulletPrefab = default;
    /// <summary> 発射間隔 </summary>
    [Header("発射間隔"), SerializeField] float _fireInterval = 1f;
    /// <summary> 弾を撃てるかどうかを表す変数。 </summary>
    bool _isFire = true;

    //<===== Unityメッセージ =====>//
    void Start()
    {
        Initialized(Constants.ON_FIRE_PRESS_TYPE_CONSECUTIVELY);

        //***テスト用処理***//
        //*左腕に装備する。*//
        SetEquip_LeftArm();
        //******************//
    }

    //<===== overrides =====>//
    protected override bool Initialized(bool pressType)
    {
        return base.Initialized(pressType);
    }
    protected override void OnFire_ThisWeapon()
    {
        // 攻撃可能であれば実行する
        if (_isFire)
        {
            // 弾を生成する。
            Instantiate(_bulletPrefab, transform);
            // インターバルを待つ。
            StartCoroutine(WaitInterval());
        }
    }

    //<===== コルーチン =====>//
    /// <summary> 射撃のインターバルを待つ。 </summary>
    IEnumerator WaitInterval()
    {
        _isFire = false;
        yield return new WaitForSeconds(_fireInterval);
        _isFire = true;
    }
}
