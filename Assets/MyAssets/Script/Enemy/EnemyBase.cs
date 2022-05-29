using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //エネミー共通の基本情報
    [SerializeField] protected int _hit_Point;//体力
    [SerializeField] protected int _offensive_Power;//攻撃力
    [SerializeField] protected Vector2 _player_knock_back_Power;//プレイヤーに対するノックバック力

    //向いている方向
    protected bool _isRight;
    protected bool _isLeft;

    //プレイヤーのコンポーネント
    GameObject player;
    protected Transform _playerPos;//player's position
    protected PlayerBasicInformation _player_basic_information;//プレイヤーのライフを減らす用
    protected Rigidbody2D _playersRigidBody2D;

    //自身のコンポーネント
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rigidBody2d;

    //色変更用
    bool isColorChange = false;
    float _color_change_time = 0;

    //ノックバック関連
    public bool _isKnockBackNow;//ノックバック中かどうか
    [SerializeField] public float _tank;//吹っ飛ばされにくさ
    float _knockBackModeTime = 0f;//ノックバック時間を表す変数

    //全エネミーで共通のEnemyの初期化関数。継承先のStart関数で呼び出す。
    protected void Enemy_Initialize()
    {
        //プレイヤーの情報を取得
        player = GameObject.Find("ChibiRobo");
        _player_basic_information = player.GetComponent<PlayerBasicInformation>();
        _playerPos = player.GetComponent<Transform>();
        _playersRigidBody2D = player.GetComponent<Rigidbody2D>();

        //自身のコンポーネントを取得
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody2d = GetComponent<Rigidbody2D>();
    }

    //全エネミーで共通のEnemyのUpdate関数。継承先のUpdate関数で呼び出す
    protected void NeedEnemyElement()
    {
        //体力がなくなった時の処理
        if (_hit_Point <= 0)
        {
            //体力がなくなったら消滅する
            Destroy(this.gameObject);
        }
        //色を変える処理
        if (!Mathf.Approximately(_color_change_time, 0f))
        {

            if (isColorChange)
            {
                _spriteRenderer.color = Color.red;
                isColorChange = false;
            }
            else if (_color_change_time < 0)
            {
                _spriteRenderer.color = new Color(255, 255, 255, 255);
            }
            else if (_color_change_time > 0)
            {
                _color_change_time -= Time.deltaTime;
            }

        }
        //プレイヤーがいる方向を取得する
        if (transform.position.x < _playerPos.transform.position.x)
        {
            _isRight = true;
        }
        else
        {
            _isRight = false;
        }

    }


    //プレイヤーからの攻撃時に、呼び出すので public で宣言する。
    public void HitPlayerAttadk(int damage)//ノックバックしない場合
    {
        _hit_Point -= damage;
        isColorChange = true;
        _color_change_time = 0.1f;
    }
    public void HitPlayerAttadk(int damage, float knockBackTimer)//ノックバックする場合
    {
        _hit_Point -= damage;
        isColorChange = true;
        _color_change_time = 0.1f;

        //ノックバックする。プレイヤーのノックバック力(時間)-エネミーの耐久力(時間)分、Moveを停止する。
        _knockBackModeTime = (knockBackTimer - _tank) > 0f ? (knockBackTimer - _tank) : 0f;
        StartCoroutine(KnockBackMode());
    }

    //プレイヤーと敵が接触した時に呼ばれる。プレイヤーの体力を減らして、ノックバックさせる。
    public void HitPlayer()
    {
        //プレイヤーのHitPointを減らす
        _player_basic_information._playerHitPoint -= _offensive_Power;
        //プレイヤーをノックバックする
        if (_isRight)
        {
            _playersRigidBody2D.AddForce(Vector2.right * _player_knock_back_Power, ForceMode2D.Impulse);
        }
        else
        {
            _playersRigidBody2D.AddForce(Vector2.left * _player_knock_back_Power, ForceMode2D.Impulse);
        }
    }

    //エネミー移動関数(オーバーライド可能)
    protected virtual void Move()
    {

    }

    //ノックバック用のコード。
    IEnumerator KnockBackMode()
    {
        _isKnockBackNow = true;
        yield return new WaitForSeconds(_knockBackModeTime);
        _isKnockBackNow = false;
    }
}
