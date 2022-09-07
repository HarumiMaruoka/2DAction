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
        if (!base.Initialize_Enemy())
        {
            Debug.LogError($"初期化に失敗しました。{gameObject.name}");
        }
    }
    protected override void Update()
    {
        base.Update();
        Move();
    }

    //<======== protectedメンバー関数 ========>//
    /// <summary> マシンコウモリの移動処理 : 敵(プレイヤー)に向かって移動し続ける </summary>
    protected override void Move()
    {
        // エネミーはプレイヤーがいる方向を向く : 絵の向きを変える。
        _spriteRenderer.flipX = (transform.position.x < _playerPos.transform.position.x);

        // ノックバック中でなければ、プレイヤーに向かって移動する。
        if (!_isMove)
        {
            // Enemyから見てどの方向にプレイヤーがいるかを取得する。
            float moveX = _playerPos.transform.position.x - transform.position.x;
            float moveY = _playerPos.transform.position.y - transform.position.y;
            // 距離が近い時は停止するその方向の移動は、停止する。
            if (moveX < _distanceToStop) moveX = 0f;
            if (moveY < _distanceToStop) moveY = 0f;
            // そうで無い場合はプレイヤーに向かって移動する。
            if (!Mathf.Approximately(moveX, 0f)) moveX = (moveX > 0) ? _moveSpeed : -_moveSpeed;
            if (!Mathf.Approximately(moveY, 0f)) moveY = (moveY > 0) ? _moveSpeed : -_moveSpeed;

            // 移動処理
            _rigidBody2d.velocity = new Vector2(moveX, moveY).normalized;
        }
    }
}
