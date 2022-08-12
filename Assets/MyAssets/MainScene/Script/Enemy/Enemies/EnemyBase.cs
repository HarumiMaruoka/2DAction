using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //<============= �����o�[�ϐ� =============>//
    //�G�l�~�[���ʂ̃X�e�[�^�X
    /// <summary> �q�b�g�|�C���g </summary>
    [SerializeField] protected int _hitPoint;
    /// <summary> �U���� </summary>
    [SerializeField] protected int _offensive_Power;
    /// <summary> ������΂��� </summary>
    [SerializeField] protected Vector2 _blowingPower;
    /// <summary> �d�� : ������΂���ɂ��� </summary>
    [SerializeField] public float _weight;

    //���̃G�l�~�[�������Ă������
    protected bool _isRight;

    //�v���C���[�̃R���|�[�l���g
    protected GameObject _player;
    protected Transform _playerPos;
    protected PlayerBasicInformation _playerBasicInformation;
    protected Rigidbody2D _playersRigidBody2D;
    protected PlayerMoveManager _playerMoveManager;

    //���g�̃R���|�[�l���g
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rigidBody2d;

    //�F�ύX�p
    /// <summary> �F��ς��邩�ǂ��� </summary>
    protected bool _isColorChange = false;
    /// <summary> �F��ύX���鎞�� </summary>
    protected float _colorChangeTime = 0.1f;
    /// <summary> �F��ύX���鎞�� : ���݂̎c�莞�� </summary>
    protected float _colorChangeTimeValue = 0;

    //�m�b�N�o�b�N�֘A
    /// <summary> ���݃m�b�N�o�b�N�����ǂ��� </summary>
    public bool _isKnockBackNow;
    /// <summary> �m�b�N�o�b�N���� </summary>
    float _knockBackModeTime = 0f;


    //<============== protected�����o�[�֐� ==============>//
    /// <summary> �S�G�l�~�[�ŋ��ʂ�Enemy�̏������֐��B�p�����Start�֐��ŌĂяo���B </summary>
    /// <returns> ���������� true ��Ԃ��B </returns>
    protected bool Initialize_Enemy()
    {
        if (EnemyInitialize_Get_PlayerComponents())
        {
            Debug.LogError($"�������Ɏ��s���܂����B : {gameObject.name}");
            return false;
        }
        if (EnemyInitialize_Get_ThisGameObjectComponents())
        {
            Debug.LogError($"�������Ɏ��s���܂����B : {gameObject.name}");
            return false;
        }

        return true;
    }
    //�S�G�l�~�[�ŋ��ʂ�Enemy��Update�֐��B�p�����Update�֐��ŌĂяo��
    protected void Update_Enemy()
    {
        //�̗͂��Ȃ��Ȃ������̏���
        if (_hitPoint <= 0)
        {
            //�̗͂��Ȃ��Ȃ�������ł���
            Destroy(this.gameObject);
        }

        //�F��ς���K�v������Ες���
        if (_isColorChange)
        {
            _spriteRenderer.color = Color.red;
            _isColorChange = false;
        }
        //�F�����ɖ߂�
        else if (_colorChangeTimeValue < 0)
        {
            _spriteRenderer.color = new Color(255, 255, 255, 255);
        }
        //�N�[���^�C������
        else if (_colorChangeTimeValue > 0)
        {
            _colorChangeTimeValue -= Time.deltaTime;
        }
    }


    //<============= private�֐� =============>//
    //******************** �������֘A ********************//
    /// <summary> �v���C���[�̃R���|�[�l���g���擾 </summary>
    /// <returns> ���������� true ��Ԃ��B </returns>
    bool EnemyInitialize_Get_PlayerComponents()
    {
        //�v���C���[�̏����擾
        _player = GameObject.Find("ChibiRobo");
        if (_player == null)
        {
            Debug.LogError($"Player���擾�ł��܂���ł����B : {gameObject.name}");
            return false;
        }
        _playerBasicInformation = _player.GetComponent<PlayerBasicInformation>();
        if (_playerBasicInformation == null)
        {
            Debug.LogError($"Player��PlayerBasicInformation�R���|�[�l���g���擾�ł��܂���ł����B : {gameObject.name}");
            return false;
        }
        _playerPos = _player.GetComponent<Transform>();
        if (_playerPos == null)
        {
            Debug.LogError($"Player��Transform�R���|�[�l���g���擾�ł��܂���ł����B : {gameObject.name}");
            return false;
        }
        _playersRigidBody2D = _player.GetComponent<Rigidbody2D>();
        if (_playersRigidBody2D == null)
        {
            Debug.LogError($"Player��_playersRigidBody2D�R���|�[�l���g���擾�ł��܂���ł����B : {gameObject.name}");
            return false;
        }
        _playerMoveManager = _player.GetComponent<PlayerMoveManager>();
        if (_playerMoveManager == null)
        {
            Debug.LogError($"Player��PlayerMoveManager�R���|�[�l���g���擾�ł��܂���ł����B : {gameObject.name}");
            return false;
        }

        return true;
    }
    /// <summary> ���̃Q�[���I�u�W�F�N�g�ɃA�^�b�`����Ă���R���|�[�l���g���擾����B </summary>
    /// <returns> ���������� true ��Ԃ��B </returns>
    bool EnemyInitialize_Get_ThisGameObjectComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError($"SpriteRenderer�̎擾�Ɏ��s���܂����B : {gameObject.name}");
            return false;
        }
        _rigidBody2d = GetComponent<Rigidbody2D>();
        if (_rigidBody2d == null)
        {
            Debug.LogError($"Rigidbody2D�̎擾�Ɏ��s���܂����B : {gameObject.name}");
            return false;
        }

        return true;
    }


    //<============= public�֐� =============>//
    //******************** �U���q�b�g�֘A ********************//
    /// <summary> �v���C���[����̍U������ : �m�b�N�o�b�N������ </summary>
    /// <param name="damage"> �_���[�W�� </param>
    public void HitPlayerAttadk(int damage)
    {
        //���g�̗̑͂����炵�A��莞�� �F���_���p�̐F�ɕς���B
        _hitPoint -= damage;
        _isColorChange = true;
        _colorChangeTimeValue = _colorChangeTime;
    }
    /// <summary> �v���C���[����̍U������ : �m�b�N�o�b�N�L��� </summary>
    /// <param name="damage"> �_���[�W�� </param>
    /// <param name="knockBackTimer"> �m�b�N�o�b�N���� </param>
    public void HitPlayerAttadk(int damage, float knockBackTimer)
    {
        //���g�̗̑͂����炵�A0.1�b�����F��Ԃɕς���B
        _hitPoint -= damage;
        _isColorChange = true;
        _colorChangeTimeValue = _colorChangeTime;

        //�m�b�N�o�b�N����B�v���C���[�̃m�b�N�o�b�N��(����) - �G�l�~�[�̑ϋv��(����)���AMove���~����B
        _knockBackModeTime = (knockBackTimer - _weight) > 0f ? (knockBackTimer - _weight) : 0f;
        StartCoroutine(KnockBackMode());
    }
    /// <summary> �v���C���[�ɑ΂���U������ : �I�[�o�[���C�h�� </summary>
    public virtual void HitPlayer()
    {
        //�v���C���[��HitPoint�����炷
        PlayerStatusManager.Instance.PlayerHealthPoint -= _offensive_Power;
        _playersRigidBody2D.velocity = Vector2.zero;
        //�v���C���[���m�b�N�o�b�N����
        if (_isRight)
        {
            _playersRigidBody2D.AddForce((Vector2.right + Vector2.up) * _blowingPower, ForceMode2D.Impulse);
        }
        else
        {
            _playersRigidBody2D.AddForce((Vector2.left + Vector2.up) * _blowingPower, ForceMode2D.Impulse);
        }
    }


    //<============= �R���[�`�� =============>//
    /// <summary> �m�b�N�o�b�N���s�p�R���[�`���B : �m�b�N�o�b�N�����ǂ�����\���ϐ�����莞�� true �ɂ���B </summary>
    IEnumerator KnockBackMode()
    {
        _isKnockBackNow = true;
        yield return new WaitForSeconds(_knockBackModeTime);
        _isKnockBackNow = false;
    }


    //<============= ���z�֐� =============>//
    /// <summary> Enemy�ړ��p�֐� : �I�[�o�[���C�h�� </summary>
    protected virtual void Move() { }
}
