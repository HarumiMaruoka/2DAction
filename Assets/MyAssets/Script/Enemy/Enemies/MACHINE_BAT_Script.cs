using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�R�E�����}�V���̃R�[�h
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


    //�v���C���[�Ɍ������Ĉړ���������
    protected override void Move()
    {
        if (!_isKnockBackNow)
        {
            //�v���C���[�ƃG�l�~�[�̈ʒu���擾����
            Vector3 pv = _playerPos.transform.position;
            Vector3 ev = transform.position;

            //�v���C���[�ƃG�l�~�[�̈ʒu�̍����擾����
            float p_vX = pv.x - ev.x;
            float p_vY = pv.y - ev.y;

            //���ۂɈړ�����p�̕ϐ�
            float vx = 0f;
            float vy = 0f;

            //�ړ��X�s�[�h
            float moveSpeed = 120f;

            // ���Z�������ʂ��}�C�i�X�ł����X�͌��Z����
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

            // ���Z�������ʂ��}�C�i�X�ł����Y�͌��Z����
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
