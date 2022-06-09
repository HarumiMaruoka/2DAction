using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Stomper : EnemyBase
{
    [SerializeField] float _moveSpeed;
    void Start()
    {
        base.EnemyInitialize();
    }

    void Update()
    {
        NeedEnemyElement();
        Move();
    }

    //横に動くだけ
    protected override void Move()
    {
        //向いている方向へ進む
        if (_isRight)
        {
            _spriteRenderer.flipX = _isRight;
            _rigidBody2d.velocity = new Vector2(1 * _moveSpeed, _rigidBody2d.velocity.y);
        }
        else
        {
            _spriteRenderer.flipX = _isRight;
            _rigidBody2d.velocity = new Vector2(-1 * _moveSpeed, _rigidBody2d.velocity.y);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _isRight ^= true;
    }
}
