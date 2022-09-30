using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// プレイヤーの残り体力やスタミナをプレイヤーが認知する為のUIを
/// 制御するためのコンポーネント。
/// </summary>
public class PlayerUIManager : MonoBehaviour
{
    //===== インスペクタ変数 =====//
    [Header("アサインすべきUI")]
    [Tooltip("体力を表現するためのUI(スライダー)"), SerializeField]
    Slider _hitPointSlider;
    [Tooltip("スタミナを表現するためのUI(スライダー)"), SerializeField]
    Slider _staminaSlider;

    //===== フィールド =====//
    /// <summary>
    /// プレイヤーの体力等を管理しているクラス
    /// </summary>
    PlayerBasicInformation _playerBasicInformation;

    //===== Unityメッセージ =====//
    private void Start()
    {
        Init();
    }
    private void FixedUpdate()
    {
        // 最大体力が変更されたらスライダーの最大値の設定する。
        if (Mathf.Abs(_hitPointSlider.maxValue - PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp) > 0.01f)
        {
            _hitPointSlider.maxValue = PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp;
        }
        // 体力が変更されたらスライダーの値を変更する。
        if (Mathf.Abs(_hitPointSlider.value - PlayerStatusManager.Instance.PlayerHealthPoint) > 0.01f)
        {
            _hitPointSlider.value = PlayerStatusManager.Instance.PlayerHealthPoint;
        }
        // 現在のスタミナ量を代入する。
        _staminaSlider.value = _playerBasicInformation._hoverValue;
    }

    //===== privateメソッド =====//
    private bool Init()
    {
        //コンポーネントを取得
        if ((_playerBasicInformation = GetComponent<PlayerBasicInformation>()) == null)
        {
            Debug.LogError(
                $"{gameObject.name} : 初期化に失敗しました！\n 修正してください！");
            return false;
        }

        // 体力用スライダーの初期設定を行う
        _hitPointSlider.maxValue = PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp;
        _hitPointSlider.minValue = 0;
        // スタミナ用スライダーの初期設定を行う
        _staminaSlider.maxValue = _playerBasicInformation.MaxHealthForHover;
        _staminaSlider.minValue = 0;

        return true;
    }
}
