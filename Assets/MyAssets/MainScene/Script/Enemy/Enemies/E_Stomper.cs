using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Stomper : EnemyBase
{
    [SerializeField] float _moveSpeed;
    [SerializeField] bool _isGizmo;

    //右に何があるか判定用
    [Tooltip("右に何かがないか判定用"), SerializeField]
    private Vector3 _overLapBoxOffsetRight;
    //左に何があるか判定用
    [Tooltip("左に何かがないか判定用"), SerializeField]
    private Vector3 _overLapBoxOffsetLeft;
    //上記のサイズ
    [Tooltip("上記のサイズ"), SerializeField]
    private Vector2 _overLapBoxSizeVertical;

    [SerializeField] LayerMask _layerMask;

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

        if (BodyContactLeft())
        {
            Debug.Log("右に進め");
            _isRight = true;
        }
        else if(BodyContactRight())
        {
            Debug.Log("左に進め");
            _isRight = false;
        }
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    _isRight ^= true;
    //}

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_isGizmo)
        {
            //右のgizmo
            Gizmos.DrawCube(_overLapBoxOffsetRight + transform.position, _overLapBoxSizeVertical);
            //左のgizmo
            Gizmos.DrawCube(_overLapBoxOffsetLeft + transform.position, _overLapBoxSizeVertical);
        }
    }

    bool BodyContactLeft()
    {
        //左に何かあるか判定する
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _overLapBoxOffsetLeft + transform.position,
            _overLapBoxSizeVertical,
            0f,
            _layerMask);
        //何かあればtrueを返す
        if (collision.Length != 0)
        {
            return true;
        }
        return false;
    }

    bool BodyContactRight()
    {
        //右に何かあるか判定する
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _overLapBoxOffsetRight + transform.position,
            _overLapBoxSizeVertical,
            0f,
            _layerMask);
        //何かあればtrueを返す
        if (collision.Length != 0)
        {
            return true;
        }
        return false;
    }
}
