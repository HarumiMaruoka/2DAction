using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerMain : BossBase
{
    //各パラメータ
    [Tooltip("最初ののクールタイム。xはMinValue、yはMaxValue。"), SerializeField] Vector2 _firstRandomAttackCoolTime = default;

    [Tooltip("弱攻撃後のクールタイム。xはMinValue、yはMaxValue。"), SerializeField] Vector2 _lightAttackCoolTime = default;
    [Tooltip("弱攻撃後のクールタイム。xはMinValue、yはMaxValue。"), SerializeField] Vector2 _heavyAttackCoolTime = default;
    [Tooltip("弱攻撃後のクールタイム。xはMinValue、yはMaxValue。"), SerializeField] Vector2 _longRangeAttackCoolTime = default;

    [Tooltip("接近スピード"), SerializeField] float _approachSpeed;
    [Tooltip("後退スピード"), SerializeField] float _recessionSpeed;



    void Start()
    {
        base.InitBoss();
    }

    void Update()
    {
        base.CommonUpdateBoss();
    }

    /// <summary> Bringer独自の、ChangeState関数 </summary>
    protected override void CangeState()
    {
        //クールタイムが残っている場合、攻撃以外の行動を取る

        //クールタイムが解消されたら、攻撃する
    }

    /// <summary> Bringer独自の、UpdateBoss関数 </summary>
    protected override void UpdateBoss()
    {
        switch (_nowState)
        {
            case BossState.IDLE: Idle(); break;
            case BossState.APPROACH: Approach(); break;
            case BossState.RECESSION: Recession(); break;

            case BossState.LIGHT_ATTACK: LightAttack(); break;
            case BossState.HEAVY_ATTACK: HeavyAttack(); break;
            case BossState.LONG_RANGE_ATTACK: LongRangeAttack(); break;

            //再抽選するコードをここに書く
            //ifでNomalかAttackに分岐し、処理を変える
            default: Debug.Log("このボスのステートはありません。"); break;
        }
    }

    protected override void BattleStart()
    {
        if (base.CommonBattleStart())
        {
            //ここにBringer独自の、戦闘開始時の処理を書く。
            Debug.Log("BringerBattleStart!");
            //最初のクールタイムを設定
            _coolTimeValue = UnityEngine.Random.Range(_firstRandomAttackCoolTime.x, _firstRandomAttackCoolTime.y);
            _isCoolTimerStart = true;
        }
    }

    protected override void BattleExit()
    {
        if (base.CommonBattleExit())
        {
            //ここにBringer独自の、戦闘終了時の処理を書く。
            Debug.Log("BringerBattleExit...");
        }
    }

    /// <summary> BossがIdle中の処理 </summary>
    void Idle()
    {
        //クールタイム残り時間の処理
        if (_isCoolTimerStart)
        {
            Debug.Log("IdleTimerStart");
            StartCoroutine(CoolTime());//クールタイム開始
            //Idle中は特に何もしない
        }
        //Idle中の処理
        else
        {
            //Debug.Log("Idle中");
        }
        //クールタイム解消時にアタックに移行
        if (_isCoolTimeExit)
        {
            _isCoolTimeExit = false;
            Debug.Log("IdleExit");
            StateChangeAttack();
        }
    }

    /// <summary> BossがApproach中の処理 </summary>
    void Approach()
    {
        //クールタイム残り時間の処理
        if (_isCoolTimerStart)
        {
            Debug.Log("ApproachTimerStart");
            StartCoroutine(CoolTime());//クールタイム開始
        }
        //Approach中の処理
        else
        {
            //Approach中はプレイヤーに向かって前進する
            _rigidBody2d.velocity = (_playerPos.position.x - transform.position.x) < 0 ?
            new Vector2(_approachSpeed * Time.deltaTime, _rigidBody2d.velocity.y) :
            new Vector2(-_approachSpeed * Time.deltaTime, _rigidBody2d.velocity.y);

            //Debug.Log("プレイヤーに向かって前進中");
        }
        //クールタイム解消時にアタックに移行
        if (_isCoolTimeExit)
        {
            _isCoolTimeExit = false;
            Debug.Log("ApproachExit");
            StateChangeAttack();
        }
    }

    /// <summary> BossがRecession中の処理 </summary>
    void Recession()
    {
        //クールタイム残り時間の処理
        if (_isCoolTimerStart)
        {
            Debug.Log("RecessionTimerStart");
            StartCoroutine(CoolTime());//クールタイム開始
        }
        //Approach中の処理
        else
        {
            //Approach中はプレイヤーに向かって前進する
            _rigidBody2d.velocity = (_playerPos.position.x - transform.position.x) < 0 ?
            new Vector2(-_recessionSpeed * Time.deltaTime, _rigidBody2d.velocity.y) :
            new Vector2(_recessionSpeed * Time.deltaTime, _rigidBody2d.velocity.y);

            //Debug.Log("プレイヤーに向かって前進中");
        }
        //クールタイム解消時にアタックに移行
        if (_isCoolTimeExit)
        {
            _isCoolTimeExit = false;
            Debug.Log("RecessionExit");
            StateChangeAttack();
        }
    }

    /// <summary> BossがLightAttack中の処理 </summary>
    void LightAttack()
    {
        if (_isAttackStart)
        {
            //LightAttackは、近距離:弱攻撃
            //ここに弱攻撃中のコードを書く

            //テスト用コード
            AttackStateExit();
        }
        if (_isAttackExit)
        {
            //クールタイム設定
            _coolTimeValue = UnityEngine.Random.Range(_lightAttackCoolTime.x, _lightAttackCoolTime.y);
            Debug.Log("弱攻撃終了");
            //ChangeState Nomal
            StateChangeNomal();
        }
    }

    /// <summary> BossがHeavyAttack中の処理 </summary>
    void HeavyAttack()
    {
        if (_isAttackStart)
        {
            //HeavyAttackは、近距離:強攻撃
            //ここに強攻撃中のコードを書く

            //テスト用コード
            AttackStateExit();
        }
        if (_isAttackExit)
        {
            //クールタイム設定
            _coolTimeValue = UnityEngine.Random.Range(_heavyAttackCoolTime.x, _heavyAttackCoolTime.y);
            Debug.Log("強攻撃終了");
            //ChangeState Nomal
            StateChangeNomal();
        }
    }

    /// <summary> BossがLongRangeAttack中の処理 </summary>
    void LongRangeAttack()
    {
        if (_isAttackStart)
        {
            //LongRangeAttackは遠距離攻撃
            //ここに遠距離攻撃中のコードを書く

            //テスト用コード
            AttackStateExit();
        }
        if (_isAttackExit)
        {
            //クールタイム設定
            _coolTimeValue = UnityEngine.Random.Range(_longRangeAttackCoolTime.x, _longRangeAttackCoolTime.y);
            Debug.Log("遠距離攻撃終了");
            //ChangeState Nomal
            StateChangeNomal();
        }
    }

    /// <summary> ステートをアタックに移行 </summary>
    void StateChangeAttack()
    {
        _isAttackStart = true;
        //本物のコード
        _nowState = (BossState)UnityEngine.Random.Range((int)BossState.LIGHT_ATTACK, (int)BossState.ATTACK_END);
    }

    /// <summary> ステートをノーマルに移行 </summary>
    void StateChangeNomal()
    {
        //本物のコード
        _nowState = (BossState)UnityEngine.Random.Range((int)BossState.IDLE, (int)BossState.NOMAL_END);
    }

    IEnumerator CoolTime()
    {
        _isCoolTimerStart = false;
        _isCoolTimeExit = false;
        yield return new WaitForSeconds(_coolTimeValue);
        _isCoolTimeExit = true;
    }

    ///<summary> アタックモードの終了時に呼ばれる。アニメーションイベントから呼び出す。 </summary>
    public void AttackStateExit()
    {
        Debug.Log("AttackExit");
        _isAttackStart = false;
        _isAttackExit = true;
        _isCoolTimerStart = true;
    }
}
