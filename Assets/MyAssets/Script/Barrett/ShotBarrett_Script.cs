using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBarrett_Script : MonoBehaviour
{
    ChangePlayerState _changePlayerState;
    SpriteRenderer _spriteRenderer;
    Rigidbody2D _rigidBody2D;

    bool _isRigth;
    bool _isLeft;

    float _destroyTime;
    bool _isDeth = false;

    float isDash;

    [SerializeField, Tooltip("Gizmo�\��")] bool _isGizmo;


    [SerializeField] LayerMask layerMask_of_Burrett;
    [SerializeField] LayerMask layerMask_Hit_Enemy;
    [SerializeField] LayerMask layerMask_Hit_Ground;

    [SerializeField] float _moveSpeed;
    [SerializeField] float _dashSpeed;
    [SerializeField] int _barrettPower;

    EnemyBase _enemy;

    float dethTimer = 0;

    enum Contact_partner
    {
        NON,ENEMY,BLOCK,ERROR,
    }

    // Start is called before the first frame update
    void Start()
    {
        isDash = 1f;
        _destroyTime = 0f;
        //SpriteRenderer���擾����
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody2D = GetComponent<Rigidbody2D>();

        //�v���C���[�̌������擾����
        _changePlayerState = GameObject.Find("ChibiRobo").GetComponent<ChangePlayerState>();
        _isRigth = _changePlayerState.isRigth;
        _isLeft = _changePlayerState.isLeft;

        int direction = 1;//���ˈʒu�����p
        if (_isLeft)//�K�v�ł���΍������ɂ���
        {
            _spriteRenderer.flipX = true;
            direction = -1;
        }

        if (Input.GetButton("Dash"))
        {
            isDash *= _dashSpeed;
        }

        //���ˈʒu��ݒ肷��
        transform.position = GameObject.Find("ChibiRobo").transform.position + (Vector3.down * 0.25f) + (Vector3.right * direction * 0.8f);//�����ʒu�͏e���ӂ�
    }

    // Update is called once per frame
    void Update()
    {
        //�����Ă�������ɐi�ݑ�����
        if (_isRigth)
        {
            _rigidBody2D.velocity = Vector2.right * _moveSpeed * isDash;
        }
        else if (_isLeft)
        {
            _rigidBody2D.velocity = Vector2.left * _moveSpeed * isDash;
        }

        //�����Ŕj��
        //if(Vector3.Distance(transform.position, player.transform.position) > 8)
        //{
        //    Destroy(this.gameObject);
        //}



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
            dethTimer += Time.deltaTime;
        }
        if (dethTimer > 0.04f)
        {
            Destroy(this.gameObject);
        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            //�����ɓG�ƐڐG�����Ƃ��̏���������
            collision.gameObject.GetComponent<EnemyBase>().HitPlayerAttadk(_barrettPower,Vector2.zero);
            _isDeth = true;
        }
        else if (collision.gameObject.tag == "Ground")
        {
            //Ground�ƐڐG�������A�e�͏�������
            Destroy(this.gameObject);
        }
    }
}
