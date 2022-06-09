using UnityEngine;
using System.Collections.Generic;

public class BossBase : MonoBehaviour
{
    public enum BossState
    {
        IDLE,//待機
        APPROACH,//接近
        RECESSION,//後退

        HEAVY_ATTACK,//強攻撃
        LIGHT_ATTACK,//弱攻撃
        LONG_RANGE_ATTACK,//遠距離攻撃

        END
    }

    protected struct CoolTimeRandomValue
    {
        float minValue;
        float maxValue;
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
    private bool _isBattle = false;
    /// <summary> 現在のクールタイム </summary>
    private float _coolTimeValue = 0f;
    /// <summary> 現在のクールタイム </summary>
    [Tooltip("戦闘開始までの距離"), SerializeField] private Vector2 _fightStartDistance;
    [Tooltip("戦闘停止までの距離"), SerializeField] private Vector2 _fightStopDistance;
    //色変更用
    bool _isColorChange = false;
    float _colorChangeTime = 0;
    /// <summary> ボス攻撃後のクールタイム </summary>
    Dictionary<BossState, float> _bossAttackCoolTime = new Dictionary<BossState, float>();


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
    protected void BossUpdate()
    {
        
    }

    /// <summary> 距離を測り、戦闘するかどうか判断する </summary>
    void DoFight()
    {
        //プレイヤーとの距離を測る処理をここに書く

        //距離が一定以上近づいたら_isFightをTrueにする

        //距離が一定以上離れたら_isFightをFalseにする
    }

    /// <summary> ボスのステートを、必要に応じて変更する </summary>
    void CangeState()
    {

    }


    /// <summary> プレイヤーと接触したときに呼ばれる </summary>
    void HitPlayer()//プレイヤーの体力を減らし、ノックバックさせる。
    {

    }

    /// <summary> プレイヤーの攻撃に接触したときに呼ばれる </summary>
    void HitPlayerAttack()//自分の体力を減らし、必要であればノックバックする。
    {

    }

    /// <summary> 戦闘開始時の処理 </summary>
    void BattleStart()
    {

    }

    /// <summary> 戦闘終了時の処理 </summary>
    void BattleExit()
    {

    }
}
