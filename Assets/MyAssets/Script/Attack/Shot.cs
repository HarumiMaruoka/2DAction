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
    float _destroyTime;//�e���j�󂳂��܂ł̎��ԁB
    float _dethTimer;//�G�ɓ���������A�^�C�}�[�X�^�[�g�B
    bool _isDeth = false;//�v���C���[���_�b�V�����Ă��邩�ǂ�����\���B
    float _dashMode;//�v���C���[�������Ă���Ƃ��́A1������B

    //�v���C���[�̃R���|�[�l���g
    SpriteRenderer _playersSpriteRendere;

    //���g�̃R���|�[�l���g
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigidBody2D;


    enum Contact_partner
    {
        NON, ENEMY, BLOCK, ERROR,
    }

    // Start is called before the first frame update
    void Start()
    {
        //�e�ϐ��̏�����
        _dashMode = 1f;
        _destroyTime = 0f;
        _dethTimer = 0f;

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
        //���ԂŔj��
        if (_destroyTime > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _destroyTime += Time.deltaTime;
        }

        //�G�ƐڐG�����Ƃ��͏����x�点�āA�e������������
        if (_isDeth)
        {
            _dethTimer += Time.deltaTime;
        }
        if (_dethTimer > 0.04f)
        {
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
        else if (collision.gameObject.tag == "Ground")//Ground�ƐڐG�������A�e�͏�������
        {
            Destroy(this.gameObject);
        }
    }
}
