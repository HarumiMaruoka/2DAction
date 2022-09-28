using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 
/// 装備画面に、プレイヤーステータスを描画するコンポーネント。
/// </summary>
public class DrawPlayerStatus_OnEquipmentWindow : MonoBehaviour
{

    /// <summary> このクラスを初期化したかどうか </summary>
    bool _whetherInitialized = false;
    /// <summary> 子オブジェクトが持つ、プレイヤーステータスを表示するテキストコンポーネントの配列 </summary>
    Text[] _playerStatusText;

    //<===== unityメッセージ =====>//
    void Start()
    {
        Initialize_ThisClass();
    }
    void OnEnable()
    {
        EquipmentUIUpdateManager.SwapEquipmentUpdate += Update_StatusText;
    }
    void OnDisable()
    {
        EquipmentUIUpdateManager.SwapEquipmentUpdate -= Update_StatusText;
    }

    //<===== privateメンバー関数 =====>//
    /// <summary> このクラスの初期化関数。成功したらtrueを返す。 </summary>
    bool Initialize_ThisClass()
    {
        //テキストを持つ全ての子オブジェクトを取得する。
        _playerStatusText = GetComponentsInChildren<Text>();
        //取得したテキストを更新する。
        Update_StatusText();

        return true;
    }
    /// <summary> プレイヤーステータステキストを更新する処理 </summary>
    /// <param name="drawAmountOfChangeFlag"> 変化幅を表示するかどうか : true なら表示する。 </param>
    void Update_StatusText()
    {
        var status = PlayerStatusManager.Instance.ConsequentialPlayerStatus;
        _playerStatusText[Constants.PLAYER_NAME_DRAW_AREA].text = PlayerStatusManager.Instance.BaseStatus._playerName;

        _playerStatusText[Constants.MAX_HP_DRAW_AREA].text = $"最大体力 : {status._maxHp}";

        _playerStatusText[Constants.MAX_STAMINA_TYPE_DRAW_AREA].text = $"最大スタミナ : {status._maxStamina}";

        _playerStatusText[Constants.SHORT_RANGE_ATTACK_POWER_DRAW_AREA].text = $"近距離攻撃力 : {status._shortRangeAttackPower}";

        _playerStatusText[Constants.LONG_RANGE_ATTACK_POWER_DRAW_AREA].text = $"遠距離攻撃力 : {status._longRangeAttackPower}";

        _playerStatusText[Constants.DEFENSE_POWER_DRAW_AREA].text = $"防御力 : {status._defensePower}";

        _playerStatusText[Constants.MOVE_SPEED_DRAW_AREA].text = $"移動速度 : {status._moveSpeed}";

        _playerStatusText[Constants.DIFFICULT_TO_BLOW_OFF_DRAW_AREA].text = $"吹っ飛びにくさ : {status._difficultToBlowOff}";
    }
}
