using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class BossBase : MonoBehaviour
{
    public enum BossState
    {
        IDLE,//待機
        APPROACH,//接近
        RECESSION,//後退
        NOMAL_END,

        LIGHT_ATTACK,//弱攻撃
        HEAVY_ATTACK,//強攻撃
        LONG_RANGE_ATTACK,//遠距離攻撃

        ATTACK_END
    }

    [Tooltip("体力"), SerializeField] protected int _hitPoint;
    [Tooltip("攻撃力"), SerializeField] protected int _offensive_Power;
    [Tooltip("プレイヤーに対するノックバック力"), SerializeField] protected Vector2 _playerKnockBackPower;

    //プレイヤーのコンポーネント
    GameObject _playerObjedt;
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

    /// <summary> 戦闘中かどうか関連 </summary>
    private bool _previousFrameIsBattle = false;
    private bool _isBattle = false;

    //クールタイム関連
    /// <summary> 現在のクールタイム </summary>
    protected bool _isCoolTimeNow = false;
    [SerializeField] protected float _coolTimeValue = 0f;
    /// <summary> ボス攻撃後のクールタイム </summary>
    Dictionary<BossState, float> _bossAttackCoolTime = new Dictionary<BossState, float>();

    [Tooltip("戦闘開始までの距離"), SerializeField] private Vector2 _fightStartDistance;
    [Tooltip("戦闘停止までの距離"), SerializeField] private Vector2 _fightStopDistance;

    //色変更用
    bool _isColorChange = false;
    float _colorChangeTime = 0.1f;
    float _colorChangeTimeValue = 0;

    /// <summary> ノックバック関連 </summary>
    bool _isKnockBackNow;
    float _knockBackModeTime = 0f;

    //自分の状態
    protected BossState _nowState;

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
        if (_isBattle)
        {
            CangeState();
            UpdateBoss();
        }
    }

    /// <summary> 距離を測り、戦闘するかどうか判断する </summary>
    void DoFight()
    {
        //前フレームの状態を保存する
        _previousFrameIsBattle = _isBattle;
        //プレイヤーとの距離を測る処理をここに書く
        Vector2 difference = _playerPos.position - transform.position;
        float diffX = Mathf.Abs(difference.x);
        float diffY = Mathf.Abs(difference.y);
        //距離が一定以上近づいたら_isFightをTrueにする
        if (diffX < 7.5f && diffY < 5f)
        {
            _isBattle = true;
        }
        //距離が一定以上離れたら_isFightをFalseにする
        if (diffX > 15f || diffY > 10f)
        {
            _isBattle = false;
        }
    }

    /// <summary> ボスのステートを、必要に応じて変更する。要 Over ride </summary>
    virtual protected void CangeState() { }

    /// <summary> ボスの処理。要 Over ride </summary>
    virtual protected void UpdateBoss() { }


    /// <summary> プレイヤーと接触したときに呼ばれる </summary>
    void HitPlayer()//プレイヤーの体力を減らし、ノックバックさせる。
    {
        //プレイヤーのHitPointを減らす
        _playerBasicInformation._playerHitPoint -= _offensive_Power;
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

    /// <summary> プレイヤーの攻撃に接触したときに呼ばれる </summary>
    void HitPlayerAttack(int damage)//自分の体力を減らす。
    {
        //自身の体力を減らし、0.1秒だけ色を赤に変える。
        _hitPoint -= damage;
        _isColorChange = true;
        _colorChangeTimeValue = _colorChangeTime;
        StartCoroutine(KnockBackMode());
    }

    //ノックバック用のコード。
    IEnumerator KnockBackMode()
    {
        _isKnockBackNow = true;
        yield return new WaitForSeconds(_knockBackModeTime);
        _isKnockBackNow = false;
    }

    /// <summary> 戦闘開始時の処理 </summary>
    protected bool CommonBattleStart()
    {
        //_isBattleがfalseからtrueになったフレームのみ実行する
        if (_previousFrameIsBattle == false && _isBattle == true)
        {
            //戦闘開始時に実行したい処理をここに書く
            return true;
        }
        return false;
    }

    /// <summary> 戦闘終了時の処理 </summary>
    protected bool CommonBattleExit()
    {
        //_isBattleがtrueからfalseになったフレームのみ実行する
        if (_previousFrameIsBattle == true && _isBattle == false)
        {
            //戦闘終了時に実行したい処理をここに書く
            return true;
        }
        return false;
    }

    virtual protected void BattleStart() { }

    virtual protected void BattleExit() { }
}
