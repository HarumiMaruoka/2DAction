using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //エネミー共通の基本情報
    [SerializeField] protected int _hitPoint;//体力
    [SerializeField] protected int _offensive_Power;//攻撃力
    [SerializeField] protected Vector2 _playerKnockBackPower;//プレイヤーに対するノックバック力

    //向いている方向、ノックバック時に使う
    protected bool _isRight;

    //プレイヤーのコンポーネント
    GameObject _player;
    protected Transform _playerPos;//player's position
    protected PlayerBasicInformation _playerBasicInformation;//プレイヤーのライフを減らす用
    protected Rigidbody2D _playersRigidBody2D;
    PlayerMoveManager _playerMoveManager;


    //自身のコンポーネント
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rigidBody2d;

    //色変更用
    bool _isColorChange = false;
    float _colorChangeTimeValue = 0;
    float _colorChangeTime = 0.1f;

    //ノックバック関連
    public bool _isKnockBackNow;//ノックバック中かどうか
    [SerializeField] public float _tank;//吹っ飛ばされにくさ
    float _knockBackModeTime = 0f;//ノックバック時間を表す変数

    //全エネミーで共通のEnemyの初期化関数。継承先のStart関数で呼び出す。
    protected void EnemyInitialize()
    {
        //各変数の初期化
        //プレイヤーの情報を取得
        _player = GameObject.Find("ChibiRobo");
        _playerBasicInformation = _player.GetComponent<PlayerBasicInformation>();
        _playerPos = _player.GetComponent<Transform>();
        _playersRigidBody2D = _player.GetComponent<Rigidbody2D>();
        _playerMoveManager = _player.GetComponent<PlayerMoveManager>();

        //自身のコンポーネントを取得
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody2d = GetComponent<Rigidbody2D>();
    }

    //全エネミーで共通のEnemyのUpdate関数。継承先のUpdate関数で呼び出す
    protected void NeedEnemyElement()
    {
        //体力がなくなった時の処理
        if (_hitPoint <= 0)
        {
            //体力がなくなったら消滅する
            Destroy(this.gameObject);
        }

        //色を変える必要があれば変える
        if (_isColorChange)
        {
            _spriteRenderer.color = Color.red;
            _isColorChange = false;
        }
        //色を元に戻す
        else if (_colorChangeTimeValue < 0)
        {
            _spriteRenderer.color = new Color(255, 255, 255, 255);
        }
        //クールタイム解消
        else if (_colorChangeTimeValue > 0)
        {
            _colorChangeTimeValue -= Time.deltaTime;
        }
    }

    //プレイヤーからの攻撃時に、呼び出すので public で宣言する。
    public void HitPlayerAttadk(int damage)//ノックバックしない場合
    {
        //自身の体力を減らし、0.1秒だけ色を赤に変える。
        _hitPoint -= damage;
        _isColorChange = true;
        _colorChangeTimeValue = _colorChangeTime;
    }
    public void HitPlayerAttadk(int damage, float knockBackTimer)//ノックバックする場合
    {
        //自身の体力を減らし、0.1秒だけ色を赤に変える。
        _hitPoint -= damage;
        _isColorChange = true;
        _colorChangeTimeValue = _colorChangeTime;

        //ノックバックする。プレイヤーのノックバック力(時間)-エネミーの耐久力(時間)分、Moveを停止する。
        _knockBackModeTime = (knockBackTimer - _tank) > 0f ? (knockBackTimer - _tank) : 0f;
        StartCoroutine(KnockBackMode());
    }

    //プレイヤーと敵が接触した時に呼ばれる。プレイヤーの体力を減らして、ノックバックさせる。
    public void HitPlayer()
    {
        //プレイヤーのHitPointを減らす
        PlayerManager.Instance.PlayerHealthPoint -= _offensive_Power;
        _playersRigidBody2D.velocity = Vector2.zero;
        //プレイヤーをノックバックする
        if (_isRight)
        {
            _playersRigidBody2D.AddForce((Vector2.right + Vector2.up) * _playerKnockBackPower, ForceMode2D.Impulse);
        }
        else
        {
            _playersRigidBody2D.AddForce((Vector2.left + Vector2.up) * _playerKnockBackPower, ForceMode2D.Impulse);
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
