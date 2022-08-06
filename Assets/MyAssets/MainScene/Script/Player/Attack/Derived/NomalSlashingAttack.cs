using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 基本斬撃攻撃クラス </summary>
public class NomalSlashingAttack : PlayerWeaponBase
{
    /// <summary> アクティブになった時のプレイヤーの向き </summary>
    bool _onActivationPlayerDirection_isRight;

    /// <summary> 基本斬撃攻撃クラス独自の初期化処理 </summary>
    protected override void WeaponInit()
    {
        // GetComponent<>() 等の共通の処理を行う。
        base.WeaponInit();
        // 武器IDを設定する。
        _myWeaponID = (int)ArmParts.WeaponID.WeaponID_00_NomalShotAttack;
        //PressTypeを設定する。
        _isPressType = true;
    }

    private void Update()
    {
        if(_onActivationPlayerDirection_isRight == PlayerStatusManager.Instance.IsRight)
        {
            transform.position = transform.position + _positionOffsetAtBirth;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary> アクティブになった時の処理 </summary>
    private void OnEnable()
    {
        OnEnable_ThisWeapon();
        //アクティブになった時のプレイヤーの向きを保存しておく。
        _onActivationPlayerDirection_isRight = PlayerStatusManager.Instance.IsRight;
    }

    /// <summary> 非アクティブになった時の処理 </summary>
    private void OnDisable()
    {
        
    }
}
