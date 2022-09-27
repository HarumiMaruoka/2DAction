using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Enemy : マシンコウモリのコンポーネント </summary>
public class E_MachineBat : EnemyBase
{
    //<=========== メンバー変数 ===========>//
    //移動スピード
    [Header("移動スピード"), SerializeField] float _moveSpeed = 120f;
    [Header("X軸方向の移動を停止する距離"), SerializeField] float _distanceToStopX;
    [Header("Y軸方向の移動を停止する距離"), SerializeField] float _distanceToStopY;

    //<=========== Unityメッセージ ===========>//
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }

    //<======== protectedメンバー関数 ========>//
    /// <summary> マシンコウモリの移動処理 : 敵(プレイヤー)に向かって移動し続ける </summary>
    protected override void Move()
    {
        // エネミーはプレイヤーがいる方向を向く : 絵の向きを変える。
        _spriteRenderer.flipX = (transform.position.x < _playerPos.transform.position.x);

        // ノックバック中でなければ、プレイヤーに向かって移動する。
        if (_isMove)
        {
            // Enemyから見てどの方向にプレイヤーがいるかを取得する。
            float moveX = _playerPos.transform.position.x - transform.position.x;
            float moveY = _playerPos.transform.position.y - transform.position.y;
            // 距離が近い時は停止するその方向の移動は、停止する。
            if (Mathf.Abs(moveX) < _distanceToStopY) moveX = 0f;
            if (Mathf.Abs(moveY) < _distanceToStopX) moveY = 0f;
            // そうで無い場合はプレイヤーに向かって移動する。
            if (!Mathf.Approximately(moveX, 0f)) moveX = (moveX > 0) ? Constants.RIGHT : Constants.LEFT;
            if (!Mathf.Approximately(moveY, 0f)) moveY = (moveY > 0) ? Constants.UP : Constants.DOWN;

            // 移動処理
            _rigidBody2d.velocity = new Vector2(moveX, moveY).normalized * _moveSpeed * (100f - UseItemManager.Instance._enemyMoveSpeedDownValue) * 0.01f;
            _isRight = _rigidBody2d.velocity.x > 0;
        }
    }
}
