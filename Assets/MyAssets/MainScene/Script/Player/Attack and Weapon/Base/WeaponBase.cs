using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// •Ší‚ÌŠî’êƒNƒ‰ƒX
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    /// <summary> EnemyÚG‚Ìˆ— </summary>
    protected abstract void OnHitEnemy(EnemyBase enemy);
    /// <summary> •ŠíÚG‚Ìˆ— </summary>
    /// <param name="collision"> ÚG‘ÎÛ </param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EnemyBase enemy))
        {
            OnHitEnemy(enemy);
        }
    }
}
