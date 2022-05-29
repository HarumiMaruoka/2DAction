using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//�v���C���[�̈ړ��Ɋւ���N���X
public class PlayerController : MonoBehaviour
{
    public float move_speed_x;
    float previous_move_speed;
    const float dash_speed = 1.6f;
    public float jump_power;
    Rigidbody2D _rigidbody2D;
    Jump_Script jump_script;
    ChangePlayerState change_player_state;

    [SerializeField] GameObject _burrettPrefab;
    [SerializeField] GameObject _slashPrefabOne;
    //[SerializeField] GameObject _slashPrefabTow;
    float _burrettCoolTime;
    float _slashCoolTime;

    [SerializeField] float _hoverPower;


    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        jump_script = GetComponent<Jump_Script>();
        change_player_state = GetComponent<ChangePlayerState>();
        _burrettCoolTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (change_player_state._isMove)//�s���\�ł���Ύ��s����
        {
            //���͂��擾����
            float h = Input.GetAxisRaw("Horizontal");//������
            float v = Input.GetAxisRaw("Vertical");//�c����

            //���ړ�
            if (v == 0)//�c�̓��͂����鎞�͉��ړ��ł��Ȃ�
            {
                if (h != 0)
                {
                    if (Input.GetButton("Dash"))
                    {
                        _rigidbody2D.AddForce(Vector2.right * h * move_speed_x * Time.deltaTime * dash_speed, ForceMode2D.Force);
                    }
                    else
                    {
                        //rigidbody2D.AddForce�ł̈ړ�
                        _rigidbody2D.AddForce(Vector2.right * h * move_speed_x * Time.deltaTime, ForceMode2D.Force);

                        //velocity�ňړ������
                        //rigidbody2D.velocity = Vector2.right * h * move_speed_x * dash_speed;


                    }
                }
            }

            //�W�����v
            jump_script.Jump(jump_power);

            //�z�o�[
            if (change_player_state._isHover)
            {
                _rigidbody2D.velocity = Vector2.up * _hoverPower;
            }

            //�V���b�g
            if (v == 0 && !change_player_state._isHover)//�c�̓��͂����鎞�͑łĂȂ��A�z�o�[�����łĂȂ�
            {
                //�V���b�g
                if (_burrettCoolTime <= 0f)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        Instantiate(_burrettPrefab, Vector3.zero, Quaternion.identity);
                        _burrettCoolTime = 0.15f;
                    }
                }
                else if (_burrettCoolTime >= 0f)
                {
                        _burrettCoolTime -= Time.deltaTime;
                }
            }

            //�X���b�V��
            if (v == 0 && !change_player_state._isHover)//�c�̓��͂����鎞�͑łĂȂ��A�z�o�[�����łĂȂ�
            {
                if (_slashCoolTime <= 0f)
                {
                    if (Input.GetButtonDown("Fire2"))
                    {
                        Instantiate(_slashPrefabOne, Vector3.zero, Quaternion.identity);
                        _slashCoolTime = 0.25f;
                    }
                }
                else
                {
                    if (_slashCoolTime >= 0f)
                    {
                        _slashCoolTime -= Time.deltaTime;
                    }
                }
            }
        }
    }
}
