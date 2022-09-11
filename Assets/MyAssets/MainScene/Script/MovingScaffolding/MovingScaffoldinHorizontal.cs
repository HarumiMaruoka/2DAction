using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 動く足場を制御するコンポーネント : 水平方向版
/// </summary>
public class MovingScaffoldinHorizontal : MonoBehaviour
{
    [Header("横の移動量"), SerializeField] float _endPointX;
    [Header("掛ける時間"), SerializeField] float _moveTimeX;


    void Start()
    {
        transform.
        DOMoveX(_endPointX, _moveTimeX). // X方向に移動する。_endPointX に向けて _moveTimeX 秒間掛けて。
        SetEase(Ease.Linear).            // 移動のニュアンスを付ける。 一覧 → https://game-ui.net/?p=835 
        SetLoops(-1, LoopType.Yoyo);     // 何回繰り返すか。-1で無限回。LoopTypeは繰り返し方法。
    }
    void OnDestroy()
    {
        // DOTween を止める（止めないと警告が出る）
        DOTween.KillAll();
    }
}
