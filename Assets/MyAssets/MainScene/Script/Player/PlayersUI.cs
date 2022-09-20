using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersUI : MonoBehaviour
{
    //スライダーを取得
    [SerializeField] Slider _hitPointSlider;
    [SerializeField] Slider _hoverSlider;
    PlayerBasicInformation _playerBasicInformation;

    void Start()
    {
        //コンポーネントを取得
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();

        //HP用スライダー
        _hitPointSlider.maxValue = PlayerStatusManager.Instance.ConsequentialPlayerStatus._maxHp;
        _hitPointSlider.minValue = 0;
        //ホバー用スライダーの初期化
        _hoverSlider.maxValue = _playerBasicInformation.MaxHealthForHover;
        _hoverSlider.minValue = 0;
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
        _hoverSlider.value = _playerBasicInformation._hoverValue;
    }
}
