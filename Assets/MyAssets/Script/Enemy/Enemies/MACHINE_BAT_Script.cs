using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�R�E�����}�V���̃R�[�h
public class MACHINE_BAT_Script : EnemyBase
{
    //�ړ��X�s�[�h
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


    //�v���C���[�Ɍ������Ĉړ���������
    protected override void Move()
    {
        if (!_isKnockBackNow)//�m�b�N�o�b�N���łȂ���Ύ��s����
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


            // ���Z�������ʂ��}�C�i�X�ł����X�͌��Z����
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

            // ���Z�������ʂ��}�C�i�X�ł����Y�͌��Z����
            if (p_vY < 0)
            {
                vy = -moveSpeed;
            }
            else
            {
                vy = moveSpeed;
            }
            //���@���ŋ@�B�I�Ȉړ���\���������̂�velocity�ňړ�����
            _rigidBody2d.velocity = new Vector2(vx / 50, vy / 150);
        }
        else//�m�b�N�o�b�N����
        {
            _rigidBody2d.velocity = new Vector2(0f, 0f);
            Vector2 knockBack = _spriteRenderer.flipX ? new Vector2(-1, 1) : new Vector2(1, 1);
            _rigidBody2d.AddForce(knockBack, ForceMode2D.Impulse);
        }
    }
}
