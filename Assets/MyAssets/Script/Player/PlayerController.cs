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
    Rigidbody2D rigidbody2D;
    Jump_Script jump_script;
    ChangePlayerState change_player_state;

    [SerializeField] GameObject burrett_prefab;
    float burrett_Cool_Time;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        jump_script = GetComponent<Jump_Script>();
        change_player_state = GetComponent<ChangePlayerState>();
        burrett_Cool_Time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (change_player_state.isMove)//�s���\�ł���Ύ��s����
        {
            //���ړ��̓��͂��󂯕t����
            float h = Input.GetAxisRaw("Horizontal");//������
            float v = Input.GetAxisRaw("Vertical");//�c����


            //���ړ�
            if (v == 0)//�c�̓��͂����鎞�͉��ړ��ł��Ȃ�
            {
                if (h != 0)
                {
                    if (Input.GetButton("Dash"))
                    {
                        rigidbody2D.AddForce(Vector2.right * h * move_speed_x * Time.deltaTime * dash_speed, ForceMode2D.Force);
                    }
                    else
                    {
                        //rigidbody2D.AddForce�ł̈ړ�
                        rigidbody2D.AddForce(Vector2.right * h * move_speed_x * Time.deltaTime, ForceMode2D.Force);

                        //velocity�ňړ������
                        //rigidbody2D.velocity = Vector2.right * h * move_speed_x * dash_speed;


                    }
                }
            }

            //�W�����v
            jump_script.Jump(jump_power);

            //�V���b�g
            if (v == 0)//�c�̓��͂����鎞�͑łĂȂ�
            {
                //�V���b�g
                if (burrett_Cool_Time < 0f)
                {
                    if (Input.GetButton("Fire1"))
                    {
                        Instantiate(burrett_prefab, Vector3.zero, Quaternion.identity);
                        burrett_Cool_Time = 0.15f;
                    }
                }
                else
                {
                    if (burrett_Cool_Time >= 0f)
                    {
                        burrett_Cool_Time -= Time.deltaTime;
                    }
                }
            }
        }
    }
}
    