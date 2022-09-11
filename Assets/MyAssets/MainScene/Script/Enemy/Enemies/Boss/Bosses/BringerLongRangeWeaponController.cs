using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bringer の遠距離攻撃を制御するコンポーネント
/// </summary>
public class BringerLongRangeWeaponController : EnemyBase
{
    public Collider2D _collider { get; private set; }
    void Start()
    {
        _collider = GetComponent<Collider2D>();
    }
    /// <summary> このオブジェクトにアタッチされているコライダーをアクティブにする。 </summary>
    void ClliderOn()
    {
        _collider.enabled = true;
    }

}
