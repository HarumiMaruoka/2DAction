using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 装備ボタンが持つコンポーネント </summary>
public class EquipmentButton : MonoBehaviour
{
    /// <summary> このボタンの装備 </summary>
    public Equipment _myEquipment { get; private set; }
    /// <summary> 子のテキストオブジェクト </summary>
    Text _myText;
    /// <summary> 子の装備ボタン </summary>
    GameObject _equipButton;

    private void Start()
    {
        //テキストを取得
        _myText = transform.GetComponentInChildren<Text>();
        UpdateText();
        _equipButton = transform.GetChild(1).gameObject;
    }

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

    /// <summary> 「装備」ボタンをアクティブにする。 </summary>
    public void OnEnabled_EquipButton()
    {
        _equipButton.SetActive(true);
    }

    /// <summary> 「装備」ボタンを非アクティブにする。 </summary>
    public void OffEnabled_EquipButton()
    {
        _equipButton.SetActive(false);
    }

    /// <summary> このボタンを押した時に実行すべき関数 </summary>
    public void OnClick_ThisButton()
    {
        // このボタンが持つ装備を着用する。この機能は「装備」ボタンに持たせるべきか？
        EquipmentManager.Instance.Swap_HaveToEquipped((int)_myEquipment._myID, _myEquipment._myType, this);
        UpdateText();
    }
}
