using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// Enemyの基底クラス :  <br/>
/// ステータスを表す変数や、攻撃判定系メソッド、<br/>
/// ノックバック系メソッド、を用意。<br/>
/// </summary>
public class EnemyBase : MonoBehaviour, IAttackOnPlayer
{
    //===== メンバー変数 =====//
    //エネミー共通のステータス
    [SerializeField] protected EnemyStatus _status;
    public EnemyStatus Status { get => _status; }

    /// <summary> ドロップ品と確率の情報を格納する変数。 </summary>
    [Header("ドロップ品と確率"), Tooltip("IDは範囲外にならないように注意して設定してください。"), SerializeField]
    protected DropItemAndProbability[] _dropItemAndProbabilities;

    //このエネミーが向いている方向
    protected bool _isRight;
    public bool IsRight { get => _isRight; }


    //プレイヤーのコンポーネント
    protected Transform _playerPos;

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
    protected bool _isPause { get; private set; } = false;

    // ポーズ用
    float _angularVelocity;
    Vector2 _velocity;

    // コルーチン
    IEnumerator _colorChangeCoroutine;
    IEnumerator _moveStopCoroutine;

    //===== Unityメッセージ =====//
    protected virtual void Start()
    {
        Initialize_EnemyBase();
    }
    protected virtual void Update()
    {
        Update_Enemy();
    }
    protected virtual void OnEnable()
    {
        GameManager.OnPause += OnPause;
        GameManager.OnResume += OnResume;
    }
    protected virtual void OnDisable()
    {
        GameManager.OnPause -= OnPause;
        GameManager.OnResume -= OnResume;
    }

    /// <summary>
    /// ポーズ開始処理
    /// </summary>
    protected virtual void OnPause()
    {
        if (!_isPause)
        {
            _isPause = true;
            // RigidBody2Dを停止する処理をここに記述してください。
            _angularVelocity = _rigidBody2d.angularVelocity;
            _velocity = _rigidBody2d.velocity;
            _rigidBody2d.Sleep();
            _rigidBody2d.simulated = false;

            // コルーチンを停止する。
            PauseCoroutine();
        }

    }
    /// <summary>
    /// ポーズ解除処理
    /// </summary>
    protected virtual void OnResume()
    {
        if (_isPause)
        {
            _isPause = false;
            // RigidBody2Dを再開する処理。
            _rigidBody2d.simulated = true;
            _rigidBody2d.WakeUp();
            _rigidBody2d.angularVelocity = _angularVelocity;
            _rigidBody2d.velocity = _velocity;

            // コルーチンを再開。
            ResumeCoroutine();
        }
    }
    /// <summary>
    /// 登録されたコルーチンのポーズ処理 <br/>
    /// このメソッドは、Pause用に作成されたモノ。
    /// </summary>
    protected virtual void PauseCoroutine()
    {
        if (_moveStopCoroutine != null)
        {
            StopCoroutine(_moveStopCoroutine);
        }
        if (_colorChangeCoroutine != null)
        {
            StopCoroutine(_colorChangeCoroutine);
        }
    }
    /// <summary>
    /// 登録されたコルーチンの再開処理 <br/>
    /// このメソッドは、Pause用に作成されたモノ。
    /// </summary>
    protected virtual void ResumeCoroutine()
    {
        if (_moveStopCoroutine != null)
        {
            StartCoroutine(_moveStopCoroutine);
        }
        if (_colorChangeCoroutine != null)
        {
            StartCoroutine(_colorChangeCoroutine);
        }
    }

    //===== protectedメンバー関数 =====//
    /// <summary> 全エネミーで共通のEnemyの初期化関数。継承先のStart関数で呼び出す。 </summary>
    /// <returns> 成功したら true を返す。 </returns>
    protected bool Initialize_EnemyBase()
    {
        // プレイヤーのTransformを取得
        _playerPos = GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG_NAME).transform;
        if (_playerPos == null)
        {
            Debug.LogError($"PlayerのTransformコンポーネントを取得できませんでした。 : {gameObject.name}");
            return false;
        }

