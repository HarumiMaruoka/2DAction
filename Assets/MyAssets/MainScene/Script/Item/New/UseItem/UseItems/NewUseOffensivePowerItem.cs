using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// アイテム使用による、<br/>
/// 攻撃力上昇を表す為のコンポーネント <br/>
/// ItemID : 03, 04, 05用。<br/>
/// </summary>
public class NewUseOffensivePowerItem : UseItemBehavior
{
    protected override void OnEffect()
    {
        // 攻撃力を上昇させる。
        UseItemManager.Instance.OffensivePowerUpValueChange((int)_effectValue);
    }
    protected override void OffEffect()
    {
        // 攻撃力を減衰させる。
        UseItemManager.Instance.OffensivePowerUpValueChange(-(int)_effectValue);
    }
}
