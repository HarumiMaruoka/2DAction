using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 近距離攻撃武器の基底クラス。
/// </summary>
public class ShortRangeWeaponBase : WeaponBase
{
    /// <summary> エネミーに対して攻撃がヒットした際の硬直時間 </summary>
    [Header("エネミーに対して攻撃がヒットした際の硬直時間"), SerializeField]
    protected float _basicRecoveryTime;
    /// <summary> エネミーに対して攻撃がヒットした際のノックバック力 </summary>
    [Header("エネミーに対して攻撃がヒットした際のノックバック力"), SerializeField]
    protected float _basicKnockbackForce;
    protected override void OnHitEnemy(EnemyBase enemy)
    {
        //プレイヤーの近距離攻撃力に応じて、対象にダメージを与える。
        //又、プレイヤーの重量に応じて対象をノックバックさせ、硬直させる。
        enemy.HitPlayerAttack
            (
            PlayerStatusManager.Instance.ConsequentialPlayerStatus._shortRangeAttackPower,
            _basicRecoveryTime,
            _basicKnockbackForce
            );
    }

    /// <summary> 初期化処理 </summary>
    /// <returns> 初期化に成功したら true ,そうでなければ false を返す。</returns>
    protected virtual bool Initialized()
    {
        return true;
    }
}
