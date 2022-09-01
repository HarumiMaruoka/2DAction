using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> �u�����v�̃{�^�� </summary>
public class EquipmentButton : UseEventSystemBehavior
{
    //<===== ���̃N���X�Ŏg�p����^ =====>//
    /// <summary> ���̃{�^���̑��� </summary>
    public Equipment _myEquipment { get; private set; }

    //<===== �����o�[�ϐ� =====>//
    /// <summary> �q�̃e�L�X�g�I�u�W�F�N�g </summary>
    Text _myText;
    /// <summary> �q�̑����{�^�� </summary>
    GameObject _equipButton_OtherArm;
    GameObject _equipButton_LeftArm;
    GameObject _equipButton_RightArm;

    //<===== Unity���b�Z�[�W =====>//
    private void Start()
    {
        base.Initialized_UseEventSystemBehavior();
        //�e�L�X�g���擾
        _myText = transform.GetComponentInChildren<Text>();
        UpdateText();
        _equipButton_OtherArm = transform.GetChild(1).gameObject;
        _equipButton_LeftArm = transform.GetChild(2).gameObject;
        _equipButton_RightArm = transform.GetChild(3).gameObject;
    }

    //<===== �����o�[�֐� =====>//
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

    /// <summary> �u�����v�{�^�����A�N�e�B�u�ɂ���B : �r�ȊO�̏ꍇ </summary>
    public void OnEnabled_EquipButton_OtherArm()
    {
        _equipButton_OtherArm.SetActive(true);
    }

    /// <summary> �u�����v�{�^�����A�N�e�B�u�ɂ���B : �r�ȊO�̏ꍇ </summary>
    public void OffEnabled_EquipButton_OtherArm()
    {
        _equipButton_OtherArm.SetActive(false);
    }

    /// <summary> �u�����v�{�^�����A�N�e�B�u�ɂ���B : ���r�̏ꍇ </summary>
    public void OnEnabled_EquipButton_LeftArm()
    {
        _equipButton_LeftArm.SetActive(true);
    }

    /// <summary> �u�����v�{�^�����A�N�e�B�u�ɂ���B : ���r�̏ꍇ </summary>
    public void OffEnabled_EquipButton_LeftArm()
    {
        _equipButton_LeftArm.SetActive(false);
    }
    /// <summary> �u�����v�{�^�����A�N�e�B�u�ɂ���B : �E�r�̏ꍇ </summary>
    public void OnEnabled_EquipButton_RightArm()
    {
        _equipButton_RightArm.SetActive(true);
    }

    /// <summary> �u�����v�{�^�����A�N�e�B�u�ɂ���B : �E�r�̏ꍇ </summary>
    public void OffEnabled_EquipButton_RightArm()
    {
        _equipButton_RightArm.SetActive(false);
    }

    //<====== �ȉ��{�^�����������Ƃ��Ɏ��s����֐� : ���̃N���X�Ɉڂ����� ======>//
    /// <summary> 
    /// �u��������v�{�^�����N���b�N���Ɏ��s����B
    /// ���p���Ă��鑕���Ə������Ă��鑕������������B : �r�ȊO 
    /// </summary>
    public void OnClick_ExecutionSwap_OtherArm()
    {
        // ���̃{�^�����������𒅗p����B���̋@�\�́u�����v�{�^���Ɏ�������ׂ����H
        EquipmentDataBase.Instance.Swap_HaveToEquipped((int)_myEquipment._myID, _myEquipment._myType, this);
        UpdateText();
        OffEnabled_EquipButton_OtherArm();
        _eventSystem.SetSelectedGameObject(null);
    }
    /// <summary>
    /// �u��������v�{�^�����N���b�N���Ɏ��s����B
    /// ���p���Ă��鑕���Ə������Ă��鑕������������B : ���r 
    /// </summary>
    public void OnClick_ExecutionSwap_LeftArm()
    {
        // ���̃{�^�����������𒅗p����B���̋@�\�́u�����v�{�^���Ɏ�������ׂ����H
        EquipmentDataBase.Instance.Swap_HaveToEquipped((int)_myEquipment._myID, _myEquipment._myType, this, Constants.LEFT_ARM);
        UpdateText();
        OffEnabled_EquipButton_LeftArm();
        OffEnabled_EquipButton_RightArm();
        _eventSystem.SetSelectedGameObject(null);
    }
    /// <summary> 
    /// �u��������v�{�^�����N���b�N���Ɏ��s����B
    /// ���p���Ă��鑕���Ə������Ă��鑕������������B : ���r 
    /// </summary>
    public void OnClick_ExecutionSwap_RightArm()
    {
        // ���̃{�^�����������𒅗p����B���̋@�\�́u�����v�{�^���Ɏ�������ׂ����H
        EquipmentDataBase.Instance.Swap_HaveToEquipped((int)_myEquipment._myID, _myEquipment._myType, this, Constants.RIGHT_ATM);
        UpdateText();
        OffEnabled_EquipButton_LeftArm();
        OffEnabled_EquipButton_RightArm();
        _eventSystem.SetSelectedGameObject(null);
    }
    /// <summary> �u�����v�{�^�����N���b�N���Ɏ��s����B </summary>
    public void OnClick_EquipmentButton()
    {
        //���ݑI�𒆂̃p�[�c�́u��������v�{�^�����A�N�e�B�u�ɂ���B
        if (_myEquipment._myType != Equipment.EquipmentType.ARM_PARTS)
        {
            OnEnabled_EquipButton_OtherArm();
        }
        else
        {
            OnEnabled_EquipButton_LeftArm();
            OnEnabled_EquipButton_RightArm();
        }
        EquipmentDataBase.Instance.IsTextUpdate = true;
    }
}
