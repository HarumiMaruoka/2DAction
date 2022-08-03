using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> �����{�^�������R���|�[�l���g </summary>
public class EquipmentButton : MonoBehaviour
{
    /// <summary> ���̃{�^���̑��� </summary>
    public Equipment _myEquipment { get; private set; }
    /// <summary> �q�̃e�L�X�g�I�u�W�F�N�g </summary>
    Text _myText;
    /// <summary> �q�̑����{�^�� </summary>
    GameObject _equipButton;

    private void Start()
    {
        //�e�L�X�g���擾
        _myText = transform.GetComponentInChildren<Text>();
        UpdateText();
        _equipButton = transform.GetChild(1).gameObject;
    }

    /// <summary> ���̃{�^���̑�����ݒ肷��B </summary>
    public void Set_Equipment(Equipment equipment)
    {
        _myEquipment = equipment;
    }

    /// <summary> �������e�L�X�g���X�V���� </summary>
    void UpdateText()
    {
        if (_myEquipment != null) _myText.text = _myEquipment._myName;
        else gameObject.SetActive(false);
    }

    /// <summary> �u�����v�{�^�����A�N�e�B�u�ɂ���B </summary>
    public void OnEnabled_EquipButton()
    {
        _equipButton.SetActive(true);
    }

    /// <summary> �u�����v�{�^�����A�N�e�B�u�ɂ���B </summary>
    public void OffEnabled_EquipButton()
    {
        _equipButton.SetActive(false);
    }

    /// <summary> ���̃{�^�������������Ɏ��s���ׂ��֐� </summary>
    public void OnClick_ThisButton()
    {
        // ���̃{�^�����������𒅗p����B���̋@�\�́u�����v�{�^���Ɏ�������ׂ����H
        EquipmentManager.Instance.Swap_HaveToEquipped((int)_myEquipment._myID, _myEquipment._myType, this);
        UpdateText();
    }
}
