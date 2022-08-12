using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Enemy : マシンコウモリのコンポーネント </summary>
public class E_MachineBat : EnemyBase
{
    //<=========== メンバー変数 ===========>//
    //移動スピード
    [Header("移動スピード"), SerializeField] float _moveSpeed = 120f;
    [Header("〇軸方向の移動を停止する距離 : どっかで停止しないと、ぶるぶるするので"), SerializeField] float _distanceToStop;

    //<=========== Unityメッセージ ===========>//
    void Start()
    {
        base.Initialize_Enemy();
    }
    void Update()
    {
        Update_Enemy();
        Move();
    }

    //<======== protectedメンバー関数 ========>//
    /// <summary> マシンコウモリの移動処理 : 敵(プレイヤー)に向かって移動し続ける </summary>
    protected override void Move()
    {
        //エネミーはプレイヤーがいる方向を向く : 絵の向きを変える。
        _spriteRenderer.flipX = (transform.position.x < _playerPos.transform.position.x);

        //ノックバック中でなければ、プレイヤーに向かって移動する。
        if (!_isKnockBackNow)
        {
            //プレイヤーとエネミーの位置の差を取得する
            float moveX = _playerPos.transform.position.x - transform.position.x;
            float moveY = _playerPos.transform.position.y - transform.position.y;

            // 絶対値を取る
            moveX = (moveX > 0) ? _moveSpeed : -_moveSpeed;
            moveY = (moveY > 0) ? _moveSpeed : -_moveSpeed;

            // 距離が近い時は停止するその方向の移動は、停止する。
            if (moveX < _distanceToStop) moveX = 0f;
            if (moveY < _distanceToStop) moveY = 0f;

            //無機質で機械的な移動を表現したいのでvelocityで移動する
            _rigidBody2d.velocity = new Vector2(moveX / 50, moveY / 150);
        }
        else//ノックバック処理
        {
            // 速度をリセット
            _rigidBody2d.velocity = Vector2.zero;
            // ノックバックする方向を指定する
            Vector2 knockBack = _spriteRenderer.flipX ? new Vector2(-1, 1) : new Vector2(1, 1);
            // 指定された方向に力を加える : ***** 今のままだとノックバック中ずっと力を加えるので一度だけ加えるようにするか検討する。 *****
            _rigidBody2d.AddForce(knockBack, ForceMode2D.Impulse);
        }
    }
}
