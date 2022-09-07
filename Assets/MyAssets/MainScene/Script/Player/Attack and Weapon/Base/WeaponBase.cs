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
    /// <summary> ����ڐG���̏��� </summary>
    /// <param name="collision"> �ڐG�Ώ� </param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out EnemyBase enemy))
        {
            OnHitEnemy(enemy);
        }
    }
}
