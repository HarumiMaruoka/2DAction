using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����̊��N���X
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    /// <summary> Enemy�ڐG���̏��� </summary>
    protected abstract void OnHitEnemy(EnemyBase enemy);
}
