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

    [SerializeField, Tooltip("Gizmo表示")] bool _isGizmo;


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
        //SpriteRendererを取得する
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody2D = GetComponent<Rigidbody2D>();

        //プレイヤーの向きを取得する
        _changePlayerState = GameObject.Find("ChibiRobo").GetComponent<ChangePlayerState>();
        _isRigth = _changePlayerState.isRigth;
        _isLeft = _changePlayerState.isLeft;

        int direction = 1;//発射位置調整用
        if (_isLeft)//必要であれば左向きにする
        {
            _spriteRenderer.flipX = true;
            direction = -1;
        }

        if (Input.GetButton("Dash"))
        {
            isDash *= _dashSpeed;
        }

        //発射位置を設定する
        transform.position = GameObject.Find("ChibiRobo").transform.position + (Vector3.down * 0.25f) + (Vector3.right * direction * 0.8f);//初期位置は銃口辺り
    }

    // Update is called once per frame
    void Update()
    {
        //向いている方向に進み続ける
        if (_isRigth)
        {
            _rigidBody2D.velocity = Vector2.right * _moveSpeed * isDash;
        }
        else if (_isLeft)
        {
            _rigidBody2D.velocity = Vector2.left * _moveSpeed * isDash;
        }

        //距離で破壊
        //if(Vector3.Distance(transform.position, player.transform.position) > 8)
        //{
        //    Destroy(this.gameObject);
        //}



        //時間で破壊
        if (_destroyTime > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _destroyTime += Time.deltaTime;
        }

        //敵と接触したときは少し遅らせて、弾を消失させる
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
            //ここに敵と接触したときの処理を書く
            collision.gameObject.GetComponent<EnemyBase>().HitPlayerAttadk(_barrettPower);
            _isDeth = true;
        }
        else if (collision.gameObject.tag == "Ground")
        {
            //Groundと接触した時、弾は消失する
            Destroy(this.gameObject);
        }
    }
}
