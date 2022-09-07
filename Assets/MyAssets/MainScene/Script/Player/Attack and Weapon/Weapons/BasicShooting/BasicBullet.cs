using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BasicShootingが放つ弾が持つコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class BasicBullet : LongRangeWeaponBase
{
    [Header("弾の速度"), SerializeField] float _moveSpeed = 5f;
    [Header("ダッシュ時の加速度"), SerializeField] float _dashAcceleration = 1.5f;

    const float RIGHT = 1f;
    const float LEFT = -1f;

    void Start()
    {
        Initialized();
    }

    protected override bool Initialized()
    {
        //プレイヤーの向きに応じて飛んでいく方向を決める。
        GetComponent<Rigidbody2D>().velocity =
            Vector2.right * _moveSpeed * (PlayerStatusManager.Instance.IsRight ? RIGHT : LEFT);

        return true;
    }
}
