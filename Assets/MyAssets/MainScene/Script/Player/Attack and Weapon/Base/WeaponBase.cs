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
}
