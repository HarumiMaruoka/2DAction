using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerWeaponBase : MonoBehaviour
{
    [Header("���̕���̍U����"), SerializeField] float _offensivePower;
    public float OffensivePower { get => _offensivePower; }

    protected Rigidbody2D _rigidBody2D;
     
    protected virtual void WeaponInit()
    {
        _rigidBody2D = GetComponent<Rigidbody2D>();
    }

    /// <summary> �K�v�ł���Έړ�����(�I�[�o�[���C�h��) </summary>
    public virtual void Move() { }

    /// <summary> �U���J�n�̏���(�I�[�o�[���C�h��) </summary>
    public virtual void On_Attack()
    {
        gameObject.SetActive(true);
    }

    /// <summary> �U���I���̏���(�I�[�o�[���C�h��) </summary>
    public virtual void Off_Attack()
    {
        gameObject.SetActive(false);
    }
}
