using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//コウモリマシンのコード
public class MACHINE_BAT_Script : EnemyBase
{
    protected Rigidbody2D _rigidBody2d;


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
        if (!_isKnockBackNow)
        {
            //プレイヤーとエネミーの位置を取得する
            Vector3 pv = _playerPos.transform.position;
            Vector3 ev = transform.position;

            //プレイヤーとエネミーの位置の差を取得する
            float p_vX = pv.x - ev.x;
            float p_vY = pv.y - ev.y;

            //実際に移動する用の変数
            float vx = 0f;
            float vy = 0f;

            //移動スピード
            float moveSpeed = 120f;

            // 減算した結果がマイナスであればXは減算処理
            if (p_vX < 0)
            {
                vx = -moveSpeed;
                sprite_renderer.flipX = false;
            }
            else
            {
                vx = moveSpeed;
                sprite_renderer.flipX = true;
            }

            // 減算した結果がマイナスであればYは減算処理
            if (p_vY < 0)
            {
                vy = -moveSpeed;
            }
            else
            {
                vy = moveSpeed;
            }

            _rigidbody2d.velocity = new Vector2(vx / 50, vy / 150);
        }
    }
}
