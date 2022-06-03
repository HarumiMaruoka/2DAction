using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�R�E�����}�V���̃R�[�h
public class E_MachineBat : EnemyBase
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
        //�v���C���[������������擾����
        _isRight = (transform.position.x < _playerPos.transform.position.x);
        //�G�l�~�[�̓v���C���[���������������
        _spriteRenderer.flipX = _isRight;

        if (!_isKnockBackNow)//�m�b�N�o�b�N���łȂ���Έړ�����B
        {
            //�v���C���[�ƃG�l�~�[�̈ʒu���擾����
            Vector3 pv = _playerPos.transform.position;
            Vector3 ev = transform.position;

            //�v���C���[�ƃG�l�~�[�̈ʒu�̍����擾����
            float moveX = pv.x - ev.x;
            float moveY = pv.y - ev.y;

            // ���Z�������ʂ��}�C�i�X�ł����X�͌��Z�����B�v���C���[����������ɂ���Č������ς���
            moveX = (moveX > 0) ? moveSpeed : -moveSpeed;

            // ���Z�������ʂ��}�C�i�X�ł����Y�͌��Z����
            moveY = (moveY > 0) ? moveSpeed : -moveSpeed;

            //���@���ŋ@�B�I�Ȉړ���\���������̂�velocity�ňړ�����
            _rigidBody2d.velocity = new Vector2(moveX / 50, moveY / 150);
        }
        else//�m�b�N�o�b�N����
        {
            _rigidBody2d.velocity = new Vector2(0f, 0f);
            Vector2 knockBack = _spriteRenderer.flipX ? new Vector2(-1, 1) : new Vector2(1, 1);
            _rigidBody2d.AddForce(knockBack, ForceMode2D.Impulse);
        }
    }
}
