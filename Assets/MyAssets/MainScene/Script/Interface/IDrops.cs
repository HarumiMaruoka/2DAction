using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 何かを落とすモノが持つべきインターフェース
/// </summary>
interface IDrops
{
    /// <summary>
    /// <para>
    /// 落とすもののIDを設定する。<br/>
    /// 継承先特有のIDを設定する。<br/></para>
    /// アイテムならItemID<br/>
    /// 装備ならEquipmentID<br/>
    /// </summary>
    public void SetID(int id);
}

