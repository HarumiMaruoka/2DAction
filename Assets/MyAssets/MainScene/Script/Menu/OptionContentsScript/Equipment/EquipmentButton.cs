using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 装備ボタンが持つコンポーネント </summary>
public class EquipmentButton : MonoBehaviour
{
    /// <summary> このボタンの装備 </summary>
    public Equipment _myEquipment { get; private set; }
    Text _myNameText;
    /// <summary> 子の装備ボタン </summary>
    GameObject _equipButton;

    private void Start()
    {
        //テキストを取得
        _myNameText = transform.GetComponentInChildren<Text>();
        UpdateText();
        _equipButton = transform.GetChild(1).gameObject;
    }

    /// <summary> このボタンの装備を設定する。 </summary>
    public void Set_Equipment(Equipment equipment)
    {
        _myEquipment = equipment;
    }

    void UpdateText()
    {
        _myNameText.text = _myEquipment._myName;
    }

    public void OnEnabled_EquipButton()
    {
        Debug.Log(_equipButton.gameObject.name);
        _equipButton.SetActive(true);
    }

    public void OffEnabled_EquipButton()
    {
        _equipButton.SetActive(false);
    }
}
