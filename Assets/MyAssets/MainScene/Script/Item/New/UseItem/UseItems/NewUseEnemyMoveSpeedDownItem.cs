using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテムID06用に作成したコンポーネント。<br/>
/// 敵の移動速度を一定時間減らす。<br/>
/// </summary>
public class NewUseEnemyMoveSpeedDownItem : UseItemBehavior
{
    protected override void OnEffect()
    {
        // 敵の移動速度を減らす
        UseItemManager.Instance.EnemyMoveSpeedDownValueChange(_effectValue);
    }

    protected override void OffEffect()
    {
        // 敵の移動速度を増やす
        UseItemManager.Instance.EnemyMoveSpeedDownValueChange(-_effectValue);
    }
}
