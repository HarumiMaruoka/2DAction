using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> Enemyの基底クラス </summary>
public class EnemyBase : MonoBehaviour
{
    //<============= メンバー変数 =============>//
    //エネミー共通のステータス
    [SerializeField] protected EnemyStatus _status;
    public EnemyStatus Status { get => _status; }

    /// <summary> ドロップ品と確率の情報を格納する変数。 </summary>
    [Header("ドロップ品と確率"), SerializeField]
    protected DropItemAndProbability[] _dropItemAndProbabilities;

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
    const float _colorChangeTime = 0.1f;
    /// <summary> 色を変更する時間 : 現在の残り時間 </summary>
    protected float _colorChangeTimeValue = 0;

    //ノックバック関連
    /// <summary> ノックバック時間 </summary>
    float _knockBackModeTime = 0f;
    protected bool _isMove = true;


    //<============== protectedメンバー関数 ==============>//
    /// <summary> 全エネミーで共通のEnemyの初期化関数。継承先のStart関数で呼び出す。 </summary>
    /// <returns> 成功したら true を返す。 </returns>
    protected bool Initialize_Enemy()
    {
        if (!EnemyInitialize_Get_PlayerComponents())
        {
            Debug.LogError($"初期化に失敗しました。 : {gameObject.name}");
            return false;
        }
        if (!EnemyInitialize_Get_ThisGameObjectComponents())
        {
            Debug.LogError($"初期化に失敗しました。 : {gameObject.name}");
            return false;
        }

        return true;
    }
    protected virtual void Update()
    {
        Update_Enemy();
    }
    //全エネミーで共通のEnemyのUpdate関数。継承先のUpdate関数で呼び出す
    protected void Update_Enemy()
    {
        //色を変える必要があれば変える
        if (_isColorChange)
        {
            _spriteRenderer.color = Color.red;
        }
        //色を元に戻す
        else
        {
            _spriteRenderer.color = new Color(255, 255, 255, 255);
        }
    }
    protected void StartKnockBack(float knockBackPower)
    {
        _rigidBody2d.velocity = Vector2.zero;
        _rigidBody2d.AddForce((Vector2.up + Vector2.right) * knockBackPower, ForceMode2D.Impulse);
    }
    //<============= private関数 =============>//
    //***** 初期化関連 *****//
    /// <summary> プレイヤーにアタッチされているコンポーネントを取得する。 </summary>
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


    //<============= publicメンバー関数 =============>//
    //***** 攻撃ヒット関連 *****//
    /// <summary> プレイヤーからの攻撃処理 : ノックバック無し版 </summary>
    /// <param name="playerOffensivePower"> ダメージ量 </param>
    public void HitPlayerAttack(float playerOffensivePower)
    {
        //自身の体力を減らし、一定時間 色を被ダメ用の色に変える。
        _status._hitPoint -= playerOffensivePower;//体力がなくなった時の処理
        if (_status._hitPoint <= 0)
        {
            //体力がなくなったら消滅する
            Destroy(this.gameObject);
        }
        StartCoroutine(ColorChange());
        _colorChangeTimeValue = _colorChangeTime;
    }
    /// <summary> プレイヤーからの攻撃処理 : ノックバック有り版 </summary>
    /// <param name="playerOffensivePower"> ダメージ量 </param>
    /// <param name="knockBackTimer"> ノックバック時間 </param>
    public void HitPlayerAttack(float playerOffensivePower, float knockBackTimer, float knockBackPower)
    {
        //自身の体力を減らし、0.1秒だけ色を赤に変える。
        _status._hitPoint -= playerOffensivePower;
        if (_status._hitPoint <= 0)
        {
            //体力がなくなったら消滅する
            Destroy(this.gameObject);
        }
        StartCoroutine(ColorChange());
        _colorChangeTimeValue = _colorChangeTime;

        //指定された時間だけ移動を停止する。
        _knockBackModeTime = (knockBackTimer - _status._weight) > 0f ? (knockBackTimer - _status._weight) : 0f;
        StartCoroutine(MoveStop());
        //ノックバック処理。
        StartKnockBack((knockBackPower - _status._weight) > 0f ? knockBackPower - _status._weight : 0f);
    }
    /// <summary> プレイヤーに対する攻撃処理 : オーバーライド可 </summary>
    public virtual void HitPlayer()
    {
        //プレイヤーのHitPointを減らす
        PlayerStatusManager.Instance.PlayerHealthPoint -= _status._offensivePower;
        _playersRigidBody2D.velocity = Vector2.zero;
        //プレイヤーをノックバックする
        if (_isRight)
        {
            _playersRigidBody2D.AddForce((Vector2.right + Vector2.up) * _status._blowingPower, ForceMode2D.Impulse);
        }
        else
        {
            _playersRigidBody2D.AddForce((Vector2.left + Vector2.up) * _status._blowingPower, ForceMode2D.Impulse);
        }
    }

    //<============= コルーチン =============>//
    /// <summary> ノックバック実行用コルーチン。 : ノックバック中かどうかを表す変数を一定時間 true にする。 </summary>
    IEnumerator MoveStop()
    {
        _isMove = false;
        yield return new WaitForSeconds(_knockBackModeTime);
        _isMove = true;
    }
    /// <summary> プレイヤーから攻撃を受けると、指定された時間だけ色が変わる。 </summary>
    IEnumerator ColorChange()
    {
        _isColorChange = true;
        yield return new WaitForSeconds(_colorChangeTime);
        _isColorChange = false;
    }

    //<============= 仮想関数 =============>//
    /// <summary> Enemy移動用関数 : オーバーライド可 </summary>
    protected virtual void Move() { }
}
/// <summary> Enemyのステータスを表す型 </summary>
[System.Serializable]
public struct EnemyStatus
{
    /// <summary> 体力 </summary>
    public float _hitPoint;
    /// <summary> 攻撃力 </summary>
    public float _offensivePower;
    /// <summary> 吹っ飛ばし力/方向 </summary>
    public Vector2 _blowingPower;
    /// <summary> 吹っ飛びにくさ/重量 </summary>
    public float _weight;

    /// <summary> EnemyStatusのコンストラクタ </summary>
    /// <param name="hitPoint"> 体力 </param>
    /// <param name="offensivePower"> 攻撃力 </param>
    /// <param name="blowingPowerUp"> 吹っ飛ばし力 : 上方向の威力 </param>
    /// <param name="blowingPowerRight"> 吹っ飛ばし力 : 右方向の威力 </param>
    /// <param name="weight"> 重さ : 吹っ飛びにくさ </param>
    public EnemyStatus(int hitPoint = 1, int offensivePower = 1, int blowingPowerUp = 1, int blowingPowerRight = 1, float weight = 1f)
    {
        _hitPoint = hitPoint;
        _offensivePower = offensivePower;
        _blowingPower = Vector2.up * blowingPowerUp + Vector2.right * blowingPowerRight;
        _weight = weight;
    }
}

/// <summary>
/// この型は、倒せる、壊せるものが持つべき構造体。
/// 落とすアイテム、落とす装備、ドロップ率変数をメンバーに持つ。
/// </summary>
[System.Serializable]
public struct DropItemAndProbability
{
    /// <summary> 落とすアイテム </summary>
    Item.ItemID _itemID;
    /// <summary> 落とす装備 </summary>
    EquipmentDataBase.EquipmentID _equipmentID;
    /// <summary> ドロップ率(%) </summary>
    float _probability;
}