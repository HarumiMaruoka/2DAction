using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shot : MonoBehaviour
{
    //�e�p�����[�^
    [SerializeField] float _moveSpeed;//�e�̈ړ��X�s�[�h
    [SerializeField] float _dashSpeed;//�_�b�V�����̈ړ����x
    [SerializeField] int _barrettPower;//�e�̍U����

    bool _isRigth;//�v���C���[�������Ă������������
    float _dethTimer;//�G�ɓ���������A�^�C�}�[�X�^�[�g�B
    float _dashMode;//�v���C���[�������Ă���Ƃ��́A1������B

    //�v���C���[�̃R���|�[�l���g
    SpriteRenderer _playersSpriteRendere;

    //���g�̃R���|�[�l���g
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigidBody2D;

    //�j�󎞂̃G�t�F�N�g
    [SerializeField] GameObject _destroyingEffectPrefab;

    //�j��Ǘ��p
    bool _isDeth;

    bool _isDethMode;
    float _dethTimer2;


    // Start is called before the first frame update
    void Start()
    {
        //�e�ϐ��̏�����
        _dashMode = 1f;
        _dethTimer = 0f;
        _dethTimer2 = 1.5f;

        //SpriteRenderer���擾����
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody2D = GetComponent<Rigidbody2D>();

        //�v���C���[�̌������擾����
        _playersSpriteRendere = GameObject.Find("ChibiRobo").GetComponent<SpriteRenderer>();
        _isRigth = !_playersSpriteRendere.flipX;

        int direction = 1;//���ˈʒu�����p
        if (!_isRigth)//�K�v�ł���΍������ɂ���
        {
            _spriteRenderer.flipX = true;
            direction = -1;
        }

        if (Input.GetButton("Dash"))
        {
            _dashMode *= _dashSpeed;
        }

        //���ˈʒu��ݒ肷��
        transform.position = GameObject.Find("ChibiRobo").transform.position + (Vector3.down * 0.25f) + (Vector3.right * direction * 0.8f);//�����ʒu�͏e���ӂ�

        //�����Ă�������ɐi�ݑ�����
        if (_isRigth)
        {
            _rigidBody2D.velocity = Vector2.right * _moveSpeed * _dashMode;
        }
        else
        {
            _rigidBody2D.velocity = Vector2.left * _moveSpeed * _dashMode;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Destroy(this.gameObject,1.5f);

        //�G�ƐڐG�����Ƃ��͏����x�点�āA�e������������
        if (_isDeth)
        {
            _dethTimer += Time.deltaTime;
        }
        if (_dethTimer > 0.03f)
        {
            Instantiate(_destroyingEffectPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyBase enemy))//�G�ɐڐG�����Ƃ��̏���
        {
            enemy.HitPlayerAttadk(_barrettPower);
            _isDeth = true;
        }
        if (collision.TryGetComponent(out BossBase boss))
        {
            boss.HitPlayerAttack(_barrettPower);
            _isDeth = true;
        }
        else if (collision.gameObject.tag == "Ground")//Ground�ƐڐG�������A�e�͏�������
        {
            Instantiate(_destroyingEffectPrefab, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }

    IEnumerator DethTimer()
    {
        _isDethMode = true;
        yield return new WaitForSeconds(_dethTimer2);
        _isDethMode = false;
    }
}
