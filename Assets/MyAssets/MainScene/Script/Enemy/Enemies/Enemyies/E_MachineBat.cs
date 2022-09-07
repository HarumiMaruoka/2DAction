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
        if (!base.Initialize_Enemy())
        {
            Debug.LogError($"�������Ɏ��s���܂����B{gameObject.name}");
        }
    }
    protected override void Update()
    {
        base.Update();
        Move();
    }

    //<======== protected�����o�[�֐� ========>//
    /// <summary> �}�V���R�E�����̈ړ����� : �G(�v���C���[)�Ɍ������Ĉړ��������� </summary>
    protected override void Move()
    {
        // �G�l�~�[�̓v���C���[��������������� : �G�̌�����ς���B
        _spriteRenderer.flipX = (transform.position.x < _playerPos.transform.position.x);

        // �m�b�N�o�b�N���łȂ���΁A�v���C���[�Ɍ������Ĉړ�����B
        if (!_isMove)
        {
            // Enemy���猩�Ăǂ̕����Ƀv���C���[�����邩���擾����B
            float moveX = _playerPos.transform.position.x - transform.position.x;
            float moveY = _playerPos.transform.position.y - transform.position.y;
            // �������߂����͒�~���邻�̕����̈ړ��́A��~����B
            if (moveX < _distanceToStop) moveX = 0f;
            if (moveY < _distanceToStop) moveY = 0f;
            // �����Ŗ����ꍇ�̓v���C���[�Ɍ������Ĉړ�����B
            if (!Mathf.Approximately(moveX, 0f)) moveX = (moveX > 0) ? _moveSpeed : -_moveSpeed;
            if (!Mathf.Approximately(moveY, 0f)) moveY = (moveY > 0) ? _moveSpeed : -_moveSpeed;

            // �ړ�����
            _rigidBody2d.velocity = new Vector2(moveX, moveY).normalized;
        }
    }
}
