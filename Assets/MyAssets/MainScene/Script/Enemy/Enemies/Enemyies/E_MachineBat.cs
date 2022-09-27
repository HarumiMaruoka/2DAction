using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Enemy : �}�V���R�E�����̃R���|�[�l���g </summary>
public class E_MachineBat : EnemyBase
{
    //<=========== �����o�[�ϐ� ===========>//
    //�ړ��X�s�[�h
    [Header("�ړ��X�s�[�h"), SerializeField] float _moveSpeed = 120f;
    [Header("X�������̈ړ����~���鋗��"), SerializeField] float _distanceToStopX;
    [Header("Y�������̈ړ����~���鋗��"), SerializeField] float _distanceToStopY;

    //<=========== Unity���b�Z�[�W ===========>//
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }

    //<======== protected�����o�[�֐� ========>//
    /// <summary> �}�V���R�E�����̈ړ����� : �G(�v���C���[)�Ɍ������Ĉړ��������� </summary>
    protected override void Move()
    {
        // �G�l�~�[�̓v���C���[��������������� : �G�̌�����ς���B
        _spriteRenderer.flipX = (transform.position.x < _playerPos.transform.position.x);

        // �m�b�N�o�b�N���łȂ���΁A�v���C���[�Ɍ������Ĉړ�����B
        if (_isMove)
        {
            // Enemy���猩�Ăǂ̕����Ƀv���C���[�����邩���擾����B
            float moveX = _playerPos.transform.position.x - transform.position.x;
            float moveY = _playerPos.transform.position.y - transform.position.y;
            // �������߂����͒�~���邻�̕����̈ړ��́A��~����B
            if (Mathf.Abs(moveX) < _distanceToStopY) moveX = 0f;
            if (Mathf.Abs(moveY) < _distanceToStopX) moveY = 0f;
            // �����Ŗ����ꍇ�̓v���C���[�Ɍ������Ĉړ�����B
            if (!Mathf.Approximately(moveX, 0f)) moveX = (moveX > 0) ? Constants.RIGHT : Constants.LEFT;
            if (!Mathf.Approximately(moveY, 0f)) moveY = (moveY > 0) ? Constants.UP : Constants.DOWN;

            // �ړ�����
            _rigidBody2d.velocity = new Vector2(moveX, moveY).normalized * _moveSpeed * (100f - UseItemManager.Instance._enemyMoveSpeedDownValue) * 0.01f;
            _isRight = _rigidBody2d.velocity.x > 0;
        }
    }
}
