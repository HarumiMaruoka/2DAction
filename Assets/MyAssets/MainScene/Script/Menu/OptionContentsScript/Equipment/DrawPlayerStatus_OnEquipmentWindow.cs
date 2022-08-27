using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 
/// 装備画面に、プレイヤーステータスを描画するコンポーネント。
/// 現在選択中のパーツを装備することによるステータスの変化量を描画する機能を追記してください。
/// </summary>
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

    //<===== unityメッセージ =====>//
    void Start()
    {
        //初期化していなければ初期化する。
        if (!_whetherInitialized)
        {
            if(!(_whetherInitialized = Initialize_ThisClass()))
            {
                Debug.LogError($"初期化に失敗しました。 : ゲームオブジェクト名 {gameObject.name}");
            }
        }
    }
    private void OnEnable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment += SetALL_PlayerStatusText;
    }
    private void OnDisable()
    {
        EquipmentDataBase.Instance.ReplacedEquipment -= SetALL_PlayerStatusText;
    }

    //<===== privateメンバー関数 =====>//
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
        _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.BaseStatus._playerName;

        //繰り返し使用するので変数に保存。
        var status = PlayerStatusManager.Instance.ConsequentialPlayerStatus;
        //プレイヤーの最大体力を設定する
        _playerStatusText[(int)StatusName.MaxHP].text =
            $"最大体力 : {status._maxHp}";
        //プレイヤーの最大スタミナを設定する
        _playerStatusText[(int)StatusName.MaxStamina].text =
            $"最大スタミナ : {status._maxStamina}";
        //プレイヤーの近距離攻撃力を設定する
        _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = 
            $"近距離攻撃力 : {status._shortRangeAttackPower}";
        //プレイヤーの遠距離攻撃力を設定する
        _playerStatusText[(int)StatusName.LongRangeAttackPower].text =
            $"遠距離攻撃力 : {status._longRangeAttackPower}";
        //プレイヤーの防御力を設定する
        _playerStatusText[(int)StatusName.DefensePower].text = 
            $"防御力 : {status._defensePower}";
        //プレイヤーの移動速度を設定する
        _playerStatusText[(int)StatusName.MoveSpeed].text =
            $"移動速度 : {status._moveSpeed}";
        //プレイヤーの吹っ飛びにくさを設定する
        _playerStatusText[(int)StatusName.DifficultToBlowOff].text = 
            $"吹っ飛びにくさ : {status._defensePower}";
    }

    /// <summary> ターゲットのステータステキストを設定する。 </summary>
    /// <param name="target"></param>
   　void Update_TargetPlayerStatusText(StatusName target)
    {
        switch (target)
        {
            case StatusName.PlayerName: _playerStatusText[(int)StatusName.PlayerName].text = PlayerStatusManager.Instance.BaseStatus._playerName; break;
            case StatusName.MaxHP: _playerStatusText[(int)StatusName.MaxHP].text = "最大体力 : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp.ToString(); break;
            case StatusName.MaxStamina: _playerStatusText[(int)StatusName.MaxStamina].text = "最大スタミナ : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxStamina.ToString(); break;
            case StatusName.ShortRangeAttackPower: _playerStatusText[(int)StatusName.ShortRangeAttackPower].text = "近距離攻撃力 : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._shortRangeAttackPower.ToString(); break;
            case StatusName.LongRangeAttackPower: _playerStatusText[(int)StatusName.LongRangeAttackPower].text = "遠距離攻撃力 : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._longRangeAttackPower.ToString(); break;
            case StatusName.DefensePower: _playerStatusText[(int)StatusName.DefensePower].text = "防御力 : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._defensePower.ToString(); break;
            case StatusName.MoveSpeed: _playerStatusText[(int)StatusName.MoveSpeed].text = "移動速度 : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._moveSpeed.ToString(); break;
            case StatusName.DifficultToBlowOff: _playerStatusText[(int)StatusName.DifficultToBlowOff].text = "吹っ飛びにくさ : " + PlayerStatusManager.Instance.ConsequentialPlayerStatus._difficultToBlowOff.ToString(); break;
            default: Debug.LogError("無効な値です。 : Set_PlayerStatusText(StatusName target);"); break;
        }
    }
}
