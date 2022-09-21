using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// "Bringer"が放つ攻撃を制御するコンポーネント
/// </summary>
public class BringerWeaponController : EnemyWeaponBase
{
    #region Field and Property
    [Header("遠距離武器かどうか : 遠距離武器であればチェックをしてください。"), SerializeField]
    bool _isLongRangeWeapon = false;
    [Header("遠距離武器の場合の出現位置オフセット"), SerializeField]
    float _appearancePositionOffsetY = 3.1f;
    #endregion

    protected override void Start()
    {
        base.Start();
        // Spel(遠距離武器)の場合の処理
        if (_isLongRangeWeapon)
        {
            // プレイヤーの頭上に出現する。
            var pos = transform.position;
            pos =
            GameObject.FindGameObjectWithTag(Constants.PLAYER_TAG_NAME).transform.position +
            Vector3.up * _appearancePositionOffsetY;
            transform.position = pos;
        }

        // そうでない場合は何もしない。(生まれたままの位置にいる。)
    }
}