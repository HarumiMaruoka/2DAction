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
        _hitPointSlider.maxValue = _playerBasicInformation._maxHitPoint;
        _hitPointSlider.minValue = 0;
        //ホバー用スライダーの初期化
        _hoverSlider.maxValue = _playerBasicInformation._maxHealthForHover;
        _hoverSlider.minValue = 0;
    }

    private void FixedUpdate()
    {
        _hitPointSlider.value = _playerBasicInformation._playerHitPoint;
        _hoverSlider.value = _playerBasicInformation._hoverValue;
    }

}
