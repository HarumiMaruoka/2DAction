using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// νΜξκNX
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    /// <summary> EnemyΪGΜ </summary>
    protected abstract void OnHitEnemy(EnemyBase enemy);
    /// <summary> νΪGΜ </summary>
    /// <param name="collision"> ΪGΞΫ </param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EnemyBase enemy))
        {
            OnHitEnemy(enemy);
        }
    }
}
