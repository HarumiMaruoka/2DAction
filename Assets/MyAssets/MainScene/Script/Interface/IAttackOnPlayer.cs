using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// プレイヤーを攻撃するオブジェクトが継承すべきインターフェース。
/// </summary>
interface IAttackOnPlayer

{
    /// <summary> 
    /// プレイヤーに対する攻撃処理 :<br/>
    /// プレイヤーの体力を減らし、ノックバックさせる。<br/>
    /// </summary>
    /// <param name="playerRb2D"> プレイヤーのRigidBody2D </param>
    void HitPlayer(Rigidbody2D playerRb2D);
}
