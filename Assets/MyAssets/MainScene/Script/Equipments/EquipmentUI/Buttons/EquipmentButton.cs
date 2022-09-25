﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 
/// 「装備」ボタン。 <br/>
/// 「装備する」ボタンとは異なる。 <br/>
/// </summary>
public class EquipmentButton : UseEventSystemBehavior
{
    /// <summary> このボタンの装備 </summary>
    public Equipment _myEquipment { get; private set; }

    //<===== メンバー変数 =====>//
    /// <summary> 子のテキストオブジェクト </summary>
    Text _myText;
    /// <summary> 子オブジェクトである「装備する」ボタン </summary>
    GameObject _equipButton_OtherArm;
    GameObject _equipButton_LeftArm;
    GameObject _equipButton_RightArm;


    //<===== Unityメッセージ =====>//
    protected override void Start()
    {
        base.Start();
        //テキストを取得
        _myText = transform.GetComponentInChildren<Text>();
        UpdateText();
        _equipButton_OtherArm = transform.GetChild(1).gameObject;
        _equipButton_LeftArm = transform.GetChild(2).gameObject;
        _equipButton_RightArm = transform.GetChild(3).gameObject;
    }

    //<===== メンバー関数 =====>//
    /// <summary> このボタンの装備を設定する。 </summary>
    public void Set_Equipment(Equipment equipment)
    {
        _myEquipment = equipment;
    }

    /// <summary> 装備名テキストを更新する </summary>
    void UpdateText()
    {
        if (_myEquipment != null) _myText.text = _myEquipment._myName;
        else gameObject.SetActive(false);
    }

    /// <summary> 「装備」ボタンをアクティブにする。 : 腕以外の場合 </summary>
    public void OnEnabled_EquipButton_OtherArm()
    {
        _equipButton_OtherArm.SetActive(true);
    }

    /// <summary> 「装備」ボタンを非アクティブにする。 : 腕以外の場合 </summary>
    public void OffEnabled_EquipButton_OtherArm()
    {
        _equipButton_OtherArm.SetActive(false);
    }

    /// <summary> 「装備」ボタンをアクティブにする。 : 左腕の場合 </summary>
    public void OnEnabled_EquipButton_LeftArm()
    {
        _equipButton_LeftArm.SetActive(true);
    }

    /// <summary> 「装備」ボタンを非アクティブにする。 : 左腕の場合 </summary>
    public void OffEnabled_EquipButton_LeftArm()
    {
        _equipButton_LeftArm.SetActive(false);
    }
    /// <summary> 「装備」ボタンをアクティブにする。 : 右腕の場合 </summary>
    public void OnEnabled_EquipButton_RightArm()
    {
        _equipButton_RightArm.SetActive(true);
    }

    /// <summary> 「装備」ボタンを非アクティブにする。 : 右腕の場合 </summary>
    public void OffEnabled_EquipButton_RightArm()
    {
        _equipButton_RightArm.SetActive(false);
    }

    //<====== 以下ボタンを押したときに実行する関数 : 他のクラスに移すかも ======>//
    /// <summary> 
    /// 「装備する」ボタンをクリック時に実行する。
    /// 着用している装備と所持している装備を交換する。 : 腕以外 
    /// </summary>
    public void OnClick_ExecutionSwap_OtherArm()
    {
        // このボタンが持つ装備を着用する。この機能は「装備」ボタンに持たせるべきか？
        //EquipmentDataBase.Instance.Swap_HaveToEquipped((int)_myEquipment._myID, _myEquipment._myType, this);
        EquipmentManager.Instance.Swap_HaveToEquipped(this);
        UpdateText();
        OffEnabled_EquipButton_OtherArm();
        _eventSystem.SetSelectedGameObject(null);
    }
    /// <summary>
    /// 「装備する」ボタンをクリック時に実行する。
    /// 着用している装備と所持している装備を交換する。 : 左腕 
    /// </summary>
    public void OnClick_ExecutionSwap_LeftArm()
    {
        // このボタンが持つ装備を着用する。この機能は「装備」ボタンに持たせるべきか？
        //EquipmentDataBase.Instance.Swap_HaveToEquipped((int)_myEquipment._myID, _myEquipment._myType, this, Constants.LEFT_ARM);
        EquipmentManager.Instance.Swap_HaveToEquipped(this, Constants.LEFT_ARM);
        UpdateText();
        OffEnabled_EquipButton_LeftArm();
        OffEnabled_EquipButton_RightArm();
        _eventSystem.SetSelectedGameObject(null);
    }
    /// <summary> 
    /// 「装備する」ボタンをクリック時に実行する。
    /// 着用している装備と所持している装備を交換する。 : 左腕 
    /// </summary>
    public void OnClick_ExecutionSwap_RightArm()
    {
        // このボタンが持つ装備を着用する。この機能は「装備」ボタンに持たせるべきか？
        //EquipmentDataBase.Instance.Swap_HaveToEquipped((int)_myEquipment._myID, _myEquipment._myType, this, Constants.RIGHT_ARM);
        EquipmentManager.Instance.Swap_HaveToEquipped(this, Constants.RIGHT_ARM);
        UpdateText();
        OffEnabled_EquipButton_LeftArm();
        OffEnabled_EquipButton_RightArm();
        _eventSystem.SetSelectedGameObject(null);
    }
    /// <summary> 「装備」ボタンをクリック時に実行する。 </summary>
    public void OnClick_EquipmentButton()
    {
        //現在選択中のパーツの「装備する」ボタンをアクティブにする。
        if (_myEquipment._myType != Equipment.EquipmentType.ARM_PARTS)
        {
            OnEnabled_EquipButton_OtherArm();
        }
        else
        {
            OnEnabled_EquipButton_LeftArm();
            OnEnabled_EquipButton_RightArm();
        }
    }
}
