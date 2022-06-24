using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_Stomper : EnemyBase
{
    [SerializeField] float _moveSpeed;
    [SerializeField] bool _isGizmo;

    //�E�ɉ������邩����p
    [Tooltip("�E�ɉ������Ȃ�������p"), SerializeField]
    private Vector3 _overLapBoxOffsetRight;
    //���ɉ������邩����p
    [Tooltip("���ɉ������Ȃ�������p"), SerializeField]
    private Vector3 _overLapBoxOffsetLeft;
    //��L�̃T�C�Y
    [Tooltip("��L�̃T�C�Y"), SerializeField]
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

    //���ɓ�������
    protected override void Move()
    {
        //�����Ă�������֐i��
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
            Debug.Log("�E�ɐi��");
            _isRight = true;
        }
        else if(BodyContactRight())
        {
            Debug.Log("���ɐi��");
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
            //�E��gizmo
            Gizmos.DrawCube(_overLapBoxOffsetRight + transform.position, _overLapBoxSizeVertical);
            //����gizmo
            Gizmos.DrawCube(_overLapBoxOffsetLeft + transform.position, _overLapBoxSizeVertical);
        }
    }

    bool BodyContactLeft()
    {
        //���ɉ������邩���肷��
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _overLapBoxOffsetLeft + transform.position,
            _overLapBoxSizeVertical,
            0f,
            _layerMask);
        //���������true��Ԃ�
        if (collision.Length != 0)
        {
            return true;
        }
        return false;
    }

    bool BodyContactRight()
    {
        //�E�ɉ������邩���肷��
        Collider2D[] collision = Physics2D.OverlapBoxAll(
            _overLapBoxOffsetRight + transform.position,
            _overLapBoxSizeVertical,
            0f,
            _layerMask);
        //���������true��Ԃ�
        if (collision.Length != 0)
        {
            return true;
        }
        return false;
    }
}
