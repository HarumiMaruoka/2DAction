using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 基本斬撃攻撃クラス </summary>
public class NomalSlashingAttack : PlayerWeaponBase
{
    /// <summary> 基本斬撃攻撃クラス独自の初期化処理 </summary>
    protected override void WeaponInit()
    {
        // プレイヤーのトランスフォームを取得する。
        _playerPos = transform.parent;
        // 武器IDを設定する。
        _myWeaponID = (int)ArmParts.WeaponID.WeaponID_01_NomalSlashingAttack;
        //PressTypeを設定する。
        _isPressType = true;
    }

    private void Start()
    {
        
    }

    public override void Run_FireProcess()
    {
        UpdatePosition();
    }
}
