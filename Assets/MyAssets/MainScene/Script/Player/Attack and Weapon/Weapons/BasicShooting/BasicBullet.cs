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
    [Header("消滅までの時間"), SerializeField] float _dethTime = 1.5f;

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
            Vector2.right * _moveSpeed * (PlayerStatusManager.Instance.IsRight ? LEFT : RIGHT) + Vector2.right * GameObject.Find("ChibiRobo").GetComponent<Rigidbody2D>().velocity.x;
        if (PlayerStatusManager.Instance.IsRight)
        {
            var l = transform.localScale;
            l.x *= -1;
            transform.localScale = l;
        }
        // 発射音を鳴らす
        AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, transform.position);
        // デストロイするまで待つ
        StartCoroutine(WaitDestroy());

        // 現在コルーチンを使用して弾を消失させているが、
        // 弾を少しづつ小さくして消失させるという表現の方がよいと思うので、
        // それをDOTweenを使用して実装しよう。

        return true;
    }

    //<===== コルーチン =====>//
    IEnumerator WaitDestroy()
    {
        yield return new WaitForSeconds(_dethTime);
        Destroy(gameObject);
    }
}
