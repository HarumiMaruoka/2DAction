using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotBarrett_Script : MonoBehaviour
{
    GameObject player;
    ChangePlayerState change_player_state;
    SpriteRenderer spriteRenderer;
    Rigidbody2D rigidbody2D;

    bool isRigth;
    bool isLeft;

    float destroy_Time;
    bool is_deth = false;

    float isDash;

    Vector2 over_lap_pos;
    public float over_lap_radias;

    [SerializeField, Tooltip("Gizmo•\¦")] bool _isGizmo;


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
        //SpriteRenderer‚ğæ“¾‚·‚é
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //ƒvƒŒƒCƒ„[‚ÌˆÊ’u‚ğİ’è‚·‚é
        player = GameObject.Find("ChibiRobo");
        transform.position = player.transform.position + (Vector3.down * 0.25f);//‰ŠúˆÊ’u‚ÍeŒû•Ó‚è
        over_lap_pos = transform.position;//over lap position ‚Ì‰ŠúˆÊ’u‚ğİ’è

        //ƒvƒŒƒCƒ„[‚ÌŒü‚«‚ğæ“¾‚·‚é
        change_player_state = player.GetComponent<ChangePlayerState>();
        isRigth = change_player_state.isRigth;
        isLeft = change_player_state.isLeft;
        if (isLeft)//•K—v‚Å‚ ‚ê‚Î¶Œü‚«‚É‚·‚é
        {
            spriteRenderer.flipX = true;
        }

        if (Input.GetButton("Dash"))
        {
            isDash *= 1.5f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Œü‚¢‚Ä‚¢‚é•ûŒü‚Éi‚İ‘±‚¯‚é
        if (isRigth)
        {
            rigidbody2D.velocity = Vector2.right * moveSpeed * isDash;
        }
        else if (isLeft)
        {
            rigidbody2D.velocity = Vector2.left * moveSpeed * isDash;
        }
        //over lap position ‚ğXV
        over_lap_pos = transform.position;

        //‹——£‚Å”j‰ó
        //if(Vector3.Distance(transform.position, player.transform.position) > 8)
        //{
        //    Destroy(this.gameObject);
        //}



        //ŠÔ‚Å”j‰ó
        if (destroy_Time > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            destroy_Time += Time.deltaTime;
        }

        //“G‚ÆÚG‚µ‚½‚Æ‚«‚Í­‚µ’x‚ç‚¹‚ÄA’e‚ğÁ¸‚³‚¹‚é
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
            //‚±‚±‚É“G‚ÆÚG‚µ‚½‚Æ‚«‚Ìˆ—‚ğ‘‚­
            collision.gameObject.GetComponent<EnemyBase>().HitBurrett(1);
            is_deth = true;
        }
        else if (collision.gameObject.tag == "Ground")
        {
            //Ground‚ÆÚG‚µ‚½A’e‚ÍÁ¸‚·‚é
            Destroy(this.gameObject);
        }

    }
}