        // スプライトレンダラーコンポーネントを取得する。
        _spriteRenderer = GetComponent<SpriteRenderer>();
        if (_spriteRenderer == null)
        {
            Debug.LogError($"SpriteRendererの取得に失敗しました。 : {gameObject.name}");
            return false;
        }
        // Rigidbody2Dを取得する。
        _rigidBody2d = GetComponent<Rigidbody2D>();
        if (_rigidBody2d == null)
        {
            Debug.LogError($"Rigidbody2Dの取得に失敗しました。 : {gameObject.name}");
            return false;
        }

        return true;
    }
    /// <summary> 全エネミーで共通のEnemyのUpdate関数。継承先のUpdate関数で呼び出す。 </summary>
    protected void Update_Enemy()
    {
        ChangeColor();
        Move();
    }
    /// <summary> ノックバック処理 </summary>
    /// <param name="knockBackPower"></param>
    protected void StartKnockBack(float knockBackPower)
    {
        _rigidBody2d.velocity = Vector2.zero;
        float diff = PlayerStatusManager.Instance.IsRight ? Constants.LEFT : Constants.RIGHT;
        _rigidBody2d.AddForce((Vector2.up + Vector2.right * diff) * knockBackPower, ForceMode2D.Impulse);
    }
    /// <summary>
    /// アイテムをドロップする処理。
    /// </summary>
    protected void DropItems()
    {
        // 配列に入ったオブジェクトを、ドロップ率を基に落とすかどうかを判定する。
        foreach (var i in _dropItemAndProbabilities)
        {
            // nullチェック
            if (i._dropGameObjectPrefab != null)
            {
                // 確率を判定する
                if (i._probability > Random.Range(0f, 99.99f))
                {
                    // 判定を通り抜けた場合、オブジェクトを生成し、IDをセットする。
                    var drop = Instantiate(i._dropGameObjectPrefab, transform.position, Quaternion.identity);
                    if (drop.TryGetComponent(out IDrops drops))
                    {
                        drops.SetID(i._iD);
                    }
                }
            }
            else
            {
                Debug.LogError("アイテムを設定してください！");
            }
        }
    }
    //<============= publicメンバー関数 =============>//
    //***** 攻撃ヒット関連 *****//
    /// <summary> プレイヤーからこのエネミーに対する攻撃処理。 : ノックバック無し版 </summary>
    /// <param name="playerOffensivePower"> ダメージ量 </param>
    public virtual void HitPlayerAttack(float playerOffensivePower)
    {
        //自身の体力を減らし、一定時間 色を被ダメ用の色に変える。
        _status._hitPoint -= playerOffensivePower;//体力がなくなった時の処理
        if (_status._hitPoint <= 0)
        {
            // アイテムをドロップする。
            DropItems();
            // 体力がなくなった時の処理を実行
            Deth();
        }
        // 攻撃がヒットしたことを演出するために色を変更する。
        _colorChangeCoroutine = ColorChange();
        StartCoroutine(_colorChangeCoroutine);
    }
    /// <summary> プレイヤーからこのエネミーに対する攻撃処理。 : ノックバック有り版 </summary>
    /// <param name="playerOffensivePower"> ダメージ量 </param>
    /// <param name="knockBackTimer"> ノックバック時間 </param>
    public virtual void HitPlayerAttack(float playerOffensivePower, float knockBackTimer, float knockBackPower)
    {
        //自身の体力を減らす。
        _status._hitPoint -= playerOffensivePower;
        if (_status._hitPoint <= 0)
        {
            // アイテムをドロップする。
            DropItems();
            // 体力がなくなった時の処理を実行
            Deth();
        }
        //攻撃がヒットしたことを表現する為に一定時間色を変更する。
        _colorChangeCoroutine = ColorChange();
        StartCoroutine(_colorChangeCoroutine);

        //指定された時間だけ移動を停止する。
        _knockBackModeTime = (knockBackTimer - _status._weight) > 0f ? (knockBackTimer - _status._weight) : 0f;
        _moveStopCoroutine = MoveStop();
        StartCoroutine(_moveStopCoroutine);
        //ノックバック処理。
        StartKnockBack((knockBackPower - _status._weight) > 0f ? knockBackPower - _status._weight : 0f);
    }
    public virtual void HitPlayer(Rigidbody2D playerRigidbody2D)
    {
        //プレイヤーのHitPointを減らす
        PlayerStatusManager.Instance.PlayerHealthPoint -= _status._offensivePower;
        playerRigidbody2D.velocity = Vector2.zero;
        //プレイヤーをノックバックする
        if (_isRight)
        {
            playerRigidbody2D.AddForce(Vector2.right * _status._blowingPower.x + Vector2.up * _status._blowingPower.y, ForceMode2D.Impulse);
        }
        else
        {
            playerRigidbody2D.AddForce(Vector2.left * _status._blowingPower.x + Vector2.up * _status._blowingPower.y, ForceMode2D.Impulse);
        }
    }


    //===== コルーチン =====//
    /// <summary> 
    /// ノックバック実行用コルーチン。 : <br/>
    /// 一定時間 _isMove変数を、falseにする。<br/>
    /// </summary>
    IEnumerator MoveStop()
    {
        // 計測用タイマー
        float timer = 0f;
        // 移動可能かどうか表す変数を false にする。
        _isMove = false;
        // 停止処理
        while (timer < _knockBackModeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        // 移動可能かどうか表す変数を true にする。
        _isMove = true;
        // コルーチン終了処理
        _moveStopCoroutine = null;
    }
    /// <summary> プレイヤーから攻撃を受けると、指定された時間だけ色が変わる。 </summary>
    IEnumerator ColorChange()
    {
        float timer = 0f;
        _isColorChange = true;
        while (timer < _colorChangeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        _isColorChange = false;
        _colorChangeCoroutine = null;
    }

    //<============= 仮想関数 =============>//
    /// <summary> Enemy移動用関数 : オーバーライド可 </summary>
    protected virtual void Move() { }
    /// <summary>
    /// このエネミーに対して攻撃がヒットしたことを演出するためのメソッド。<br/>
    /// 色を一定時間変更する。<br/>
    /// </summary>
    protected virtual void ChangeColor()
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
    /// <summary>
    /// 体力がなくなった時の処理 : <br/>
    /// オーバーライド可。<br/>
    /// そのまま使用する場合は自分自身を破棄する。<br/>
    /// </summary>
    protected virtual void Deth()
    {
        Destroy(gameObject);
    }
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
    /// <summary> 移動速度 </summary>
    public float _moveSpeed;

    /// <summary> EnemyStatusのコンストラクタ </summary>
    /// <param name="hitPoint"> 体力 </param>
    /// <param name="offensivePower"> 攻撃力 </param>
    /// <param name="blowingPowerUp"> 吹っ飛ばし力 : 上方向の威力 </param>
    /// <param name="blowingPowerRight"> 吹っ飛ばし力 : 右方向の威力 </param>
    /// <param name="weight"> 重さ : 吹っ飛びにくさ </param>
    public EnemyStatus(int hitPoint = 1, int offensivePower = 1, int blowingPowerUp = 1, int blowingPowerRight = 1, float weight = 1f, float moveSpeed = 1f)
    {
        _hitPoint = hitPoint;
        _offensivePower = offensivePower;
        _blowingPower = Vector2.up * blowingPowerUp + Vector2.right * blowingPowerRight;
        _weight = weight;
        _moveSpeed = moveSpeed;
    }
}

/// <summary>
/// この型は、倒せる、壊せるものが持つべき構造体。
/// 落とすアイテム、落とす装備、ドロップ率変数をメンバーに持つ。
/// </summary>
[System.Serializable]
public struct DropItemAndProbability
{
    /// <summary> 落とすモノのID </summary>
    public int _iD;
    /// <summary> 落とすアイテム、あるいは装備のプレハブ </summary>
    public GameObject _dropGameObjectPrefab;
    /// <summary> ドロップ率(%) </summary>
    public float _probability;
}