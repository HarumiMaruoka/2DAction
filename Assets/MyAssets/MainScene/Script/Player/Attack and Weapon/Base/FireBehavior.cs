using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーが装備できる全ての武器の基底クラス。
/// </summary>
public abstract class FireBehavior : MonoBehaviour
{
    //<===== メンバー変数 =====>//
    /// <summary> 
    /// 入力タイプ : <br/>
    /// trueなら押下時のみ、falseなら押下中ずっと実行することを表す。 <br/>
    /// </summary>
    protected bool _pressType;

    //<===== protectedメンバー関数 =====>//
    /// <summary> 左腕に装備する処理。 </summary>
    public void SetEquip_LeftArm()
    {
        // 入力タイプに応じて、対応する方のデリゲート変数に登録する。
        // 反対側のデリゲート変数は破棄する。
        if (_pressType)
        {
            NewPlayerAttack._playerLeftArmWeapon_Moment = OnFire_ThisWeapon;
            NewPlayerAttack._playerLeftArmWeapon_Consecutively = null;
        }
        else
        {
            NewPlayerAttack._playerLeftArmWeapon_Moment = null;
            NewPlayerAttack._playerLeftArmWeapon_Consecutively = OnFire_ThisWeapon;
        }
    }
    /// <summary> 右腕に装備する処理。 </summary>
    public void SetEquip_RightArm()
    {
        if (_pressType)
        {
            NewPlayerAttack._playerRightArmWeapon_Moment = OnFire_ThisWeapon;
            NewPlayerAttack._playerRightArmWeapon_Consecutively = null;
        }
        else
        {
            NewPlayerAttack._playerRightArmWeapon_Moment = null;
            NewPlayerAttack._playerRightArmWeapon_Consecutively = OnFire_ThisWeapon;
        }
    }

    //<===== 仮想関数 =====>//
    /// <summary> 初期化処理 </summary>
    /// <param name="pressType"> 入力タイプ : <br/>
    /// trueなら押下時のみ、falseなら押下中ずっと実行することを表す。 <br/> </param>
    /// <returns> 初期化に成功したら true ,そうでない場合に false を返す。 </returns>
    protected virtual bool Initialized(bool pressType)
    {
        _pressType = pressType;
        return true;
    }
    /// <summary> 
    /// 攻撃処理 : <br/>
    /// PlayerAttackクラスのデリゲート変数に登録し、そこから呼び出す。<br/>
    /// </summary>
    protected virtual void OnFire_ThisWeapon() { }
}
