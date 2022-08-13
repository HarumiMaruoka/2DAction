using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary> Bossの基底クラス。 :
/// ***** 現在MonoBehaviourを継承しているが、のちにEnemyBaseを継承すべき。 *****
/// ***** 現在新しいBossBaseを作成中 *****
/// </summary>
public class BossBase : MonoBehaviour
{
    //<=========== このクラスで使用する型 ===========>//
    /// <summary> ボスのステートを表す型 </summary>
    public enum BossState
    {
        IDLE,//待機
        APPROACH,//接近
        RECESSION,//後退
        NOMAL_END,

        LIGHT_ATTACK,//弱攻撃
        HEAVY_ATTACK,//強攻撃
        LONG_RANGE_ATTACK,//遠距離攻撃

        ATTACK_END,

        DIE,//死
    }


    //<============= メンバー変数 =============>//

    [Tooltip("体力"), SerializeField] 
    protected int _hitPoint;
    [Tooltip("攻撃力"), SerializeField]
    protected int _offensive_Power;
    [Tooltip("プレイヤーに対するノックバック力"), SerializeField]
    protected Vector2 _playerKnockBackPower;

    //プレイヤーのコンポーネント
    protected GameObject _playerObjedt;
    protected Transform _playerPos;
    protected PlayerBasicInformation _playerBasicInformation;
    protected Rigidbody2D _playersRigidBody2D;
    protected PlayerMoveManager _playerMoveManager;

    //自身のコンポーネント
    protected SpriteRenderer _spriteRenderer;
    protected Rigidbody2D _rigidBody2d;
    protected Animator _animator;

    //各パラメータ
    /// <summary> このキャラが向いている方向 </summary>
    protected bool _isRight;

    // 戦闘中かどうか関連
    /// <summary> 前のフレームで戦闘状態だったか？ : 戦闘中であれば true </summary>
    private bool _beforeFrameIsFight = false;
    /// <summary> 現在のフレームで戦闘状態か？ : 戦闘中であれば true </summary>
    private bool _isFight = false;

    //クールタイム関連
    /// <summary> 現在のクールタイム </summary>
    protected bool _isCoolTimerStart = false;
    [SerializeField] protected float _coolTimeValue = 0f;
    protected bool _isCoolTimeExit = false;

    //攻撃関連
    /// <summary> 攻撃を開始するか？ : 開始するフレームで true </summary>
    protected bool _isAttackStart = false;
    /// <summary> 攻撃を終了するか？ : 終了するフレームで true </summary>
    protected bool _isAttackExit = false;

    /// <summary> ボス攻撃後のクールタイム </summary>
    Dictionary<BossState, float> _bossAttackCoolTime = new Dictionary<BossState, float>();

    [Tooltip("戦闘開始までの距離"), SerializeField] private Vector2 _fightStartDistance;
    [Tooltip("戦闘停止までの距離"), SerializeField] private Vector2 _fightStopDistance;

    //色変更用
    /// <summary> Hit用 : 色を変更するかどうか </summary>
    protected bool _isColorChange = false;
    /// <summary> Hit用 : 色を変更する時間。 </summary>
    protected float _colorChangeTime = 0.1f;
    /// <summary> Hit用 : 色を変更する時間、残り時間。 </summary>
    protected float _colorChangeTimeValue = 0;

    /// <summary> ノックバック関連 </summary>
    bool _isKnockBackNow;
    float _knockBackModeTime = 0f;

    /// <summary> 現在のステート </summary>
    public BossState _nowState { get; protected set; }


    //<============= protectedメンバー関数 =============>//
    /// <summary> Boss共通の初期化処理 </summary>
    protected void InitBoss()
    {
        //プレイヤーのコンポーネントを取得
        _playerObjedt = GameObject.Find("ChibiRobo");
        _playerBasicInformation = _playerObjedt.GetComponent<PlayerBasicInformation>();
        _playerPos = _playerObjedt.GetComponent<Transform>();
        _playersRigidBody2D = _playerObjedt.GetComponent<Rigidbody2D>();
        _playerMoveManager = _playerObjedt.GetComponent<PlayerMoveManager>();

        //自身のコンポーネントを取得
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _rigidBody2d = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
    }

