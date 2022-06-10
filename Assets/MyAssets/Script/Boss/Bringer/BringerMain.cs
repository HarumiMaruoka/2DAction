using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerMain : BossBase
{
    //各パラメータ
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
        StartCoroutine(CoolTime());//クールタイム開始
        //クールタイムが残っているときの処理
        if (_isCoolTimeNow)
        {
            //Idle中は特に何もしない
            Debug.Log("Idle中");
        }
        //クールタイム解消時にアタックに移行
        else
        {
            Debug.Log("Idle");
            StateChangeAttack();
        }
    }

    /// <summary> BossがApproach中の処理 </summary>
    void Approach()
    {
        StartCoroutine(CoolTime());//クールタイム開始
        //クールタイムが残っているときの処理
        if (_isCoolTimeNow)
        {
            //Approach中はプレイヤーに向かって前進する
            _rigidBody2d.velocity = (_playerPos.position.x - transform.position.x) < 0 ?
            new Vector2(_approachSpeed * Time.deltaTime, _rigidBody2d.velocity.y) :
            new Vector2(-_approachSpeed * Time.deltaTime, _rigidBody2d.velocity.y);

            Debug.Log("プレイヤーに向かって前進中");
        }
        //クールタイム解消時にアタックに移行
        else
        {
            Debug.Log("Approach");
            StateChangeAttack();
        }
    }

    /// <summary> BossがRecession中の処理 </summary>
    void Recession()
    {
        StartCoroutine(CoolTime());//クールタイム開始
        //クールタイムが残っているときの処理
        if (_isCoolTimeNow)
        {
            //Recession中はプレイヤーに対して後退する
            _rigidBody2d.velocity = (_playerPos.position.x - transform.position.x) < 0 ?
                new Vector2(-_recessionSpeed * Time.deltaTime, _rigidBody2d.velocity.y) :
                new Vector2(_recessionSpeed * Time.deltaTime, _rigidBody2d.velocity.y);

            Debug.Log("プレイヤーに対して後退中");
        }
        //クールタイム解消時にアタックに移行
        else
        {
            Debug.Log("Recession");
            StateChangeAttack();
        }
    }

    /// <summary> BossがLightAttack中の処理 </summary>
    void LightAttack()
    {
        //LightAttackは、近距離:弱攻撃
        Debug.Log("弱攻撃攻撃発動");
        //クールタイム設定
        _coolTimeValue = UnityEngine.Random.Range(_lightAttackCoolTime.x, _lightAttackCoolTime.y);
        //ChangeState Nomal
        StateChangeNomal();
    }

    /// <summary> BossがHeavyAttack中の処理 </summary>
    void HeavyAttack()
    {
        //HeavyAttackは、近距離:強攻撃
        Debug.Log("強攻撃発動");
        //クールタイム設定
        _coolTimeValue = UnityEngine.Random.Range(_heavyAttackCoolTime.x, _heavyAttackCoolTime.y);
        //ChangeState Nomal
        StateChangeNomal();
    }

    /// <summary> BossがLongRangeAttack中の処理 </summary>
    void LongRangeAttack()
    {
        //LongRangeAttackは遠距離攻撃
        Debug.Log("遠距離攻撃発動");
        //クールタイム設定
        _coolTimeValue = UnityEngine.Random.Range(_longRangeAttackCoolTime.x, _longRangeAttackCoolTime.y);
        //ChangeState Nomal
        StateChangeNomal();
    }

    /// <summary> ステートをアタックに移行 </summary>
    void StateChangeAttack()
    {
        //本物のコード
        //_nowState = (BossState)UnityEngine.Random.Range((int)BossState.LIGHT_ATTACK, (int)BossState.ATTACK_END);

        //テスト用コード
        _nowState = BossState.LIGHT_ATTACK;
    }

    /// <summary> ステートをノーマルに移行 </summary>
    void StateChangeNomal()
    {
        _nowState = (BossState)UnityEngine.Random.Range((int)BossState.IDLE, (int)BossState.NOMAL_END);
    }

    IEnumerator CoolTime()
    {
        _isCoolTimeNow = true;
        yield return new WaitForSeconds(_coolTimeValue);
        _isCoolTimeNow = false;
    }
}
