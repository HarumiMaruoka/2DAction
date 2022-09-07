using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// BasicShortRangeAttackが放つ斬撃
/// </summary>
public class BasicAufschlitzen : ShortRangeWeaponBase
{
    /// <summary> この攻撃によるダメージ増加量 </summary>
    [Header("この攻撃によるダメージ増加量"), SerializeField] float _damageIncrease = 0f;

    protected override void OnHitEnemy(EnemyBase enemy)
    {
        //プレイヤーの近距離攻撃力に応じて、対象にダメージを与える。
        //又、プレイヤーの重量に応じて対象をノックバックさせ、硬直させる。
        enemy.HitPlayerAttack
            (
            PlayerStatusManager.Instance.ConsequentialPlayerStatus._shortRangeAttackPower + _damageIncrease,
            _basicRecoveryTime,
            _basicKnockbackForce
            );
    }
}
