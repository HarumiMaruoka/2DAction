using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    //<============= メンバー変数 =============>//
    //エネミー共通のステータス
    /// <summary> ヒットポイント </summary>
    [SerializeField] protected int _hitPoint;
    /// <summary> 攻撃力 </summary>
    [SerializeField] protected int _offensive_Power;
    /// <summary> 吹っ飛ばし力 </summary>
    [SerializeField] protected Vector2 _blowingPower;
    /// <summary> 重さ : 吹っ飛ばされにくさ </summary>
    [SerializeField] public float _weight;

    //このエネミーが向いている方向
    protected bool _isRight;

    //プレイヤーのコンポーネント
    protected GameObject _player;
    protected Transform _playerPos;
    protected PlayerBasicInformation _playerBasicInformation;
    protected Rigidbody2D _playersRigidBody2D;
    protected PlayerMoveManager _playerMoveManager;

    //自身のコンポーネント
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rigidBody2d;

    //色変更用
    /// <summary> 色を変えるかどうか </summary>
    protected bool _isColorChange = false;
    /// <summary> 色を変更する時間 </summary>
    protected float _colorChangeTime = 0.1f;
    /// <summary> 色を変更する時間 : 現在の残り時間 </summary>
    protected float _colorChangeTimeValue = 0;

    //ノックバック関連
    /// <summary> 現在ノックバック中かどうか </summary>
    public bool _isKnockBackNow;
    /// <summary> ノックバック時間 </summary>
    float _knockBackModeTime = 0f;


    //<============== protectedメンバー関数 ==============>//
    /// <summary> 全エネミーで共通のEnemyの初期化関数。継承先のStart関数で呼び出す。 </summary>
    /// <returns> 成功したら true を返す。 </returns>
    protected bool Initialize_Enemy()
    {
        if (EnemyInitialize_Get_PlayerComponents())
        {
            Debug.LogError($"初期化に失敗しました。 : {gameObject.name}");
            return false;
        }
        if (EnemyInitialize_Get_ThisGameObjectComponents())
        {
            Debug.LogError($"初期化に失敗しました。 : {gameObject.name}");
            return false;
        }

        return true;
    }
    //全エネミーで共通のEnemyのUpdate関数。継承先のUpdate関数で呼び出す
    protected void Update_Enemy()
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


    //<============= private関数 =============>//
    //******************** 初期化関連 ********************//
    /// <summary> プレイヤーのコンポーネントを取得 </summary>
    /// <returns> 成功したら true を返す。 </returns>
    bool EnemyInitialize_Get_PlayerComponents()
    {
        //プレイヤーの情報を取得
        _player = GameObject.Find("ChibiRobo");
        if (_player == null)
        {
            Debug.LogError($"Playerを取得できませんでした。 : {gameObject.name}");
            return false;
        }
        _playerBasicInformation = _player.GetComponent<PlayerBasicInformation>();
        if (_playerBasicInformation == null)
        {
            Debug.LogError($"PlayerのPlayerBasicInformationコンポーネントを取得できませんでした。 : {gameObject.name}");
            return false;
        }
        _playerPos = _player.GetComponent<Transform>();
        if (_playerPos == null)
        {
            Debug.LogError($"PlayerのTransformコンポーネントを取得できませんでした。 : {gameObject.name}");
            return false;
        }
        _playersRigidBody2D = _player.GetComponent<Rigidbody2D>();
        if (_playersRigidBody2D == null)
        {
            Debug.LogError($"Playerの_playersRigidBody2Dコンポーネントを取得できませんでした。 : {gameObject.name}");
            return false;
        }
        _playerMoveManager = _player.GetComponent<PlayerMoveManager>();
        if (_playerMoveManager == null)
        {
            Debug.LogError($"PlayerのPlayerMoveManagerコンポーネントを取得できませんでした。 : {gameObject.name}");
            return false;
        }

        return true;
    }
    /// <summary> このゲームオブジェクトにアタッチされているコンポーネントを取得する。 </summary>
    /// <returns> 成功したら true を返す。 </returns>
    bool EnemyInitialize_Get_ThisGameObjectComponents()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError($"SpriteRendererの取得に失敗しました。 : {gameObject.name}");
            return false;
        }
        _rigidBody2d = GetComponent<Rigidbody2D>();
        if (_rigidBody2d == null)
        {
            Debug.LogError($"Rigidbody2Dの取得に失敗しました。 : {gameObject.name}");
            return false;
        }

        return true;
    }


    //<============= public関数 =============>//
    //******************** 攻撃ヒット関連 ********************//
    /// <summary> プレイヤーからの攻撃処理 : ノックバック無し版 </summary>
    /// <param name="damage"> ダメージ量 </param>
    public void HitPlayerAttadk(int damage)
    {
        //自身の体力を減らし、一定時間 色を被ダメ用の色に変える。
        _hitPoint -= damage;
        _isColorChange = true;
        _colorChangeTimeValue = _colorChangeTime;
    }
    /// <summary> プレイヤーからの攻撃処理 : ノックバック有り版 </summary>
    /// <param name="damage"> ダメージ量 </param>
    /// <param name="knockBackTimer"> ノックバック時間 </param>
    public void HitPlayerAttadk(int damage, float knockBackTimer)
    {
        //自身の体力を減らし、0.1秒だけ色を赤に変える。
        _hitPoint -= damage;
        _isColorChange = true;
        _colorChangeTimeValue = _colorChangeTime;

        //ノックバックする。プレイヤーのノックバック力(時間) - エネミーの耐久力(時間)分、Moveを停止する。
        _knockBackModeTime = (knockBackTimer - _weight) > 0f ? (knockBackTimer - _weight) : 0f;
        StartCoroutine(KnockBackMode());
    }
    /// <summary> プレイヤーに対する攻撃処理 : オーバーライド可 </summary>
    public virtual void HitPlayer()
    {
        //プレイヤーのHitPointを減らす
        PlayerStatusManager.Instance.PlayerHealthPoint -= _offensive_Power;
        _playersRigidBody2D.velocity = Vector2.zero;
        //プレイヤーをノックバックする
        if (_isRight)
        {
            _playersRigidBody2D.AddForce((Vector2.right + Vector2.up) * _blowingPower, ForceMode2D.Impulse);
        }
        else
        {
            _playersRigidBody2D.AddForce((Vector2.left + Vector2.up) * _blowingPower, ForceMode2D.Impulse);
        }
    }


    //<============= コルーチン =============>//
    /// <summary> ノックバック実行用コルーチン。 : ノックバック中かどうかを表す変数を一定時間 true にする。 </summary>
    IEnumerator KnockBackMode()
    {
        _isKnockBackNow = true;
        yield return new WaitForSeconds(_knockBackModeTime);
        _isKnockBackNow = false;
    }


    //<============= 仮想関数 =============>//
    /// <summary> Enemy移動用関数 : オーバーライド可 </summary>
    protected virtual void Move() { }
}
