using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary> �������Ă��鑕���̃{�^�����Ǘ�����N���X </summary>
public class ManagerOfPossessedEquipment : MonoBehaviour
{
    //<=========== �K�v�Ȓl ===========>//
    /// <summary> �����{�^���̔z�� </summary>
    GameObject[] _equipmentButtons;
    GameObject _beforeSelectedGameObject;
    Text[] _riseValueTexts;

    //<======== �A�T�C�����ׂ��l ========>//
    [Header("�{�^���̐e�ƂȂ�ׂ��R���e���g"), SerializeField] Transform _content;
    /// <summary> �����{�^���̃v���n�u </summary>
    [Header("�����{�^���̃v���n�u"), SerializeField] GameObject _equipmentButtonPrefab;
    [Header("�C�x���g�V�X�e��"), SerializeField] EventSystem _eventSystem;
    [Header("�����̏���\������e�L�X�g�̐e"), SerializeField] GameObject _equipmentInformationParents;
    [Header("�I�𒆂̑����̐�������\������G���A�̃Q�[���I�u�W�F�N�g"), SerializeField] Text _ExplanatoryTextArea;

    void Start()
    {
        Initialized_ThisClass();
    }

    void Update()
    {
        Update_DrawEquipmentInformation();
    }

    /// <summary> ���̃N���X�̏������֐� </summary>
    void Initialized_ThisClass()
    {
        //�z�񕪂̃��������m��
        _equipmentButtons = new GameObject[EquipmentManager.Instance.MaxHaveValue];
        //�����ł��鐔�����{�^���𐶐����A�z��ɕۑ����Ă����B
        for (int i = 0; i < EquipmentManager.Instance.MaxHaveValue; i++)
        {
            //���������B
            _equipmentButtons[i] = Instantiate(_equipmentButtonPrefab, Vector3.zero, Quaternion.identity, _content);
            //���������{�^���ɒl��ݒ肷��B
            Set_ValueToButton(i);
        }
        //�����̏���\������e�L�X�g�I�u�W�F�N�g���擾���A�ϐ��ɕۑ����Ă����B
        _riseValueTexts = _equipmentInformationParents.transform.GetComponentsInChildren<Text>();
    }

    /// <summary> �S�Ẵ{�^���ɑ�������ݒ肷��B </summary>
    void Set_ValueToButtonALL()
    {
        for (int i = 0; i < EquipmentManager.Instance.MaxHaveValue; i++)
        {
            Set_ValueToButton(i);
        }
    }

    /// <summary> ����̃{�^���ɑ�������ݒ肷��B </summary>
    /// <param name="index"> �ύX�������{�^���̃C���f�b�N�X </param>
    void Set_ValueToButton(int index)
    {
        // ���������̏���ۊǂ��Ă���ꏊ����A������ID���擾����B
        int thisEquipmentID = EquipmentManager.Instance.HaveEquipmentID._equipmentsID[index];
        // -1�Ȃ珊�����Ă��Ȃ��̂�null��ݒ肷��B�����łȂ���΁A�{�^���ɑ��������Z�b�g����B
        if (thisEquipmentID != -1) _equipmentButtons[index].GetComponent<EquipmentButton>().Set_Equipment(EquipmentManager.Instance.EquipmentData[thisEquipmentID]);
        else _equipmentButtons[index].GetComponent<EquipmentButton>().Set_Equipment(null);
    }

    /// <summary> �������̕\����؂�ւ���B </summary>
    public void Update_DrawEquipmentInformation()
    {
        if (_beforeSelectedGameObject != _eventSystem.currentSelectedGameObject && _eventSystem.currentSelectedGameObject?.GetComponent<EquipmentButton>())
        {
            //�����̎��
            _riseValueTexts[0].text = "�����̎�� : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._myTypeName;
            //�ő�̗͂̑����ʂ�ݒ�
            _riseValueTexts[1].text = "�ő�̗͂̏㏸�l : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment.ThisEquipment_StatusRisingValue._maxHp.ToString();
            //�ő�X�^�~�i�̑����ʂ�ݒ�
            _riseValueTexts[2].text = "�ő�X�^�~�i�̏㏸�l : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment.ThisEquipment_StatusRisingValue._maxStamina.ToString();
            //�ߋ����U���͂̑����ʂ�ݒ�
            _riseValueTexts[3].text = "�ߋ����U���͂̏㏸�l : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment.ThisEquipment_StatusRisingValue._shortRangeAttackPower.ToString();
            //�����U���͂̑����ʂ�ݒ�
            _riseValueTexts[4].text = "�������U���͂̏㏸�l : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment.ThisEquipment_StatusRisingValue._longRangeAttackPower.ToString();
            //�h��͂̑����ʂ�ݒ�
            _riseValueTexts[5].text = "�h��͂̏㏸�l : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment.ThisEquipment_StatusRisingValue._defensePower.ToString();
            //�ړ����x�̑����ʂ�ݒ�
            _riseValueTexts[6].text = "�ړ����x�̏㏸�l : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment.ThisEquipment_StatusRisingValue._moveSpeed.ToString();
            //������тɂ����̑����ʂ�ݒ�
            _riseValueTexts[7].text = "������тɂ����̏㏸�l : " + _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment.ThisEquipment_StatusRisingValue._difficultToBlowOff.ToString();
            //��������ݒ�
            _ExplanatoryTextArea.text = _eventSystem.currentSelectedGameObject.GetComponent<EquipmentButton>()._myEquipment._explanatoryText;

            //�V���������́u�����v�{�^�����A�N�e�B�u�ɂ��A�Â������́u�����v�{�^�����A�N�e�B�u�ɂ���B
            if (_eventSystem.currentSelectedGameObject?.GetComponent<EquipmentButton>()._myEquipment._myType != Equipment.EquipmentType.ARM_PARTS)
            {
                _eventSystem.currentSelectedGameObject?.GetComponent<EquipmentButton>()?.OnEnabled_EquipButton_OtherArm();
            }
            else
            {
                _eventSystem.currentSelectedGameObject?.GetComponent<EquipmentButton>()?.OnEnabled_EquipButton_LeftArm();
                _eventSystem.currentSelectedGameObject?.GetComponent<EquipmentButton>()?.OnEnabled_EquipButton_RightArm();
            }
            if (_beforeSelectedGameObject?.GetComponent<EquipmentButton>()?._myEquipment._myType != Equipment.EquipmentType.ARM_PARTS)
            {
                _beforeSelectedGameObject?.GetComponent<EquipmentButton>()?.OffEnabled_EquipButton_OtherArm();
            }
            else
            {
                _beforeSelectedGameObject?.GetComponent<EquipmentButton>()?.OffEnabled_EquipButton_LeftArm(); 
                _beforeSelectedGameObject?.GetComponent<EquipmentButton>()?.OffEnabled_EquipButton_RightArm();
            }
        }

        //�Â��I�u�W�F�N�g��ۑ����Ă����B
        _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }

    //�����I�ɑ����̏㏸�l�e�L�X�g���X�V����
    /// <param name="equipment"> �㏸�l��\�����鑕�� </param>
    public void ForcedUpdate_RiseValueText(Equipment equipment)
    {
        //�����̎��
        _riseValueTexts[0].text = "�����̎�� : " + equipment._myTypeName;
        //�ő�̗͂̑����ʂ�ݒ�
        _riseValueTexts[1].text = "�ő�̗͂̏㏸�l : " + equipment.ThisEquipment_StatusRisingValue._maxHp.ToString();
        //�ő�X�^�~�i�̑����ʂ�ݒ�
        _riseValueTexts[2].text = "�ő�X�^�~�i�̏㏸�l : " + equipment.ThisEquipment_StatusRisingValue._maxStamina.ToString();
        //�ߋ����U���͂̑����ʂ�ݒ�
        _riseValueTexts[3].text = "�ߋ����U���͂̏㏸�l : " + equipment.ThisEquipment_StatusRisingValue._shortRangeAttackPower.ToString();
        //�����U���͂̑����ʂ�ݒ�
        _riseValueTexts[4].text = "�������U���͂̏㏸�l : " + equipment.ThisEquipment_StatusRisingValue._longRangeAttackPower.ToString();
        //�h��͂̑����ʂ�ݒ�
        _riseValueTexts[5].text = "�h��͂̏㏸�l : " + equipment.ThisEquipment_StatusRisingValue._defensePower.ToString();
        //�ړ����x�̑����ʂ�ݒ�
        _riseValueTexts[6].text = "�ړ����x�̏㏸�l : " + equipment.ThisEquipment_StatusRisingValue._moveSpeed.ToString();
        //������тɂ����̑����ʂ�ݒ�
        _riseValueTexts[7].text = "������тɂ����̏㏸�l : " + equipment.ThisEquipment_StatusRisingValue._difficultToBlowOff.ToString();
        //��������ݒ�
        _ExplanatoryTextArea.text = equipment._explanatoryText;
    }
}
