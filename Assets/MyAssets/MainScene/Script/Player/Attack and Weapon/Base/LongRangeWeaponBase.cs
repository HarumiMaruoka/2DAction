using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 遠距離攻撃武器の基底クラス。
/// </summary>
public class LongRangeWeaponBase : WeaponBase
{
    //<===== メンバー変数 =====>//


    //<===== 仮想関数 =====>//
    protected override void OnHitEnemy(EnemyBase enemy)
    {
        //プレイヤーの遠距離攻撃力に応じて、対象にダメージを与える。
        enemy.HitPlayerAttack(PlayerStatusManager.Instance.ConsequentialPlayerStatus._longRangeAttackPower);
        //ダメージを与えたら消滅する。
        Destroy(gameObject);
    }

    /// <summary> 初期化処理 </summary>
    /// <returns> 初期化に成功したら true ,そうでなければ false を返す。</returns>
    protected virtual bool Initialized()
    {
        return true;
    }

    /// <summary> 複雑な動きをする武器の場合、移動をこのメソッドに記載する。 </summary>
    protected virtual void Move()
    {

    }
}
