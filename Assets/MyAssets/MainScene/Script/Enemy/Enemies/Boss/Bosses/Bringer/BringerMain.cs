using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerMain : BossBase
{
    //各パラメータ
    [Tooltip("最初ののクールタイム。xはMinValue、yはMaxValue。"), SerializeField] Vector2 _firstRandomAttackCoolTime = default;

    [Tooltip("弱攻撃後のクールタイム。xはMinValue、yはMaxValue。"), SerializeField] Vector2 _lightAttackCoolTime = default;
    [Tooltip("強攻撃後のクールタイム。xはMinValue、yはMaxValue。"), SerializeField] Vector2 _heavyAttackCoolTime = default;
    [Tooltip("遠距離攻撃後のクールタイム。xはMinValue、yはMaxValue。"), SerializeField] Vector2 _longRangeAttackCoolTime = default;

    [Tooltip("接近スピード"), SerializeField] float _approachSpeed;
    [Tooltip("後退スピード"), SerializeField] float _recessionSpeed;

    [Header("各Buttonを子に持つキャンバス"), SerializeField] GameObject _bottonCanvas;

    Transform _heavyAttack;
    Transform _lightAttack;

    bool _isLRAStart = false;

    void Start()
    {
        base.InitBoss();
        _heavyAttack = transform.GetChild(1);
        _lightAttack = transform.GetChild(2);
    }

    void Update()
    {
        base.CommonUpdateBoss();
    }

    /// <summary> Bringer独自の、UpdateBoss関数 </summary>
    protected override void UpdateBoss()
    {
        //体力がなくなった時の処理
        if (_hitPoint <= 0)
        {
            //体力がなくなったら消滅する
            _nowState = BossState.DIE;
            //Button群をアクティブにする
            _bottonCanvas.SetActive(true);
        }

        //色を変える必要があれば変える
        if (_isColorChange)
        {
            _spriteRenderer.color = Color.gray;
            _isColorChange = false;
        }
        //色を元に戻す
        else if (_colorChangeTimeValue < 0)
        {
            _spriteRenderer.color = new Color(1, 1, 1, 1);
        }
        //クールタイム解消
        else if (_colorChangeTimeValue > 0)
        {
            _colorChangeTimeValue -= Time.deltaTime;
        }

        _rigidBody2d.velocity = Vector2.zero;
        //Bossのステートを変更する。
        switch (_nowState)
        {
            case BossState.IDLE: Idle(); break;
            case BossState.APPROACH: Approach(); break;
            case BossState.RECESSION: Recession(); break;

            case BossState.LIGHT_ATTACK: LightAttack(); break;
            case BossState.HEAVY_ATTACK: HeavyAttack(); break;
            case BossState.LONG_RANGE_ATTACK: LongRangeAttack(); break;

            case BossState.DIE: Die(); break;
            //再抽選するコードをここに書く
            //ifでNomalかAttackに分岐し、処理を変える
            default: Debug.Log("このボスのステートはありません。"); break;
        }
    }

    //プレイヤーとBossが、ある程度近づいたら呼ばれる。
    protected override void BattleStart()
    {
        if (base.IsBattleStart())
        {
            Debug.Log("start");
            //ここにBringer独自の、戦闘開始時の処理を書く。
            //Debug.Log("BringerBattleStart!");
            //最初のクールタイムを設定
            _coolTimeValue = UnityEngine.Random.Range(_firstRandomAttackCoolTime.x, _firstRandomAttackCoolTime.y);
            _isCoolTimerStart = true;
        }
    }

    /// <summary> プレイヤーとBossが、離れすぎたら呼ばれる。 </summary>
    protected override void BattleExit()
    {
        if (base.IsBattleExit())
        {
            Debug.Log("exit");
            //ここにBringer独自の、戦闘終了時の処理を書く。
            //Debug.Log("BringerBattleExit...");
        }
    }

    /// <summary> BossがIdle中の処理 </summary>
    void Idle()
    {
        //クールタイム開始の処理
        if (_isCoolTimerStart)
        {
            _rigidBody2d.velocity = new Vector2(0f, _rigidBody2d.velocity.y);
            //Debug.Log("IdleStart");
            _isCoolTimeExit = false;
            _isAttackExit = false;

            StartCoroutine(CoolTime());
        }
        //Idle中の処理
        else
        {
            //Debug.Log("Idle中");
        }
        //アタックに移行する時の処理
        if (_isCoolTimeExit)
        {
            _isCoolTimeExit = false;
            //Debug.Log("IdleExit");
            StateChangeAttack();
        }
    }

    /// <summary> BossがApproach中の処理 </summary>
    void Approach()
    {
        //クールタイム開始の処理
        if (_isCoolTimerStart)
        {
            //Debug.Log("ApproachStart");
            _isCoolTimeExit = false;
            _isAttackExit = false;
            StartCoroutine(CoolTime());//クールタイム開始
        }
        //Approach中の処理
        else
        {
            //Debug.Log("前進中");
            //Approach中はプレイヤーに向かって前進する
            _rigidBody2d.velocity = (_playerPos.position.x - transform.position.x) < 0 ?
            new Vector2(-_approachSpeed, _rigidBody2d.velocity.y) :
            new Vector2(_approachSpeed, _rigidBody2d.velocity.y);

            //Debug.Log("プレイヤーに向かって前進中");
        }
        //アタックに移行する時の処理
        if (_isCoolTimeExit)
        {
            _isCoolTimeExit = false;
            //Debug.Log("ApproachExit");
            StateChangeAttack();
        }
    }

    /// <summary> BossがRecession中の処理 </summary>
    void Recession()
    {
        //Debug.Log("RecessionStart");
        //クールタイム開始の処理
        if (_isCoolTimerStart)
        {
            _isCoolTimeExit = false;
            _isAttackExit = false;
            StartCoroutine(CoolTime());//クールタイム開始
        }
        //Approach中の処理
        else
        {
            //Debug.Log("後進中");
            //Approach中はプレイヤーに向かって前進する
            _rigidBody2d.velocity = (_playerPos.position.x - transform.position.x) < 0 ?
            new Vector2(_recessionSpeed, _rigidBody2d.velocity.y) :
            new Vector2(-_recessionSpeed, _rigidBody2d.velocity.y);

            //Debug.Log("プレイヤーに向かって前進中");
        }
        //アタックに移行する時の処理
        if (_isCoolTimeExit)
        {
            _isCoolTimeExit = false;
            //Debug.Log("RecessionExit");
            StateChangeAttack();
        }
    }

    /// <summary> BossがLightAttack中の処理 </summary>
    void LightAttack()
    {
        //弱攻撃の処理
        if (_isAttackStart)
        {

        }
        //攻撃終了時の処理
        if (_isAttackExit)
        {
            //クールタイム設定し、ステートを変更する。
            _coolTimeValue = UnityEngine.Random.Range(_lightAttackCoolTime.x, _lightAttackCoolTime.y);
            StateChangeNomal();

            //Debug.Log("弱攻撃終了");
        }
    }

    /// <summary> BossがHeavyAttack中の処理 </summary>
    void HeavyAttack()
    {
        //強攻撃の処理
        if (_isAttackStart)
        {

        }
        //攻撃終了時の処理
        if (_isAttackExit)
        {
            //クールタイム設定し、ステートを変更する。
            _coolTimeValue = UnityEngine.Random.Range(_heavyAttackCoolTime.x, _heavyAttackCoolTime.y);
            StateChangeNomal();

            //Debug.Log("強攻撃終了");
        }
    }

    /// <summary> BossがLongRangeAttack中の処理 </summary>
    void LongRangeAttack()
    {
        //遠距離攻撃の処理
        if (_isAttackStart)
        {
            if (_isLRAStart)
            {
                _isLRAStart = false;
                //Spellをアクティブにし、PlaySpellを実行
                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(0).GetComponent<SpellController>().PlaySpell();
            }
        }
        //攻撃終了時の処理
        if (_isAttackExit)
        {
            //クールタイム設定し、ステートを変更する。
            _coolTimeValue = UnityEngine.Random.Range(_longRangeAttackCoolTime.x, _longRangeAttackCoolTime.y);
            StateChangeNomal();

            //Debug.Log("遠距離攻撃終了");
        }
    }

    /// <summary> ステートをアタックに移行 </summary>
    void StateChangeAttack()
    {
        _isAttackStart = true;
        _nowState = (BossState)UnityEngine.Random.Range((int)BossState.LIGHT_ATTACK, (int)BossState.ATTACK_END);
        if (_nowState == BossState.LONG_RANGE_ATTACK)
        {
            _isLRAStart = true;
        }
    }

    /// <summary> ステートをノーマルに移行 </summary>
    void StateChangeNomal()
    {

        int random = UnityEngine.Random.Range(0, 10);
        //10分の2でボスステートをIdleに移行
        if (random < 2)
        {
            _nowState = BossState.IDLE;
        }
        //10分の6でボスステートをApproachに移行
        else if (random < 9)
        {
            _nowState = BossState.APPROACH;
        }
        //10分の2でボスステートをRecessionに移行
        else
        {
            _nowState = BossState.RECESSION;
        }
    }

    //Bringerが倒されたときの処理
    void Die()
    {
        //Bossが死んだ時の処理をここに書く
        Destroy(gameObject, 1.0f);
    }

    //(アタック後の)クールタイムを待つ
    IEnumerator CoolTime()
    {
        _isCoolTimerStart = false;
        yield return new WaitForSeconds(_coolTimeValue);
        _isCoolTimeExit = true;
    }

    ///<summary> アタックモードの終了時に呼ばれる。アニメーションイベントから呼び出す。各フラグの初期化処理を行う。 </summary>
    public void AttackStateExit()
    {
        //Debug.Log("AttackExit");
        _isAttackStart = false;
        _isAttackExit = true;
        _isCoolTimerStart = true;
    }

    //アタック関連のメソッド。アニメーションイベントから呼び出す。
    public void OnHeavyAttack()
    {
        _heavyAttack.gameObject.SetActive(true);
    }
    public void OffHeavyAttack()
    {
        _heavyAttack.gameObject.SetActive(false);
    }
    public void OnLightAttack()
    {
        _lightAttack.gameObject.SetActive(true);
    }
    public void OffLightAttack()
    {
        _lightAttack.gameObject.SetActive(false);
    }
}
