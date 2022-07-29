using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> �����{�^�������R���|�[�l���g </summary>
public class EquipmentButton : MonoBehaviour
{
    /// <summary> ���̃{�^���̑��� </summary>
    public Equipment _myEquipment { get; private set; }
    Text _myNameText;
    /// <summary> �q�̑����{�^�� </summary>
    GameObject _equipButton;

    private void Start()
    {
        //�e�L�X�g���擾
        _myNameText = transform.GetComponentInChildren<Text>();
        UpdateText();
        _equipButton = transform.GetChild(1).gameObject;
    }

    /// <summary> ���̃{�^���̑�����ݒ肷��B </summary>
    public void Set_Equipment(Equipment equipment)
    {
        _myEquipment = equipment;
    }

    void UpdateText()
    {
        if (_myEquipment != null) _myNameText.text = _myEquipment._myName;
        else gameObject.SetActive(false);
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

    /// <summary> ���̃{�^�������������Ɏ��s���ׂ��֐� </summary>
    public void OnClick_ThisButton()
    {
        // ���̃{�^�����������𒅗p����B
        EquipmentManager.Instance.Swap_HaveToEquipped((int)_myEquipment._myID, _myEquipment._myType, this);
        UpdateText();
    }
}
