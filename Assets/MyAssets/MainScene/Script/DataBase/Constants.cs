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
    public const int RIGHT_ARM = 1;
    /// <summary> 腕以外を表す定数。 </summary>
    public const int NOT_ARM = -1;
    /// <summary> 変化幅を描画する </summary>
    public const bool DRAW_AMPLITUDE = true;
    /// <summary> 変化幅を表示しない </summary>
    public const bool NOT_DRAW_AMPLITUDE = false;

    //装備UIクラスでステータス等を表示するのに使用する定数
    public const int PLAYER_NAME_DRAW_AREA = 0;
    public const int EQUIPMENT_TYPE_DRAW_AREA = 0;
    public const int MAX_HP_DRAW_AREA = 1;
    public const int MAX_STAMINA_TYPE_DRAW_AREA = 2;
    public const int SHORT_RANGE_ATTACK_POWER_DRAW_AREA = 3;
    public const int LONG_RANGE_ATTACK_POWER_DRAW_AREA = 4;
    public const int DEFENSE_POWER_DRAW_AREA = 5;
    public const int MOVE_SPEED_DRAW_AREA = 6;
    public const int DIFFICULT_TO_BLOW_OFF_DRAW_AREA = 7;

    /// <summary> Fireボタンを瞬間だけ実行することを表す値。</summary>
    public const bool ON_FIRE_PRESS_TYPE_MOMENT = true;
    /// <summary> Fireボタンを押している間、継続して実行することを表す値。 </summary>
    public const bool ON_FIRE_PRESS_TYPE_CONSECUTIVELY = false;

    //向きを表す定数
    public const float UP = 1f;
    public const float DOWN = -1f;
    public const float RIGHT = 1f;
    public const float LEFT = -1;


    /// <summary> プレイヤーが所持できる装備の最大数。 </summary>
    public const int EQUIPMENT_MAX_HAVE_VALUE = 20;

    /// <summary> アニメーション通常再生時のスピード </summary>
    public const float NOMAL_ANIM_SPEED = 1f;
    /// <summary> アニメーション逆再生時のスピード </summary>
    public const float REVERSE_PLAYBACK_ANIM_SPEED = -1f;
    /// <summary> アニメーション一時停止時のスピード </summary>
    public const float PAUSE_ANIM_SPEED = 0f;

    //===== Tag一覧 =====//
    /// <summary> プレイヤーのタグ名 </summary>
    public const string PLAYER_TAG_NAME = "Player";
    /// <summary> グラウンド(地面)のタグ名 </summary>
    public const string GROUND_TAG_NAME = "Ground";

    /// <summary> アイテムマネージャーのタグ名 </summary>
    public const string ITEM_MANAGER_TAG_NAME = "ItemManager";
    /// <summary> 装備UIマネージャーのタグ名 </summary>
    public const string EQUIPMENT_UI_COMPONENTS_MANAGER_TAG = "EquipmentUIComponentsManager";
}
