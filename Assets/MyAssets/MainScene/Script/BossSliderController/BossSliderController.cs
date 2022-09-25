using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// ボスの残り体力を表現するためのスライダーのコントローラー
/// </summary>
public class BossSliderController : MonoBehaviour
{
    //===== フィールド / プロパティ =====//
    Slider _slider = default;
    NewBossBase _boss = default;

    //===== Unityメッセージ =====//
    void Start()
    {
        Init();
    }
    void Update()
    {
        if (_boss.IsFight)
        {
            _slider.value = _boss.Status._hitPoint;
        }
    }

    //===== privateメソッド =====//
    /// <summary> 初期化処理 </summary>
    void Init()
    {
        if ((_slider = GetComponent<Slider>()) == null) Debug.LogError($"{gameObject.name}が初期化に失敗しました。");
        if ((_boss = GameObject.FindObjectOfType<NewBossBase>()) == null) Debug.LogError($"{gameObject.name}が初期化に失敗しました。");
        // スライダーの最大値を設定する。
        _slider.maxValue = _boss.Status._hitPoint;
    }
}
