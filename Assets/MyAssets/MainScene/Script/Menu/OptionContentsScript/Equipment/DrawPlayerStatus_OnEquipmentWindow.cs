using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 装備画面にプレイヤーステータスを描画する機能 </summary>
public class DrawPlayerStatus_OnEquipmentWindow : MonoBehaviour
{
    enum StatusName
    {
        /// <summary> 名前 </summary>
        PlayerName,
        /// <summary> 最大体力 </summary>
        MaxHP,
        /// <summary> 最大スタミナ </summary>
        MaxStamina,
        /// <summary> 近距離攻撃力 </summary>
        ShortRangeAttackPower,
        /// <summary> 遠距離攻撃力 </summary>
        LongRangeAttackPower,
        /// <summary> 防御力 </summary>
        DefensePower,
        /// <summary> 移動速度 </summary>
        MoveSpeed,
        /// <summary> 吹っ飛びにくさ </summary>
        DifficultToBlowOff
    }

    /// <summary> このクラスを初期化したかどうか </summary>
    bool _whetherInitialized = false;

    //子オブジェクトを格納する場所
    Text[] _playerStatusText;

    void Start()
    {
        //初期化していなければ初期化する。
        if (!_whetherInitialized)
        {
            _whetherInitialized = Initialize_ThisClass();
        }
        if (!_whetherInitialized)
        {
            Debug.LogError("クラスの初期化に失敗しました。 : クラス名 : DrawPlayerStatus_OnEquipmentWindow");
        }
    }

    void Update()
    {

    }

    /// <summary> このクラスの初期化関数。成功したらtrueを返す。 </summary>
    bool Initialize_ThisClass()
    {
        //テキストを持つ全ての子オブジェクトを取得する。
        _playerStatusText = GetComponentsInChildren<Text>();
        SetALL_PlayerStatusText();

        return true;
    }

    /// <summary> プレイヤーのステータスを、テキストに設定する。 </summary>
    void SetALL_PlayerStatusText()
    {
        //プレイヤーの名前を設定する
        _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.PlayerName;
        //プレイヤーの最大体力を設定する
        _playerStatusText[(int)StatusName.MaxHP].text = PlayerStatusManager.Instance.PlayerMaxHealthPoint.ToString();
        //プレイヤーの最大スタミナを設定する
        _playerStatusText[(int)StatusName.MaxStamina].text = PlayerStatusManager.Instance.PlayerMaxStamina.ToString();
        //プレイヤーの近距離攻撃力を設定する
        _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = PlayerStatusManager.Instance.PlayerShortRangeAttackPower.ToString();
        //プレイヤーの遠距離攻撃力を設定する
        _playerStatusText[(int)StatusName.LongRangeAttackPower].text = PlayerStatusManager.Instance.PlayerLongRangeAttackPower.ToString();
        //プレイヤーの防御力を設定する
        _playerStatusText[(int)StatusName.DefensePower].text = PlayerStatusManager.Instance.PlayerDefensePower.ToString();
        //プレイヤーの移動速度を設定する
        _playerStatusText[(int)StatusName.MoveSpeed].text = PlayerStatusManager.Instance.PlayerMoveSpeed.ToString();
        //プレイヤーの吹っ飛びにくさを設定する
        _playerStatusText[(int)StatusName.DifficultToBlowOff].text = PlayerStatusManager.Instance.PlayerDifficultToBlowOff.ToString();
    }

    /// <summary> ターゲットのステータステキストを設定する。 </summary>
    /// <param name="target"></param>
    void Set_PlayerStatusText(StatusName target)
    {
        switch (target)
        {
            case StatusName.PlayerName: _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.PlayerName; break;
            case StatusName.MaxHP: _playerStatusText[(int)StatusName.MaxHP].text = PlayerStatusManager.Instance.PlayerMaxHealthPoint.ToString(); break;
            case StatusName.MaxStamina: _playerStatusText[(int)StatusName.MaxStamina].text = PlayerStatusManager.Instance.PlayerMaxStamina.ToString(); break;
            case StatusName.ShortRangeAttackPower: _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = PlayerStatusManager.Instance.PlayerShortRangeAttackPower.ToString(); break;
            case StatusName.LongRangeAttackPower: _playerStatusText[(int)StatusName.LongRangeAttackPower].text = PlayerStatusManager.Instance.PlayerLongRangeAttackPower.ToString(); break;
            case StatusName.DefensePower: _playerStatusText[(int)StatusName.DefensePower].text = PlayerStatusManager.Instance.PlayerDefensePower.ToString(); break;
            case StatusName.MoveSpeed: _playerStatusText[(int)StatusName.MoveSpeed].text = PlayerStatusManager.Instance.PlayerMoveSpeed.ToString(); break;
            case StatusName.DifficultToBlowOff: _playerStatusText[(int)StatusName.DifficultToBlowOff].text = PlayerStatusManager.Instance.PlayerDifficultToBlowOff.ToString(); break;
            default: Debug.LogError("無効な値です。 : Set_PlayerStatusText(StatusName target);");break;
        }
    }
}
