using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 武器の基底クラス
/// </summary>
public abstract class WeaponBase : MonoBehaviour
{
    [Header("敵と接触したときに鳴らす音"), SerializeField] AudioClip clip;
    /// <summary> Enemy接触時の処理 : オーバーライド可 </summary>
    protected abstract void OnHitEnemy(EnemyBase enemy);
    /// <summary> 武器接触時の処理 : オーバーライド可 </summary>
    /// <param name="collision"> 接触対象 </param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyBase enemy))
        {
            // 敵接触時の処理
            OnHitEnemy(enemy);
            // 敵に当たったら音を鳴らす。
            if (clip != null) AudioSource.PlayClipAtPoint(clip, transform.position);
        }
    }
}
