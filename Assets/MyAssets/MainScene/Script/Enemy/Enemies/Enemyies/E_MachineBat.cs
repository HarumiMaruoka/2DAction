using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Enemy : �}�V���R�E�����̃R���|�[�l���g </summary>
public class E_MachineBat : EnemyBase
{
    //<=========== �����o�[�ϐ� ===========>//
    //�ړ��X�s�[�h
    [Header("�ړ��X�s�[�h"), SerializeField] float _moveSpeed = 120f;
    [Header("�Z�������̈ړ����~���鋗�� : �ǂ����Œ�~���Ȃ��ƁA�Ԃ�Ԃ邷��̂�"), SerializeField] float _distanceToStop;

    //<=========== Unity���b�Z�[�W ===========>//
    void Start()
    {
        base.Initialize_Enemy();
    }
    void Update()
    {
        Update_Enemy();
        Move();
    }

    //<======== protected�����o�[�֐� ========>//
    /// <summary> �}�V���R�E�����̈ړ����� : �G(�v���C���[)�Ɍ������Ĉړ��������� </summary>
    protected override void Move()
    {
        //�G�l�~�[�̓v���C���[��������������� : �G�̌�����ς���B
        _spriteRenderer.flipX = (transform.position.x < _playerPos.transform.position.x);

        //�m�b�N�o�b�N���łȂ���΁A�v���C���[�Ɍ������Ĉړ�����B
        if (!_isKnockBackNow)
        {
            //�v���C���[�ƃG�l�~�[�̈ʒu�̍����擾����
            float moveX = _playerPos.transform.position.x - transform.position.x;
            float moveY = _playerPos.transform.position.y - transform.position.y;

            // ��Βl�����
            moveX = (moveX > 0) ? _moveSpeed : -_moveSpeed;
            moveY = (moveY > 0) ? _moveSpeed : -_moveSpeed;

            // �������߂����͒�~���邻�̕����̈ړ��́A��~����B
            if (moveX < _distanceToStop) moveX = 0f;
            if (moveY < _distanceToStop) moveY = 0f;

            //���@���ŋ@�B�I�Ȉړ���\���������̂�velocity�ňړ�����
            _rigidBody2d.velocity = new Vector2(moveX / 50, moveY / 150);
        }
        else//�m�b�N�o�b�N����
        {
            // ���x�����Z�b�g
            _rigidBody2d.velocity = Vector2.zero;
            // �m�b�N�o�b�N����������w�肷��
            Vector2 knockBack = _spriteRenderer.flipX ? new Vector2(-1, 1) : new Vector2(1, 1);
            // �w�肳�ꂽ�����ɗ͂������� : ***** ���̂܂܂��ƃm�b�N�o�b�N�������Ɨ͂�������̂ň�x����������悤�ɂ��邩��������B *****
            _rigidBody2d.AddForce(knockBack, ForceMode2D.Impulse);
        }
    }
}
