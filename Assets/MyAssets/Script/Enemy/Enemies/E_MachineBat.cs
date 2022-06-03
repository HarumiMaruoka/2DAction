using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//コウモリマシンのコード
public class E_MachineBat : EnemyBase
{
    //移動スピード
    [SerializeField]float moveSpeed = 120f;

    // Start is called before the first frame update
    void Start()
    {
        base.Enemy_Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        NeedEnemyElement();
        Move();
    }

    //プレイヤーに向かって移動し続ける
    protected override void Move()
    {
        //プレイヤーがいる方向を取得する
        _isRight = (transform.position.x < _playerPos.transform.position.x);
        //エネミーはプレイヤーがいる方向を向く
        _spriteRenderer.flipX = _isRight;

        if (!_isKnockBackNow)//ノックバック中でなければ移動する。
        {
            //プレイヤーとエネミーの位置を取得する
            Vector3 pv = _playerPos.transform.position;
            Vector3 ev = transform.position;

            //プレイヤーとエネミーの位置の差を取得する
            float moveX = pv.x - ev.x;
            float moveY = pv.y - ev.y;

            // 減算した結果がマイナスであればXは減算処理。プレイヤーがいる方向によって向きも変える
            moveX = (moveX > 0) ? moveSpeed : -moveSpeed;

            // 減算した結果がマイナスであればYは減算処理
            moveY = (moveY > 0) ? moveSpeed : -moveSpeed;

            //無機質で機械的な移動を表現したいのでvelocityで移動する
            _rigidBody2d.velocity = new Vector2(moveX / 50, moveY / 150);
        }
        else//ノックバック処理
        {
            _rigidBody2d.velocity = new Vector2(0f, 0f);
            Vector2 knockBack = _spriteRenderer.flipX ? new Vector2(-1, 1) : new Vector2(1, 1);
            _rigidBody2d.AddForce(knockBack, ForceMode2D.Impulse);
        }
    }
}
