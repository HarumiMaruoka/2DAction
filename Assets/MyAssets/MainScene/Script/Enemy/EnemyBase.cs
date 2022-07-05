using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //�G�l�~�[���ʂ̊�{���
    [SerializeField] protected int _hitPoint;//�̗�
    [SerializeField] protected int _offensive_Power;//�U����
    [SerializeField] protected Vector2 _playerKnockBackPower;//�v���C���[�ɑ΂���m�b�N�o�b�N��

    //�����Ă�������A�m�b�N�o�b�N���Ɏg��
    protected bool _isRight;

    //�v���C���[�̃R���|�[�l���g
    GameObject _player;
    protected Transform _playerPos;//player's position
    protected PlayerBasicInformation _playerBasicInformation;//�v���C���[�̃��C�t�����炷�p
    protected Rigidbody2D _playersRigidBody2D;
    PlayerMoveManager _playerMoveManager;


    //���g�̃R���|�[�l���g
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rigidBody2d;

    //�F�ύX�p
    bool _isColorChange = false;
    float _colorChangeTimeValue = 0;
    float _colorChangeTime = 0.1f;

    //�m�b�N�o�b�N�֘A
    public bool _isKnockBackNow;//�m�b�N�o�b�N�����ǂ���
    [SerializeField] public float _tank;//������΂���ɂ���
    float _knockBackModeTime = 0f;//�m�b�N�o�b�N���Ԃ�\���ϐ�

    //�S�G�l�~�[�ŋ��ʂ�Enemy�̏������֐��B�p�����Start�֐��ŌĂяo���B
    protected void EnemyInitialize()
    {
        //�e�ϐ��̏�����
        //�v���C���[�̏����擾
        _player = GameObject.Find("ChibiRobo");
        _playerBasicInformation = _player.GetComponent<PlayerBasicInformation>();
        _playerPos = _player.GetComponent<Transform>();
        _playersRigidBody2D = _player.GetComponent<Rigidbody2D>();
        _playerMoveManager = _player.GetComponent<PlayerMoveManager>();

        //���g�̃R���|�[�l���g���擾
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody2d = GetComponent<Rigidbody2D>();
    }

    //�S�G�l�~�[�ŋ��ʂ�Enemy��Update�֐��B�p�����Update�֐��ŌĂяo��
    protected void NeedEnemyElement()
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

    //�v���C���[����̍U�����ɁA�Ăяo���̂� public �Ő錾����B
    public void HitPlayerAttadk(int damage)//�m�b�N�o�b�N���Ȃ��ꍇ
    {
        //���g�̗̑͂����炵�A0.1�b�����F��Ԃɕς���B
        _hitPoint -= damage;
        _isColorChange = true;
        _colorChangeTimeValue = _colorChangeTime;
    }
    public void HitPlayerAttadk(int damage, float knockBackTimer)//�m�b�N�o�b�N����ꍇ
    {
        //���g�̗̑͂����炵�A0.1�b�����F��Ԃɕς���B
        _hitPoint -= damage;
        _isColorChange = true;
        _colorChangeTimeValue = _colorChangeTime;

        //�m�b�N�o�b�N����B�v���C���[�̃m�b�N�o�b�N��(����)-�G�l�~�[�̑ϋv��(����)���AMove���~����B
        _knockBackModeTime = (knockBackTimer - _tank) > 0f ? (knockBackTimer - _tank) : 0f;
        StartCoroutine(KnockBackMode());
    }

    //�v���C���[�ƓG���ڐG�������ɌĂ΂��B�v���C���[�̗̑͂����炵�āA�m�b�N�o�b�N������B
    public void HitPlayer()
    {
        //�v���C���[��HitPoint�����炷
        PlayerManager.Instance.PlayerHealthPoint -= _offensive_Power;
        _playersRigidBody2D.velocity = Vector2.zero;
        //�v���C���[���m�b�N�o�b�N����
        if (_isRight)
        {
            _playersRigidBody2D.AddForce((Vector2.right + Vector2.up) * _playerKnockBackPower, ForceMode2D.Impulse);
        }
        else
        {
            _playersRigidBody2D.AddForce((Vector2.left + Vector2.up) * _playerKnockBackPower, ForceMode2D.Impulse);
        }
    }

    //�G�l�~�[�ړ��֐�(�I�[�o�[���C�h�\)
    protected virtual void Move()
    {

    }

    //�m�b�N�o�b�N�p�̃R�[�h�B
    IEnumerator KnockBackMode()
    {
        _isKnockBackNow = true;
        yield return new WaitForSeconds(_knockBackModeTime);
        _isKnockBackNow = false;
    }
}
