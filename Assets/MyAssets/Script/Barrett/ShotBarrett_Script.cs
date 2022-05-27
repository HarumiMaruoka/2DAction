using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBarrett_Script : MonoBehaviour
{
    ChangePlayerState change_player_state;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody2D;

    bool isRigth;
    bool isLeft;

    float destroy_Time;
    bool is_deth = false;

    float isDash;

    [SerializeField, Tooltip("Gizmo�\��")] bool _isGizmo;


    [SerializeField] LayerMask layerMask_of_Burrett;
    [SerializeField] LayerMask layerMask_Hit_Enemy;
    [SerializeField] LayerMask layerMask_Hit_Ground;

    [SerializeField] float moveSpeed;

    EnemyBase enemy;

    float dethTimer = 0;

    enum Contact_partner
    {
        NON,ENEMY,BLOCK,ERROR,
    }

    // Start is called before the first frame update
    void Start()
    {
        isDash = 1f;
        destroy_Time = 0f;
        //SpriteRenderer���擾����
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //�v���C���[�̌������擾����
        change_player_state = GameObject.Find("ChibiRobo").GetComponent<ChangePlayerState>();
        isRigth = change_player_state.isRigth;
        isLeft = change_player_state.isLeft;

        int direction = 1;//���ˈʒu�����p
        if (isLeft)//�K�v�ł���΍������ɂ���
        {
            spriteRenderer.flipX = true;
            direction = -1;
        }

        if (Input.GetButton("Dash"))
        {
            isDash *= 1.5f;
        }

        //���ˈʒu��ݒ肷��
        transform.position = GameObject.Find("ChibiRobo").transform.position + (Vector3.down * 0.25f) + (Vector3.right * direction * 0.8f);//�����ʒu�͏e���ӂ�
    }

    // Update is called once per frame
    void Update()
    {
        //�����Ă�������ɐi�ݑ�����
        if (isRigth)
        {
            rigidbody2D.velocity = Vector2.right * moveSpeed * isDash;
        }
        else if (isLeft)
        {
            rigidbody2D.velocity = Vector2.left * moveSpeed * isDash;
        }

        //�����Ŕj��
        //if(Vector3.Distance(transform.position, player.transform.position) > 8)
        //{
        //    Destroy(this.gameObject);
        //}



        //���ԂŔj��
        if (destroy_Time > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            destroy_Time += Time.deltaTime;
        }

        //�G�ƐڐG�����Ƃ��͏����x�点�āA�e������������
        if (is_deth)
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
            collision.gameObject.GetComponent<EnemyBase>().HitBurrett(1);
            is_deth = true;
        }
        else if (collision.gameObject.tag == "Ground")
        {
            //Ground�ƐڐG�������A�e�͏�������
            Destroy(this.gameObject);
        }

    }


}
