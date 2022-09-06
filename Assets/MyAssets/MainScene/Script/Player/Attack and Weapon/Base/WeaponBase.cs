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
}
