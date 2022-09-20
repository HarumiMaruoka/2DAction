using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Enemy : Stomper�̃R���|�[�l���g </summary>
public class E_Stomper : EnemyBase
{
    //<=========== �����o�[�ϐ� ===========>//
    [Header("�ړ����x"), SerializeField] float _moveSpeed;
    [Header("�f�o�b�O�pGizmo��\�����邩"), SerializeField] bool _isGizmo;

    /// <summary> �E�ɉ������邩����p�I�[�o�[���b�v�{�b�N�X�̃I�t�Z�b�g </summary>
    [Tooltip("�E�ɉ������Ȃ�������p"), SerializeField]
    Vector3 _overLapBoxOffsetRight;
    /// <summary> ���ɉ������邩����p�I�[�o�[���b�v�{�b�N�X�̃I�t�Z�b�g </summary>
    [Tooltip("���ɉ������Ȃ�������p"), SerializeField]
    Vector3 _overLapBoxOffsetLeft;
    /// <summary> �I�[�o�[���b�v�{�b�N�X�̃T�C�Y </summary>
    [Tooltip("��L�I�[�o�[���b�v�{�b�N�X�̃T�C�Y"), SerializeField]
    Vector2 _overLapBoxSizeVertical;
    /// <summary> �I�[�o�[���b�v�{�b�N�X��LayerMask </summary>
    [SerializeField] LayerMask _layerMask;


    //<=========== Unity���b�Z�[�W ===========>//
    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    // �f�o�b�O�p : �ڐG����pGizmo��\������B
    void OnDrawGizmos()
    {
        // �I�[�o�[���b�v�{�b�N�X��`�悷��
        if (_isGizmo)
        {
            //�F���w�肷��B
            Gizmos.color = Color.red;
            //�E��gizmo
            Gizmos.DrawCube(_overLapBoxOffsetRight + transform.position, _overLapBoxSizeVertical);
            //����gizmo
            Gizmos.DrawCube(_overLapBoxOffsetLeft + transform.position, _overLapBoxSizeVertical);
        }
    }

    //<======== protected�����o�[�֐� ========>//
    /// <summary> Stomper�̈ړ����� : ���ɓ��������B��������Δ��]����B </summary>
    protected override void Move()
    {
        // �����Ă�������֐i�ށB
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

        // �i��ł�������ɉ�������Δ��]����B
        if (BodyContactLeft())
        {
            _isRight = true;
        }
        else if (BodyContactRight())
        {
            _isRight = false;
        }
    }

    //<======== private�����o�[�֐� ========>//
    /// <summary> Physics2D.OverlapBoxAll�𗘗p���āA���ɉ������邩���肷��B </summary>
    /// <returns> ���������true��Ԃ��B </returns>
    bool BodyContactLeft()
    {
        // �R���C�_�[�����ׂĎ擾
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
    /// <summary> Physics2D.OverlapBoxAll�𗘗p���āA�E�ɉ������邩���肷��B </summary>
    /// <returns> ���������true��Ԃ��B </returns>
    bool BodyContactRight()
    {
        // �R���C�_�[�����ׂĎ擾
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
