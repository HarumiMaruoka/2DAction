using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bossの基底クラス :
/// 新しい、BossBaseコンポーネント。
/// 今はまだ使用していない。
/// 
/// 変更点 : EnemyBaseを継承している点。
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
    /// <summary> 前のフレームで戦闘状態だったか？ : 戦闘中であれば true </summary>
    private bool _beforeFrameIsFight = false;
    /// <summary> 現在のフレームで戦闘状態か？ : 戦闘中であれば true </summary>
    private bool _isFight = false;

    //クールタイム関連
    /// <summary> 現在クールタイム中かどうか </summary>
    protected bool _isCoolTimerNow = false;
    /// <summary> 現在クールタイム中かどうかの前フレームの値 </summary>
    protected bool _beforeIsCoolTimerNow = false;
    /// <summary> クールタイム時間 </summary>
    protected float _coolTimeValue = 0f;

    //攻撃関連
    /// <summary> 攻撃を開始するか？ : 開始するフレームで true </summary>
    protected bool _isAttackStart = false;
    /// <summary> 攻撃を終了するか？ : 終了するフレームで true </summary>
    protected bool _isAttackExit = false;

    /// <summary> ボス攻撃後のクールタイム </summary>
    Dictionary<BossState, float> _bossAttackCoolTime = new Dictionary<BossState, float>();

    [Tooltip("戦闘開始までの距離"), SerializeField] private Vector2 _fightStartDistance;
    [Tooltip("戦闘停止までの距離"), SerializeField] private Vector2 _fightStopDistance;

    /// <summary> 現在のステート </summary>
    public BossState _nowState { get; protected set; }


    //<============= protectedメンバー関数 =============>//
    /// <summary> BossBaseの初期化関数 </summary>
    /// <returns> 成功したら true を返す。 </returns>
    protected bool Initialize_BossBase()
    {
        if (!base.Initialize_Enemy())
        {
            Debug.LogError($"初期化に失敗しました。{gameObject.name}");
            return false;
        }
        return true;
    }
    protected virtual void Update_BossBase()
    {

    }

    /// <summary> 攻撃開始のフレームを検知する </summary>
    /// <returns> 攻撃開始のフレームで true を返す。 </returns>
    protected bool StartAttack()
    {
        return _beforeIsCoolTimerNow == false && _isCoolTimerNow == true;
    }
    /// <summary> 攻撃終了のフレームを検知する </summary>
    /// <returns> 攻撃終了のフレームで true を返す。 </returns>
    protected bool EndAttack()
    {
        return _beforeIsCoolTimerNow == true && _isCoolTimerNow == false;
    }

    //<============= privateメンバー関数 =============>//



    //<============= コルーチン =============>//
    IEnumerator CoolTime_Coroutine()
    {
        _isCoolTimerNow = true;
        yield return new WaitForSeconds(_coolTimeValue);
        _isCoolTimerNow = false;
    }


    //<============= 仮想関数 =============>//
    /// <summary> 攻撃開始処理 : オーバーライド可 </summary>
    protected virtual void StartAttackProcess() { }
    /// <summary> 攻撃終了処理 : オーバーライド可 </summary>
    protected virtual void EndAttackProcess() { }
}
