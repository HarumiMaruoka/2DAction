using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bossの基底クラス :
/// 新しい、BossBaseコンポーネント。
/// 今はまだ使用していない。
/// 
/// 変更点 : EnemyBaseを継承している点。
///          その他いろいろ最適化
/// </summary>
public class NewBossBase : EnemyBase
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
    // 戦闘中かどうか関連
    /// <summary> 現在のフレームで戦闘状態か？ : 戦闘中であれば true </summary>
    private bool _isFight = false;
    /// <summary> 前のフレームで戦闘状態だったか？ : 戦闘中であれば true </summary>
    private bool _beforeFrameIsFight = false;

    //クールタイム関連
    /// <summary> 現在クールタイム中かどうか </summary>
    protected bool _isCoolTimerNow = false;
    /// <summary> 現在クールタイム中かどうかの前フレームの値 </summary>
    private bool _beforeIsCoolTimerNow = false;
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


    //<============= protectedメンバー関数 =============>//
    /// <summary>
    /// BossBaseの初期化関数<br/>
    /// 基底クラスの初期化関数を実行し、<br/>
    /// Animatorコンポーネントをメンバー変数に保存する。<br/>
    /// </summary>
    /// <returns> 成功したら true を返す。 </returns>
    protected bool Initialize_BossBase()
    {
        if (!base.Initialize_Enemy())
        {
            Debug.LogError($"初期化に失敗しました。{gameObject.name}");
            return false;
        }
        if (!(_animator=GetComponent<Animator>()))
        {
            Debug.LogError($"Animatorコンポーネントの取得に失敗しました。 : {gameObject.name}");
            Debug.LogError($"初期化に失敗しました。{gameObject.name}");
            return false;
        }
        return true;
    }
    /// <summary> ボス共通の更新処理 : オーバーライド可 </summary>
    protected virtual void CommonUpdate_BossBase()
    {
        // 後のフレーム用に、クールタイムかどうかを判定する値を保存しておく。
        _beforeIsCoolTimerNow = _isCoolTimerNow;

        // 以下は判定を内部で行っているので、実行すべきタイミングで勝手に実行してくれる。
        Update_StartAttackProcess();
        Update_EndAttackProcess();
    }

    /// <summary> 攻撃開始のフレームを検知する </summary>
    /// <returns> 攻撃開始のフレームで true を返す。 </returns>
    protected bool Get_IsAttackStart()
    {
        return _beforeIsCoolTimerNow == false && _isCoolTimerNow == true;
    }
    /// <summary> 攻撃終了のフレームを検知する </summary>
    /// <returns> 攻撃終了のフレームで true を返す。 </returns>
    protected bool Get_IsAttackEnd()
    {
        return _beforeIsCoolTimerNow == true && _isCoolTimerNow == false;
    }

    //<============= privateメンバー関数 =============>//
    /// <summary> 攻撃開始を検知して処理を実行する。 </summary>
    void Update_StartAttackProcess()
    {
        if (Get_IsAttackStart())
        {
            StartAttackProcess();
        }
    }
    /// <summary> 攻撃終了を検知して処理を実行する。 </summary>
    void Update_EndAttackProcess()
    {
        if (Get_IsAttackEnd())
        {
            EndAttackProcess();
        }
    }

    //<============= コルーチン =============>//
    /// <summary> クールタイムを開始する。 : 指定された時間クールタイム変数を true にする。 </summary>
   　protected IEnumerator StartCoolTime()
    {
        _isCoolTimerNow = true;
        yield return new WaitForSeconds(_coolTimeValue);
        _isCoolTimerNow = false;
    }


    //<============= 仮想関数 =============>//
    /// <summary> 攻撃開始処理 : オーバーライド推奨 </summary>
    protected virtual void StartAttackProcess()
    {
        // ここに、オーバーライド先でアニメーションの遷移処理等の、攻撃開始に関わる処理を記述してください。
    }
    /// <summary> 攻撃終了処理 : オーバーライド推奨 </summary>
    protected virtual void EndAttackProcess()
    {
        // ここに、オーバーライド先でアニメーションの遷移処理や、クールタイム開始処理等の、攻撃終了に関わる処理を記述してください。
    }
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
