using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>Bossの基底クラス :<br/>
/// EnemyBase を継承している。</para>
/// 追加要素 : <br/>
/// ステートを表す型と、変数を用意。<br/>
/// 攻撃のバリエーションを増やすために<br/>
/// 様々なメンバーを追加<br/>
/// </summary>
public class NewBossBase : EnemyBase
{
    //<=========== このクラスで使用する型 ===========>//

    //<============= メンバー変数 =============>//
    // 戦闘中かどうか関連
    /// <summary> 現在のフレームで戦闘状態か？ : 戦闘中であれば true </summary>
    private bool _isFight = false;
    /// <summary> 前のフレームで戦闘状態だったか？ : 戦闘中であれば true </summary>
    private bool _beforeFrameIsFight = false;

    //クールタイム関連
    /// <summary> 現在クールタイム中かどうか </summary>
    protected bool _isCoolTimerNow = false;
    /// <summary> 現在クールタイム中かどうかの前フレームの値 </summary>
    protected bool _beforeIsCoolTimerNow = false;
    /// <summary> クールタイム時間 </summary>
    protected float _coolTimeValue = 0f;
    /// <summary> 現在攻撃中かどうか </summary>
    protected bool _isAttackNow = false;

    /// <summary> ボス攻撃後のクールタイム : キーはBossStateで、ValueはRandomRangeValue型。 </summary>
    [Header("ボス攻撃後のクールタイム"), SerializeField]
    Dictionary<BossState, RandomRangeValue> _bossAttackCoolTime = new Dictionary<BossState, RandomRangeValue>();

    [Tooltip("戦闘開始までの距離"), SerializeField] private Vector2 _fightStartDistance;
    [Tooltip("戦闘停止までの距離"), SerializeField] private Vector2 _fightStopDistance;

    /// <summary> 現在のステート </summary>
    public BossState _nowState { get; protected set; }

    protected Animator _animator;

    //===== Unityメッセージ =====//
    protected override void Start()
    {
        base.Start();
        Initialize_BossBase();
    }
    protected override void Update()
    {
        base.Update();
        CommonUpdate_BossBase();
    }

    //<============= protectedメンバー関数 =============>//
    /// <summary>
    /// BossBase の初期化関数<br/>
    /// Animatorコンポーネント取得しメンバー変数に保存する。<br/>
    /// </summary>
    /// <returns> 成功したら true を返す。 </returns>
    protected bool Initialize_BossBase()
    {
        if (!(_animator = GetComponent<Animator>()))
        {
            Debug.LogError($"Animatorコンポーネントの取得に失敗しました。 : {gameObject.name}");
            return false;
        }
        return true;
    }
    /// <summary> ボス共通の更新処理 : オーバーライド可 </summary>
    protected virtual void CommonUpdate_BossBase()
    {
        // 必要であれば攻撃開始処理を行う
        if(_beforeIsCoolTimerNow == false && _isCoolTimerNow == true)
            StartAttackProcess();
        // 必要であれば攻撃終了処理を行う
        else if (_beforeIsCoolTimerNow == true && _isCoolTimerNow == false)
            EndAttackProcess();

        // 現在のステート別に処理を行う
        ManageState();

        // 後のフレーム用に、クールタイムかどうかを判定する値を保存しておく。
        _beforeIsCoolTimerNow = _isCoolTimerNow;
    }

    //<============= privateメンバー関数 =============>//

    //<============= コルーチン =============>//
    /// <summary> クールタイムを開始する。 : 指定された時間クールタイム変数を true にする。 </summary>
   　protected IEnumerator StartCoolTime()
    {
        _isCoolTimerNow = true;
        yield return new WaitForSeconds(_coolTimeValue);
        _isCoolTimerNow = false;
    }


    //<============= 仮想関数 =============>//
    /// <summary> 
    /// 攻撃開始処理 : オーバーライド推奨 : <br/>
    /// オーバーライド先でアニメーションの遷移処理等の<br/>
    /// 攻撃開始に関わる処理を記述してください。<br/>
    /// </summary>
    protected virtual void StartAttackProcess() { }
    /// <summary> 
    /// 攻撃終了処理 : オーバーライド推奨 : <br/>
    /// オーバーライド先でアニメーションの遷移処理や<br/>
    /// クールタイム開始処理等の、攻撃終了に関わる処理を記述してください。<br/>
    /// </summary>
    protected virtual void EndAttackProcess() { }
    /// <summary> 
    /// ステートを管理するメソッド : オーバーライド推奨 : <br/>
    /// オーバーライド先でステート別の処理を行うように記述してください。<br/>
    /// </summary>
    protected virtual void ManageState() { }
}
/// <summary> ボスのステートを表す型 </summary>
public enum BossState
{
    /// <summary> 待機 </summary>
    IDLE,
    /// <summary> 接近 </summary>
    APPROACH,
    /// <summary> 後退 </summary>
    RECESSION,
    /// <summary> 突進 </summary>
    RUSH,
    /// <summary> 逃走 </summary>
    ESCAPE,

    NOMAL_END,

    /// <summary> 弱攻撃その1 </summary>
    LIGHT_ATTACK_PATTERN_ONE,
    /// <summary> 弱攻撃その2 </summary>
    LIGHT_ATTACK_PATTERN_TOW,
    /// <summary> 弱攻撃その3 </summary>
    LIGHT_ATTACK_PATTERN_THREE,

    /// <summary> 強攻撃その1 </summary>
    HEAVY_ATTACK_PATTERN_ONE,
    /// <summary> 強攻撃その2 </summary>
    HEAVY_ATTACK_PATTERN_TOW,
    /// <summary> 強攻撃その3 </summary>
    HEAVY_ATTACK_PATTERN_THREE,

    /// <summary> 遠距離攻撃その1 </summary>
    LONG_RANGE_ATTACK_PATTERN_ONE,
    /// <summary> 遠距離攻撃その2 </summary>
    LONG_RANGE_ATTACK_PATTERN_TOW,
    /// <summary> 遠距離攻撃その3 </summary>
    LONG_RANGE_ATTACK_PATTERN_THREE,

    ATTACK_END,

    /// <summary> 死 </summary>
    DIE,
}
/// <summary> ランダムな値の最小値と最大値のセット </summary>
[System.Serializable]
struct RandomRangeValue
{
    /// <summary> ランダムな値の最小値 </summary>
    public float _minValue;
    /// <summary> ランダムな値の最大値 </summary>
    public float _maxValue;
}
