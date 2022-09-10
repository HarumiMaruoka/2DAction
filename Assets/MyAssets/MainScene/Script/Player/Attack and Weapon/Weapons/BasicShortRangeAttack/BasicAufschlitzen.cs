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

    void Start()
    {
        //方向を修正
        if (PlayerStatusManager.Instance.IsRight)
        {
            var l = transform.localScale;
            l.x *= -1;
            transform.localScale = l;

            var p = transform.localPosition;
            p.x *= -1;
            transform.localPosition = p;
        }
    }

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

    /// <summary>
    /// このオブジェクトを破棄する。<br/>
    /// アニメーションイベントから呼び出す。<br/>
    /// </summary>
    void DestroyThisObject()
    {
        Destroy(gameObject);
    }
}
