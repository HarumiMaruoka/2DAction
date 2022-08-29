using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary> �������Ă��鑕���̃{�^�����Ǘ�����N���X </summary>
public class ManagerOfPossessedEquipment : MonoBehaviour
{
    //<===== �����o�[�ϐ� =====>//
    [Header("�{�^���̐e�ƂȂ�ׂ��R���e���g"), SerializeField] Transform _content;
    [Header("�����{�^���̃v���n�u"), SerializeField] GameObject _equipmentButtonPrefab;
    [Header("�C�x���g�V�X�e��"), SerializeField] EventSystem _eventSystem;
    [Header("�����̏���\������e�L�X�g�̐e"), SerializeField] GameObject _equipmentInformationParents;
    [Header("�I�𒆂̑����̐�������\������G���A�̃Q�[���I�u�W�F�N�g"), SerializeField] Text _ExplanatoryTextArea;

    GameObject[] _equipmentButtons;
    GameObject _beforeSelectedGameObject;
    EquipmentButton _beforeEquipmentButton;
    Text[] _riseValueTexts;

    //<===== unity���b�Z�[�W =====>//
    void Start()
    {
        Initialized_ThisClass();
    }
    void Update()
    {
        Update_DrawEquipmentInformation();
    }

    //<===== private�����o�[�֐� =====>//
    /// <summary> ���̃N���X�̏������֐� </summary>
    void Initialized_ThisClass()
    {
        //�z�񕪂̃��������m��
        _equipmentButtons = new GameObject[EquipmentDataBase.Instance.MaxHaveValue];
        //�����ł��鐔�����{�^���𐶐����A�z��ɕۑ����Ă����B
        for (int i = 0; i < EquipmentDataBase.Instance.MaxHaveValue; i++)
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
        for (int i = 0; i < EquipmentDataBase.Instance.MaxHaveValue; i++)
        {
            Set_ValueToButton(i);
        }
    }
    /// <summary> ����̃{�^���ɑ�������ݒ肷��B </summary>
    /// <param name="index"> �ύX�������{�^���̃C���f�b�N�X </param>
    void Set_ValueToButton(int index)
    {
        // ���������̏���ۊǂ��Ă���ꏊ����A������ID���擾����B
        int thisEquipmentID = EquipmentDataBase.Instance.HaveEquipmentID._equipmentsID[index];
        // -1�Ȃ珊�����Ă��Ȃ��̂�null��ݒ肷��B�����łȂ���΁A�{�^���ɑ��������Z�b�g����B
        if (thisEquipmentID != -1) _equipmentButtons[index].GetComponent<EquipmentButton>().Set_Equipment(EquipmentDataBase.Instance.EquipmentData[thisEquipmentID]);
        else _equipmentButtons[index].GetComponent<EquipmentButton>().Set_Equipment(null);
    }
    /// <summary>�u��������v�{�^�����A�N�e�B�u�ɂ��� </summary>
    /// <param name="equipmentButton"> �Ώۂ́u�����v�{�^�� </param>
    void OnEnabled_EquipButton(EquipmentButton equipmentButton)
    {
        if (equipmentButton._myEquipment._myType != Equipment.EquipmentType.ARM_PARTS)
        {
            equipmentButton.OnEnabled_EquipButton_OtherArm();
        }
        else
        {
            equipmentButton.OnEnabled_EquipButton_LeftArm();
            equipmentButton.OnEnabled_EquipButton_RightArm();
        }
    }
    /// <summary>�u��������v�{�^�����A�N�e�B�u�ɂ��� </summary>
    /// <param name="equipmentButton"> �Ώۂ́u�����v�{�^�� </param>
    void OffEnabled_EquipButton(EquipmentButton equipmentButton)
    {
        if (equipmentButton._myEquipment._myType != Equipment.EquipmentType.ARM_PARTS)
        {
            equipmentButton.OffEnabled_EquipButton_OtherArm();
        }
        else
        {
            equipmentButton.OffEnabled_EquipButton_LeftArm();
            equipmentButton.OffEnabled_EquipButton_RightArm();
        }
    }
    //<===== public�����o�[�֐� =====>//
    /// <summary> �������̕\����؂�ւ���B </summary>
    public void Update_DrawEquipmentInformation()
    {
        // �O�t���[���ƍ��t���[���őI�����Ă���I�u�W�F�N�g���قȂ�ꍇ�ɏ��������s����B
        if (_beforeSelectedGameObject != _eventSystem.currentSelectedGameObject)
        {
            //�J�����g�I�u�W�F�N�g�̏���
            if (_eventSystem.currentSelectedGameObject != null)
            {
                //�u�����v�{�^���̏ꍇ
                if (_eventSystem.currentSelectedGameObject.TryGetComponent(out EquipmentButton currentEquipmentButton))
                {
                    Update_RiseValueText(currentEquipmentButton._myEquipment);

                    //���ݑI�𒆂̃p�[�c�́u��������v�{�^�����A�N�e�B�u�ɂ���B
                    OnEnabled_EquipButton(currentEquipmentButton);
                }
            }
            //�u��������v�{�^���̏ꍇ
            if (_eventSystem.currentSelectedGameObject.TryGetComponent(out EquipButton currentEquipButton))
            {
                //�����ɏ������L�q����B
            }

            //�O�t���[���őI������Ă����{�^���̏���
            if (_beforeSelectedGameObject != null)
            {
                //�u�����v�{�^���̏ꍇ
                if (_beforeSelectedGameObject.TryGetComponent(out EquipmentButton beforeEquipmentButton))
                {
                    if ((_beforeSelectedGameObject.transform.parent.gameObject != _eventSystem.currentSelectedGameObject) &&
                        (_eventSystem.currentSelectedGameObject.transform.parent.gameObject != _beforeSelectedGameObject))
                    {
                        //�e�q�֌W�łȂ���΁A�u��������v�{�^�����A�N�e�B�u�ɂ���B
                        OffEnabled_EquipButton(beforeEquipmentButton);
                    }
                }
                //�u��������v�{�^���̏ꍇ
                if (_beforeSelectedGameObject.TryGetComponent(out EquipButton beforeEquipButton))
                {
                    foreach (var button in beforeEquipButton.transform.parent.GetComponentsInChildren<EquipButton>())
                    {
                        if (button.gameObject.activeSelf) button.gameObject.SetActive(false);
                    }
                }
            }
        }

        //�Â��I�u�W�F�N�g��ۑ����Ă����B
        _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }
    /// <summary> �㏸�l�e�L�X�g���X�V���� </summary>
    /// <param name="equipment"> �㏸�l��\�����鑕�� </param>
    public void Update_RiseValueText(Equipment equipment)
    {
        //�����̎��
        _riseValueTexts[0].text = "�����̎�� : " + equipment._myTypeName;

        var parameter = equipment.ThisEquipment_StatusRisingValue;
        //�ő�̗͂̑����ʂ�ݒ�
        _riseValueTexts[1].text = $"�ő�̗͂̏㏸�l : {parameter._maxHp}";
        //�ő�X�^�~�i�̑����ʂ�ݒ�
        _riseValueTexts[2].text = $"�ő�X�^�~�i�̏㏸�l : {parameter._maxStamina}";
        //�ߋ����U���͂̑����ʂ�ݒ�
        _riseValueTexts[3].text = $"�ߋ����U���͂̏㏸�l : {parameter._shortRangeAttackPower}";
        //�����U���͂̑����ʂ�ݒ�
        _riseValueTexts[4].text = $"�������U���͂̏㏸�l : {parameter._longRangeAttackPower}";
        //�h��͂̑����ʂ�ݒ�
        _riseValueTexts[5].text = $"�h��͂̏㏸�l : {parameter._defensePower}";
        //�ړ����x�̑����ʂ�ݒ�
        _riseValueTexts[6].text = $"�ړ����x�̏㏸�l : {parameter._moveSpeed}";
        //������тɂ����̑����ʂ�ݒ�
        _riseValueTexts[7].text = $"������тɂ����̏㏸�l : {parameter._difficultToBlowOff}";
        //��������ݒ�
        _ExplanatoryTextArea.text = equipment._explanatoryText;
    }
}

