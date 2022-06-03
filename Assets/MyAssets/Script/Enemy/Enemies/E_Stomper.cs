using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Stomper : EnemyBase
{
    [SerializeField] float _moveSpeed;
    void Start()
    {
        base.Enemy_Initialize();
    }

    void Update()
    {
        NeedEnemyElement();
        Move();
    }

    //���ɓ�������
    protected override void Move()
    {
        //�����Ă�������֐i��
        if (_isRight)
        {
            _rigidBody2d.velocity = Vector2.right * _moveSpeed;
        }
        else
        {
            _rigidBody2d.velocity = Vector2.left * _moveSpeed;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag=="Brock")
        {
            _isRight ^= true;
        }
    }
}
