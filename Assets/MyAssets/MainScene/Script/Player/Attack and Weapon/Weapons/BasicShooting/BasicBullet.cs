using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BasicShootingが放つ弾のコントローラー
/// </summary>
public class BasicBullet : LongRangeWeaponBase
{
    [Header("弾の速度"), SerializeField] float _moveSpeed = 5f;
    [Header("ダッシュ時の加速度"), SerializeField] float _dashAcceleration = 1.5f;


    void Start()
    {

    }

    void Update()
    {

    }
}
