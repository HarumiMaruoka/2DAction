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

    [SerializeField, Tooltip("Gizmo表示")] bool _isGizmo;


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
        //SpriteRendererを取得する
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();

        //プレイヤーの位置を設定する
        player = GameObject.Find("ChibiRobo");
        transform.position = player.transform.position + (Vector3.down * 0.25f);//初期位置は銃口辺り
        over_lap_pos = transform.position;//over lap position の初期位置を設定

        //プレイヤーの向きを取得する
        change_player_state = player.GetComponent<ChangePlayerState>();
        isRigth = change_player_state.isRigth;
        isLeft = change_player_state.isLeft;
        if (isLeft)//必要であれば左向きにする
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
        //向いている方向に進み続ける
        if (isRigth)
        {
            rigidbody2D.velocity = Vector2.right * moveSpeed * isDash;
        }
        else if (isLeft)
        {
            rigidbody2D.velocity = Vector2.left * moveSpeed * isDash;
        }
        //over lap position を更新
        over_lap_pos = transform.position;

        //距離で破壊
        //if(Vector3.Distance(transform.position, player.transform.position) > 8)
        //{
        //    Destroy(this.gameObject);
        //}



        //時間で破壊
        if (destroy_Time > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            destroy_Time += Time.deltaTime;
        }

        //敵と接触したときは少し遅らせて、弾を消失させる
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
            //ここに敵と接触したときの処理を書く
            collision.gameObject.GetComponent<EnemyBase>().HitBurrett(1);
            is_deth = true;
        }
        else if (collision.gameObject.tag == "Ground")
        {
            //Groundと接触した時、弾は消失する
            Destroy(this.gameObject);
        }

    }
}
