using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bringer の弱攻撃と強攻撃を制御するコンポーネント
/// </summary>
public class BringerLightAndHeavyWeaponController : EnemyBase
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
