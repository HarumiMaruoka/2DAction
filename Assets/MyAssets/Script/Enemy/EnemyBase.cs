using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //�G�l�~�[���ʂ̊�{���
    [SerializeField] protected int _hit_Point;//�̗�
    [SerializeField] protected int _offensive_Power;//�U����
    [SerializeField] protected Vector2 _player_knock_back_Power;//�m�b�N�o�b�N��

    //�����Ă������
    protected bool _isRight;
    protected bool _isLeft;

    //�v���C���[�̃R���|�[�l���g
    GameObject player;
    protected Transform _playerPos;//player's position
    protected PlayerBasicInformation _player_basic_information;//�v���C���[�̃��C�t�����炷�p
    protected Rigidbody2D _playersRigidBody2D;
    //���g�̃R���|�[�l���g
    protected SpriteRenderer sprite_renderer;
    protected Rigidbody2D rigidbody2d;

    //�F�ύX�p
    bool isColorChange = false;
    float _color_change_time = 0;


    //�S�G�l�~�[�ŋ��ʂ�Enemy�̏������֐��B�p�����Start�֐��ŌĂяo���B
    protected void Enemy_Initialize()
    {
        //�v���C���[�̏����擾
        player = GameObject.Find("ChibiRobo");
        _player_basic_information = player.GetComponent<PlayerBasicInformation>();
        _playerPos = player.GetComponent<Transform>();
        _playersRigidBody2D = player.GetComponent<Rigidbody2D>();

        //���g�̃R���|�[�l���g���擾
        sprite_renderer = GetComponent<SpriteRenderer>();
        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    //�S�G�l�~�[�ŋ��ʂ�Enemy��Update�֐��B�p�����Update�֐��ŌĂяo��
    protected void NeedEnemyElement()
    {
        //�̗͂��Ȃ��Ȃ������̏���
        if (_hit_Point <= 0)
        {
            //�̗͂��Ȃ��Ȃ�������ł���
            Destroy(this.gameObject);
        }
        //�F��ς��鏈��
        if (!Mathf.Approximately(_color_change_time, 0f))
        {

            if (isColorChange)
            {
                sprite_renderer.color = Color.red;
                isColorChange = false;
            }
            else if (_color_change_time < 0)
            {
                sprite_renderer.color = new Color(255, 255, 255, 255);
            }
            else if (_color_change_time > 0)
            {
                _color_change_time -= Time.deltaTime;
            }

        }
        //�v���C���[������������擾����
        if (transform.position.x < _playerPos.transform.position.x)
        {
            _isRight = true;
        }
        else
        {
            _isRight = false;
        }

    }

    //Burrett����Ăяo���̂� public �Ő錾����
    public void HitPlayerAttadk(int damage)
    {
        _hit_Point -= damage;
        isColorChange = true;
        _color_change_time = 0.1f;
    }

    //�v���C���[�ƓG���ڐG�������ɌĂ΂��
    public void HitPlayer()
    {
        //�v���C���[��HitPoint�����炷
        _player_basic_information._playerHitPoint -= _offensive_Power;
        //�v���C���[���m�b�N�o�b�N����
        if (_isRight) {
            _playersRigidBody2D.AddForce(Vector2.right * _player_knock_back_Power, ForceMode2D.Impulse);
        }
        else
        {
            _playersRigidBody2D.AddForce(Vector2.left * _player_knock_back_Power, ForceMode2D.Impulse);
        }
    }

    //�G�l�~�[�ړ��֐�(�I�[�o�[���C�h�\)
    protected virtual void Move()
    {

    }

}
