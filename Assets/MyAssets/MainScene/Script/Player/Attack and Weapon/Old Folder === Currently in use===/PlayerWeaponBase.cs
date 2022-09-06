using UnityEngine;

/// <summary> ����̊��N���X </summary>
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public abstract class PlayerWeaponBase : MonoBehaviour
{
    //<====== �e�R���|�[�l���g ======>//
    protected Rigidbody2D _rigidBody2D;
    protected Collider2D _collider2D;
    protected Animator _animator;
    /// <summary> ���g�̕���ID </summary>
    protected int _myWeaponID;
    [Header("�v���C���[�̈ʒu����̃I�t�Z�b�g"), SerializeField]
    protected Vector3 _offsetFromPlayerPosition;
    /// <summary> �v���C���[�̃|�W�V���� </summary>
    protected Transform _playerPos;
    /// <summary>
    /// �������Ɏ��s���镐�킩? : 
    /// ���̕ϐ���true�̏ꍇ�A���̕���́A�������̂ݎ��s���镐��B
    /// false�̏ꍇ�A�����Ă���Ԃ����Ǝ��s���ׂ�����B 
    /// </summary>
    protected bool _isPressType;

    /// <summary> ����̏��������� : �I�[�o�[���C�h�\ </summary>
    protected virtual void WeaponInit() { _playerPos = transform.parent; }

    /// <summary> �U�����s���� : Fire�{�^���������ꂽ���̏����B : �I�[�o�[���C�h�� </summary>
    public virtual void Run_FireProcess() { }

    /// <summary> ���̃I�u�W�F�N�g���A�N�e�B�u�ɂȂ������Ɏ��s���ׂ��֐��B : �I�[�o�[���C�h�� </summary>
    protected virtual void OnEnable_ThisWeapon() { }

    /// <summary> �A�b�v�f�[�g�֐��B : �I�[�o�[���C�h�� </summary>
    protected virtual void Update_ThisClass() { }

    /// <summary> �K�v�ł���Έړ�����B : �I�[�o�[���C�h�� </summary>
    public virtual void Move() { }

    /// <summary> ���g�ƃv���C���[�̌������`�F�b�N���A�K�v�ł���Ό�����ύX����B : �����͑��̃R���|�[�l���g�ɂ��K�p�������̂ŃX�P�[���ŊǗ�����B </summary>
    protected void DirectionCheck()
    {
        //�����ɂ���ă��[�e�[�V������ς���B
        //�v���C���[���E�Ɍ����Ă��鎞 ���A����I�u�W�F�N�g�������Ă���������������̂Ƃ��A����I�u�W�F�N�g�������Ă�������𔽓]������B
        if (PlayerStatusManager.Instance.IsRight && transform.localScale.x < 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        //�v���C���[�����Ɍ����Ă��鎞 ���A����I�u�W�F�N�g�������Ă���������E�����̂Ƃ��A����I�u�W�F�N�g�������Ă�������𔽓]������B
        else if (!PlayerStatusManager.Instance.IsRight && transform.localScale.x > 0)
        {
            Vector3 localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
    }
    /// <summary> �ʒu���X�V����B </summary>
    /// <returns> �X�V��̈ʒu </returns>
    protected Vector3 UpdatePosition() { return transform.position = _playerPos.position + _offsetFromPlayerPosition; }
}
