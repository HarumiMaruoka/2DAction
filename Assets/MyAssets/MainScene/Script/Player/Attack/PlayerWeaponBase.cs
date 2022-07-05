using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeaponBase : MonoBehaviour
{
    [Header("この武器の攻撃力"), SerializeField] float _offensivePower;
    public float OffensivePower { get => _offensivePower; }

    protected Rigidbody2D _rigidBody2D;
     
    protected virtual void WeaponInit()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    /// <summary> 必要であれば移動する(オーバーライド可) </summary>
    public virtual void Move() { }

    /// <summary> 攻撃開始の処理(オーバーライド可) </summary>
    public virtual void On_Attack()
    {
        gameObject.SetActive(true);
    }

    /// <summary> 攻撃終了の処理(オーバーライド可) </summary>
    public virtual void Off_Attack()
    {
        gameObject.SetActive(false);
    }
}
