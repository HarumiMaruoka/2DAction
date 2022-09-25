using System;
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
    //===== フィールド / プロパティ =====//
    /// <summary> 戦闘状態かどうかを表す変数。 </summary>
    [Header("確認用 : 戦闘中かどうか"), SerializeField]
    bool _isFight = false;

    //クールタイム関連
    [Header("戦闘を開始して最初の攻撃までのクールタイム"), SerializeField]
    protected RandomRangeValue _cooltimeFirstAttack;
    /// <summary> 今フレームでクールタイム中かどうかを表す変数。 </summary>
    protected bool _isCoolTimerNow = false;
    /// <summary> 前フレームでクールタイム中だったかどうかを表す変数。 </summary>
    protected bool _beforeIsCoolTimerNow = false;
    /// <summary> クールタイム時間 </summary>
    [Header("確認用 : 現在のクールタイム"), SerializeField]
    protected float _coolTimeValue = 0f;
    /// <summary> 現在攻撃中かどうかを表す変数。 </summary>
    protected bool _isAttackNow = false;
    /// <summary> 前フレームで攻撃中だったかどうかを表す変数。 </summary>
    protected bool _isBeforeAttack = false;

    [Header("先頭開始 /終了 の距離")]
    [Tooltip("戦闘開始までの距離"), SerializeField] private Vector2 _fightStartDistance;
    [Tooltip("戦闘停止までの距離"), SerializeField] private Vector2 _fightStopDistance;

    /// <summary> 現在のステート </summary>
    public BossState _nowState { get; protected set; }
    public BossState _beforeState { get; private set; }

    protected Animator _animator;

    protected float _holdAnimSpeed;

    protected IEnumerator _waitCoolTimeCoroutine;

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
    protected override void OnPause()
    {
        base.OnPause();
        if (_waitCoolTimeCoroutine != null)
        {
            StopCoroutine(_waitCoolTimeCoroutine);
        }
    }
    protected override void OnResume()
    {
        base.OnResume();
        if (_waitCoolTimeCoroutine != null)
        {
            StartCoroutine(_waitCoolTimeCoroutine);
        }
    }

    //===== protectedメソッド =====//
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
    protected void CommonUpdate_BossBase()
    {
        // プレイヤーがいる方向に向かって向きを変える
        if (_playerPos.position.x > transform.position.x && transform.localScale.x > 0)
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }
        else if (_playerPos.position.x < transform.position.x && transform.localScale.x < 0)
        {
            var localScale = transform.localScale;
            localScale.x *= -1;
            transform.localScale = localScale;
        }

        // 戦闘するかどうかを判断する。
        StartOrExitBattle();

        // 戦闘中の処理
        if (_isFight)
        {
            // 必要であれば攻撃開始処理を行う
            if (_beforeIsCoolTimerNow && !_isCoolTimerNow)
            {
                StartAttackProcess();
            }
            // 必要であれば攻撃終了処理を行う
            else if (!_isAttackNow && _isBeforeAttack)
            {
                EndAttackProcess();
            }

            // 現在のステート別に処理を行う
            ManageState();

            // 次のフレーム用に、クールタイムかどうかを判定する値を保存しておく。
            _beforeIsCoolTimerNow = _isCoolTimerNow;
        }

        // 戦闘状態でなければ何もしない。

        // 次フレーム用に現在のステートを保存しておく。
        _beforeState = _nowState;
        // 次フレーム用に現在攻撃中だったかどうか保存しておく。
        _isBeforeAttack = _isAttackNow;
    }
    /// <summary>
    /// プレイヤーとの距離が一定以上小さくなれば戦闘を開始し<br/>
    /// プレイヤーとの距離が一定以上離れたら戦闘を終了する。<br/>
    /// </summary>
    void StartOrExitBattle()
    {
        //プレイヤーとの距離を測る処理をここに書く
        Vector2 difference = _playerPos.position - transform.position;
        float diffX = Mathf.Abs(difference.x);
        float diffY = Mathf.Abs(difference.y);
        // 現在戦闘状態でなく、かつ
        // 距離が一定以上近づいたら、戦うかどうかを表すフィールドをtrueにする。(戦闘状態にする。)
        if (!_isFight && (diffX < _fightStartDistance.x && diffY < _fightStartDistance.y))
        {
            Debug.Log($"{gameObject.name}戦闘開始しました。");
            BattleStart();
        }
        // 現在非戦闘状態であり、かつ
        // 距離が一定以上離れたら、戦うかどうかを表すフィールドをfalseにする。(非戦闘状態にする。)
        else if (_isFight && (diffX > _fightStopDistance.x || diffY > _fightStopDistance.y))
        {
            Debug.Log($"{gameObject.name}戦闘終了しました。");
            BattleExit();
        }
    }
    /// <summary>
    /// 死んだ時の処理。(ステートがDieの時の処理。)<br/>
    /// ManageState()で使用することを想定したメソッド。<br/>
    /// 使用例はBringerのManageState()にあります。<br/>
    /// MomentOfDeath()とTreatmentAfterDeath()をオーバーライドして処理を記述してください。<br/>
    /// </summary>
    protected void TreatmentOfDeath()
    {
        // ステートがDieになったフレームのみ実行する。
        if (_beforeState != BossState.DIE && _nowState == BossState.DIE)
        {
            MomentOfDeath();
        }
        // それ以降の処理
        else if (_nowState == BossState.DIE)
        {
            TreatmentAfterDeath();
        }
    }


    //===== アニメーションイベントから呼び出す想定のメソッド =====//
    /// <summary> 
    /// このゲームオブジェクトを破棄する。 : <br/>
    /// このメソッドは、アニメーションイベントから呼び出す想定で作成したもの。<br/>
    /// </summary>
    protected void DestroyThisObject() { Destroy(gameObject); }
    /// <summary> 
    /// 攻撃開始処理。<br/>
    /// このメソッドはアニメーションイベントから呼び出す想定で作成したもの。 
    /// </summary>
    protected void AttackStart() { _isAttackNow = true; }
    /// <summary> 
    /// 攻撃終了処理。<br/>
    /// このメソッドはアニメーションイベントから呼び出す想定で作成したもの。 
    /// </summary>
    protected void AttackEnd() { _isAttackNow = false; }


    //===== コルーチン =====//
    /// <summary> クールタイムを開始する。 : 指定された時間クールタイム中だと表す変数を true にする。 </summary>
    protected IEnumerator WaitCoolTime()
    {
        float timer = 0f;
        _isCoolTimerNow = true;
        while (timer < _coolTimeValue)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        _isCoolTimerNow = false;
        _waitCoolTimeCoroutine = null;
    }


    //===== 仮想関数 =====//
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
    /// <summary>
    /// ステートがDieになった瞬間一度だけ実行するメソッド : オーバーライド推奨<br/>
    /// </summary>
    protected virtual void MomentOfDeath() { }
    /// <summary>
    /// ステートがDieになった後ずっと行われる処理 : オーバーライド推奨<br/>
    /// </summary>
    protected virtual void TreatmentAfterDeath() { }
    /// <summary>
    /// 戦闘開始を検知して戦闘中かどうかを表すフィールドを trueにする。<br/>
    /// 戦闘開始時に行う処理があればここに記述すること。:<br/>
    /// 基本的には、そのまま使用することを想定しているが<br/>
    /// もし独自の処理を行いたい場合には、オーバーライドしても良い。
    /// </summary>
    protected virtual void BattleStart()
    {
        //最初のクールタイムを待つ
        _coolTimeValue = UnityEngine.Random.Range(_cooltimeFirstAttack._minValue, _cooltimeFirstAttack._maxValue);
        _waitCoolTimeCoroutine = WaitCoolTime();
        StartCoroutine(_waitCoolTimeCoroutine);
        _isFight = true;
    }
    /// <summary>
    /// 戦闘終了を検知して戦闘中かどうかを表すフィールドを falseにする。    
    /// 戦闘終了時に行う処理があればここに記述すること。<br/>
    /// 基本的には、そのまま使用することを想定しているが<br/>
    /// もし独自の処理を行いたい場合には、オーバーライドしても良い。
    /// </summary>
    protected virtual void BattleExit()
    {
        // このコンポーネントから開始された全てのコルーチンを停止する。
        StopAllCoroutines();
        // 戦闘状態を表す変数をfalseにする。
        _isFight = false;
    }


    //===== overrides =====//
    /// <summary>
    /// 体力がなくなった時の処理 : <br/>
    /// ステートをDieに変更する<br/>
    /// オーバーライド可<br/>
    /// </summary>
    protected override void Deth()
    {
        _nowState = BossState.DIE;
    }
    static private void Info(int num)
    {
        Console.WriteLine(num);
    }
    static public System.Action<int> OnSelect;

    static void Main()
    {
        OnSelect += Info;
        OnSelect -= Info;
    }
}
/// <summary> ボスのステートを表す型 </summary>
[System.Serializable]
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
public struct RandomRangeValue
{
    /// <summary> ランダムな値の最小値 </summary>
    public float _minValue;
    /// <summary> ランダムな値の最大値 </summary>
    public float _maxValue;
}
