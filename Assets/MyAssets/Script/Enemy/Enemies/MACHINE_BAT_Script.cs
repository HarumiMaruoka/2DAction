using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//コウモリマシンのコード
public class MACHINE_BAT_Script : EnemyBase
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
        if (!_isKnockBackNow)//ノックバック中でなければ実行する
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


            // 減算した結果がマイナスであればXは減算処理
            if (p_vX < 0)
            {
                vx = -moveSpeed;
                _spriteRenderer.flipX = false;
            }
            else
            {
                vx = moveSpeed;
                _spriteRenderer.flipX = true;
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
            //無機質で機械的な移動を表現したいのでvelocityで移動する
            _rigidBody2d.velocity = new Vector2(vx / 50, vy / 150);
        }
        else//ノックバック処理
        {
            _rigidBody2d.velocity = new Vector2(0f, 0f);
            Vector2 knockBack = _spriteRenderer.flipX ? new Vector2(-1, 1) : new Vector2(1, 1);
            _rigidBody2d.AddForce(knockBack, ForceMode2D.Impulse);
        }
    }
}
