using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class PlayerWeaponBase : MonoBehaviour
{
    //<====== ���g�̊e�R���|�[�l���g ======>//
    protected Rigidbody2D _rigidBody2D;
    protected Collider2D _collider2D;
    protected Animator _animator;
    /// <summary> ���g�̕���ID </summary>
    protected int _myWeaponID;
    [Header("�A�N�e�B�u�ɂȂ����u�Ԃ̈ʒu�I�t�Z�b�g"), SerializeField] protected Vector3 _positionOffsetAtBirth;
    /// <summary> �������Ɏ��s���镐�킩? : ���̕ϐ���true�̏ꍇ�A���̕���́A�������̂ݎ��s���镐��Bfalse�̏ꍇ�A�����Ă���Ԃ����Ǝ��s���ׂ�����B </summary>
    protected bool _isPressType;

    /// <summary> ����̏��������� : �I�[�o�[���C�h�\ </summary>
    protected virtual void WeaponInit()
    {
        //�R���|�[�l���g���擾
        _animator = GetComponent<Animator>();
        _collider2D = GetComponent<Collider2D>();
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    /// <summary> �U�����s���� : Fire�{�^���������ꂽ���̏����B : �I�[�o�[���C�h�� </summary>
    public virtual void Run_FireProcess()
    {
        //���̃R���|�[�l���g���A�^�b�`����Ă���Q�[���I�u�W�F�N�g���A�N�e�B�u�ɂ���B
        gameObject.SetActive(true);
        //�A�j���[�V�������Đ�����B : �A�j���[�V�����p�����[�^��ݒ肷��B
        _animator.SetInteger("WeaponID", _myWeaponID);
    }

    /// <summary> �A�N�e�B�u�ɂȂ������̏��� : �I�[�o�[���C�h�� </summary>
    protected virtual void OnEnable_ThisWeapon()
    {
        //�����ɂ���ă��[�e�[�V������ς���B
        //�v���C���[���E�Ɍ����Ă��鎞 ���A�I�u�W�F�N�g�������Ă���������E�̎�
        if (PlayerStatusManager.Instance.IsRight && transform.rotation.x > 0)
        {

        }
        //�ʒu��ݒ肷��B
        transform.position = transform.position + _positionOffsetAtBirth;
    }

    /// <summary> �K�v�ł���Έړ�����B : �I�[�o�[���C�h�� </summary>
    public virtual void Move() { }

    /// <summary> �U���J�n�̏����B : �I�[�o�[���C�h�� </summary>
    protected virtual void OnStart_ThisAttack()
    {
        //���̃Q�[���I�u�W�F�N�g���A�N�e�B�u�ɂ���B
        gameObject.SetActive(true);
    }
    /// <summary> �U���I���̏��� : �I�[�o�[���C�h�� </summary>
    protected virtual void OnEnd_ThisAttack()
    {
        //���̃Q�[���I�u�W�F�N�g���A�N�e�B�u�ɂ���B
        gameObject.SetActive(false);
    }

    /// <summary> ���̃R���|�[�l���g���A�N�e�B�u�ɂ���B : �I�[�o�[���C�h�� </summary>
    protected virtual void Activate_ThisComponen() { enabled = true; }
    /// <summary> ���̃R���|�[�l���g���A�N�e�B�u�ɂ���B : �I�[�o�[���C�h�� </summary>
    protected virtual void DeactivateThisComponent() { enabled = false; }

    /// <summary> ���̃Q�[���I�u�W�F�N�g�ɃA�^�b�`���ꂽ�R���C�_�[���A�N�e�B�u�ɂ���B : �I�[�o�[���C�h�� </summary>
    protected virtual void ActivateCollider() { _collider2D.enabled = true; }
    /// <summary> ���̃Q�[���I�u�W�F�N�g�ɃA�^�b�`���ꂽ�R���C�_�[���A�N�e�B�u�ɂ���B : �I�[�o�[���C�h�� </summary>
    protected virtual void DeactivateCollider() { _collider2D.enabled = false; }
}
