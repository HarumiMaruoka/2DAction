using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersUI : MonoBehaviour
{
    //スライダーを取得
    [SerializeField] Slider _slider;
    PlayerBasicInformation _playerBasicInformation;

    // Start is called before the first frame update
    void Start()
    {
        _playerBasicInformation = GetComponent<PlayerBasicInformation>();
        _slider.maxValue = _playerBasicInformation._maxHitPoint;
        _slider.minValue = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _slider.value = _playerBasicInformation._playerHitPoint;
    }
}
