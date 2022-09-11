using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 
/// プレイヤーを攻撃するオブジェクトが継承すべきインターフェース。
/// </summary>
interface AttackOnPlayer
{    
    /// <summary> プレイヤーに対する攻撃処理 </summary>
    void HitPlayer();
    void HitPlayer(Rigidbody2D playerRb);
}
