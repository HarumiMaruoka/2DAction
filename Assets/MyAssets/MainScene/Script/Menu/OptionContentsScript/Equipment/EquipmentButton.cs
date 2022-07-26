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

    private void Start()
    {
        //�e�L�X�g���擾
        _myNameText = transform.GetComponentInChildren<Text>();
        UpdateText();
    }

    /// <summary> ���̃{�^���̑�����ݒ肷��B </summary>
    public void Set_Equipment(Equipment equipment)
    {
        _myEquipment = equipment;
    }

    void UpdateText()
    {
        _myNameText.text = _myEquipment._myName;
    }
}