    /// <summary> 継承先のUpdateで実行すべき関数 </summary>
    protected void CommonUpdateBoss()
    {
        DoFight();//戦うかどうか
        CommonBattleStart();//戦闘開始時のみ実行
        BattleStart();//独自の戦闘開始時の処理
        CommonBattleExit();//戦闘終了時のみ実行
        BattleExit();//独自の戦闘終了時の処理
        if (_isFight)
        {
            UpdateBoss();
        }
    }

    /// <summary> 距離を測り、戦闘するかどうか判断する </summary>
    void DoFight()
    {
        //前フレームの状態を保存する
        _beforeFrameIsFight = _isFight;
        //プレイヤーとの距離を測る処理をここに書く
        Vector2 difference = _playerPos.position - transform.position;
        _spriteRenderer.flipX = difference.x > 0f;
        float diffX = Mathf.Abs(difference.x);
        float diffY = Mathf.Abs(difference.y);
        //距離が一定以上近づいたら_isFightをTrueにする
        if (diffX < 10f && diffY < 5f)
        {
            _isFight = true;
        }
        //距離が一定以上離れたら_isFightをFalseにする
        else if (diffX > 25f || diffY > 10f)
        {
            _isFight = false;
        }
    }



    /// <summary> プレイヤーと接触したときに呼ばれる </summary>
    public void HitPlayer()//プレイヤーの体力を減らし、ノックバックさせる。
    {
        //プレイヤーのHitPointを減らす
        PlayerStatusManager.Instance.PlayerHealthPoint -= _offensive_Power;
        _playersRigidBody2D.velocity = Vector2.zero;
        //プレイヤーをノックバックする
        if (_spriteRenderer.flipX)
        {
            _playersRigidBody2D.AddForce((Vector2.right + Vector2.up) * _playerKnockBackPower, ForceMode2D.Impulse);
        }
        else
        {
            _playersRigidBody2D.AddForce((Vector2.left + Vector2.up) * _playerKnockBackPower, ForceMode2D.Impulse);
        }
    }

    /// <summary> プレイヤーの攻撃に接触したときに呼ばれる </summary>
    public void HitPlayerAttack(int damage)//自分の体力を減らす。
    {
        //自身の体力を減らし、0.1秒だけ色を赤に変える。
        _hitPoint -= damage;
        _isColorChange = true;
        _colorChangeTimeValue = _colorChangeTime;
        StartCoroutine(KnockBackMode());
    }

    

    /// <summary> Boss共通の戦闘開始時の処理 </summary>
    protected virtual void CommonBattleStart()
    {
        if (IsBattleStart())
        {

        }
    }

    /// <summary> Boss共通の戦闘終了時の処理 </summary>
    protected virtual void CommonBattleExit()
    {
        if (IsBattleExit())
        {
            _nowState = BossState.IDLE;
        }
    }

    /// <summary> 戦いを始めるかどうか判定する。 </summary>
    /// <returns> 戦う場合 true を返す。 </returns>
    protected bool IsBattleStart()
    {
        if (_beforeFrameIsFight == false && _isFight == true)
        {
            return true;
        }
        return false;
    }
    /// <summary> 戦いを終了するかどうか判定する。 </summary>
    /// <returns> 終了する場合 false を返す。 </returns>
    protected bool IsBattleExit()
    {
        if (_beforeFrameIsFight == true && _isFight == false)
        {
            return true;
        }
        return false;
    }

    //<============= コルーチン =============>//
    /// <summary> ノックバック処理 </summary>
    IEnumerator KnockBackMode()
    {
        _isKnockBackNow = true;
        yield return new WaitForSeconds(_knockBackModeTime);
        _isKnockBackNow = false;
    }

    //<============= 仮想関数 =============>//
    /// <summary> 派生先で独自の戦闘開始の処理をここに書く。オーバーライド可 </summary>
    virtual protected void BattleStart() { }
    /// <summary> 派生先で独自の戦闘終了の処理をここに書く。オーバーライド可 </summary>
    virtual protected void BattleExit() { }
    /// <summary> ボスのステートを、必要に応じて変更する。オーバーライド可 </summary>
    virtual protected void CangeState() { }
    /// <summary> ボスの処理。オーバーライド可 </summary>
    virtual protected void UpdateBoss() { }
}
