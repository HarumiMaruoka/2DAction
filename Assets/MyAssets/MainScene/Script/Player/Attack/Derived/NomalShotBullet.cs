using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalShotBullet : PlayerWeaponBase
{
    //弾は指定の方向に飛ぶだけ
    [Header("弾の速度"), SerializeField] float _bulletMoveSpeedX;

    void Start()
    {
        WeaponInit();
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public override void Move()
    {
        base.Move();
    }

    /// <summary>　アクティブになった時の処理 </summary>
    private void OnEnable()
    {
        Vector2 dir = Vector2.zero;
        if (PlayerManager.Instance.IsRight)
        {
            dir = Vector2.right;
        }
        else
        {
            dir = Vector2.left;
        }
        _rigidBody2D.velocity = dir * _bulletMoveSpeedX;
    }
}
