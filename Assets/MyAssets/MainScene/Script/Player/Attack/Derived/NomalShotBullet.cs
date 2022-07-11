using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NomalShotBullet : PlayerWeaponBase
{
    //�e�͎w��̕����ɔ�Ԃ���
    [Header("�e�̑��x"), SerializeField] float _bulletMoveSpeedX;

    void Start()
    {
        WeaponInit();
        gameObject.SetActive(false);
    }

    void Update()
    {
        
    }

    public override void Move()
    {
        base.Move();
    }

    /// <summary>�@�A�N�e�B�u�ɂȂ������̏��� </summary>
    private void OnEnable()
    {
        Vector2 dir = Vector2.zero;
        if (PlayerManager.Instance.IsRight)
        {
            dir = Vector2.right;
        }
        else
        {
            dir = Vector2.left;
        }
        _rigidBody2D.velocity = dir * _bulletMoveSpeedX;
    }
}
