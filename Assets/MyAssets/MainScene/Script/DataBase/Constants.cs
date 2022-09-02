using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 定数を集めたクラス
/// </summary>
public class Constants
{
    /// <summary> 左腕を表す定数。 </summary>
    public const int LEFT_ARM = 0;
    /// <summary> 右腕を表す定数。 </summary>
    public const int RIGHT_ATM = 1;
    /// <summary> 腕以外を表す定数。 </summary>
    public const int NOT_ARM = -1;
    /// <summary> 変化幅を描画する </summary>
    public const bool DRAW_AMPLITUDE = true;
    /// <summary> 変化幅を表示しない </summary>
    public const bool NOT_DRAW_AMPLITUDE = false;

    //装備関係のクラスで使用する定数
    public const int PLAYER_NAME_DRAW_AREA = 0;
    public const int EQUIPMENT_TYPE_DRAW_AREA = 0;
    public const int MAX_HP_DRAW_AREA = 1;
    public const int MAX_STAMINA_TYPE_DRAW_AREA = 2;
    public const int SHORT_RANGE_ATTACK_POWER_DRAW_AREA = 3;
    public const int LONG_RANGE_ATTACK_POWER_DRAW_AREA = 4;
    public const int DEFENSE_POWER_DRAW_AREA = 5;
    public const int MOVE_SPEED_DRAW_AREA = 6;
    public const int DIFFICULT_TO_BLOW_OFF_DRAW_AREA = 7;
}
