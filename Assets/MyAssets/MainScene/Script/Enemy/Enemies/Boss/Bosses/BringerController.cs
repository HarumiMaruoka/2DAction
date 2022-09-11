using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Boss, Bringer 用のコンポーネント : <br/>
/// BossBaseを継承する。
/// </summary>
public class BringerController : NewBossBase
{
    //<===== メンバー変数 =====>//
    [Header("各攻撃別に移行する確率 (最大100%)")]

    [Tooltip("弱攻撃を撃つ確率"), SerializeField]
    float _lightAttackProbability;
    [Tooltip("強攻撃を撃つ確率"), SerializeField]
    float _heavyAttackProbability;
    [Tooltip("遠距離攻撃を撃つ確率"), SerializeField]
    float _longRangeAttackProbability;

    [Header("各通常行動に移行する確率 (最大100%)")]

    [Tooltip("アイドルに移行する確率"), SerializeField]
    float _idleProbability;
    [Tooltip("接近行動に移行する確率"), SerializeField]
    float _approachProbability;
    [Tooltip("後退行動に移行する確率"), SerializeField]
    float _recessionProbability;

    //<===== Unityメッセージ =====>//
    void Start()
    {

    }
    protected override void Update()
    {
        BringerUpdate();
    }

    //<===== メンバー関数 =====>//
    /// <summary> Bringerの更新関数 </summary>
    void BringerUpdate()
    {
        switch (_nowState)
        {
            //通常行動処理
            case BossState.IDLE: Idle(); break;
            case BossState.APPROACH: Approach(); break;
            case BossState.RECESSION: Recession(); break;

            //攻撃処理
            case BossState.LIGHT_ATTACK_PATTERN_ONE: LightAttack(); break;
            case BossState.HEAVY_ATTACK_PATTERN_ONE: HeavyAttack(); break;
            case BossState.LONG_RANGE_ATTACK_PATTERN_ONE: LongRangeAttack(); break;

            //エラー値処理
            default: Debug.LogError("エラー値です。修正してください！"); break;
        }
    }
    /// <summary> アイドルの処理 </summary>
    void Idle()
    {

    }
    /// <summary> 接近の処理 </summary>
    void Approach()
    {

    }
    /// <summary> 後退の処理 </summary>
    void Recession()
    {

    }
    /// <summary> 弱攻撃の処理 </summary>
    void LightAttack()
    {

    }
    /// <summary> 強攻撃の処理 </summary>
    void HeavyAttack()
    {

    }
    /// <summary> 遠距離攻撃の処理 </summary>
    void LongRangeAttack()
    {

    }
    /// <summary> 攻撃ステートに移る。 </summary>
    BossState ChangeAttackState()
    {
        float probability = Random.Range(0f, 100f);
        //弱攻撃に移行するかどうかを判定する。
        if (probability < _lightAttackProbability)
        {
            return _nowState = BossState.LIGHT_ATTACK_PATTERN_ONE;
        }
        //強攻撃に移行するかどうかを判定する。
        else if (probability < _heavyAttackProbability + _lightAttackProbability)
        {
            return _nowState = BossState.HEAVY_ATTACK_PATTERN_ONE;
        }
        //遠距離攻撃に移行するかどうかを判定する。
        else
        {
            return _nowState = BossState.LONG_RANGE_ATTACK_PATTERN_ONE;
        }
    }
    /// <summary> 通常ステートに移る。 </summary>
    BossState ChangeNomalState()
    {
        float probability = Random.Range(0f, 100f);
        //アイドルに移行するか判定する。
        if (probability < _idleProbability)
        {
            return _nowState = BossState.IDLE;
        }
        //接近に移行するか判定する。
        else if (probability < _approachProbability + _idleProbability)
        {
            return _nowState = BossState.APPROACH;
        }
        //後退に移行するか判定する。
        else
        {
            return _nowState = BossState.RECESSION;
        }
    }

    //<===== コルーチン =====>//
    IEnumerator WaitAttackCoolTime(float waitTime)
    {
        // 攻撃後のクールタイムを待つ。
        yield return new WaitForSeconds(waitTime);
        // 攻撃ステートに移る。
        _nowState = ChangeAttackState();
    }

}
