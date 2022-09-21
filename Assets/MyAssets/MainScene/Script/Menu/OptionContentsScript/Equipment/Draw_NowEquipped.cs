using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> ���ݑ������Ă���p�[�c��\������R���|�[�l���g </summary>
public class Draw_NowEquipped : EquipmentUIBase
{
    const int REFT_ARM = 0;
    const int RIGHT_ARM = 1;
    //<==========�C���X�y�N�^����ݒ肷�ׂ��l==========>//
    [Header("�̑S�̂�\������ꏊ"), SerializeField] Image _bodyPartsImageArea;
    [Header("���p�[�c�̏���\������ꏊ"), SerializeField] Image _headPartsImageArea;
    [Header("���p�[�c�̏���\������ꏊ"), SerializeField] Image _torsoPartsImageArea;
    [Header("�r�p�[�c�̏���\������ꏊ"), SerializeField] Image _armLeftPartsImageArea;
    [Header("�r�p�[�c�̏���\������ꏊ"), SerializeField] Image _armRightPartsImageArea;
    [Header("���p�[�c�̏���\������ꏊ"), SerializeField] Image _footPartsImageArea;

    //���̓A�C�R���p�摜��p�ӂ���̂��ʓ|�������̂Ńe�L�X�g�ŕ\������B
    [Header("�̑S�̂�\������ꏊ(�e�L�X�g��) : �e�X�g�p"), SerializeField] Text _bodyPartsTextArea;
    [Header("���p�[�c�̏���\������ꏊ�̃e�L�X�g(�e�L�X�g��) : �e�X�g�p"), SerializeField] Text _headPartsTextArea;
    [Header("���p�[�c�̏���\������ꏊ�̃e�L�X�g(�e�L�X�g��) : �e�X�g�p"), SerializeField] Text _torsoPartsTextArea;
    [Header("���r�p�[�c�̏���\������ꏊ�̃e�L�X�g(�e�L�X�g��) : �e�X�g�p"), SerializeField] Text _armLeftPartsTextArea;
    [Header("�E�r�p�[�c�̏���\������ꏊ�̃e�L�X�g(�e�L�X�g��) : �e�X�g�p"), SerializeField] Text _armRightPartsTextArea;
    [Header("���p�[�c�̏���\������ꏊ�̃e�L�X�g(�e�L�X�g��) : �e�X�g�p"), SerializeField] Text _footPartsTextArea;

    protected override void Start()
    {
        base.Start();
        Update_EquippedALL();
    }

    void Update()
    {

    }

    /// <summary> ���p���Ă��鑕���̕\�����X�V����B </summary>
    void Update_EquippedALL()
    {
        Update_Equipped(Equipment.EquipmentType.HEAD_PARTS);//��
        Update_Equipped(Equipment.EquipmentType.TORSO_PARTS);//��
        Update_Equipped(Equipment.EquipmentType.ARM_PARTS, REFT_ARM);//���r
        Update_Equipped(Equipment.EquipmentType.ARM_PARTS, RIGHT_ARM);//�E�r
        Update_Equipped(Equipment.EquipmentType.FOOT_PARTS);//��
    }

    /// <summary> �����̕`����X�V </summary>
    /// <param name="updateType"> �ǂ����X�V���邩 </param>
    /// <param name="whichArm"> �r�̏ꍇ���r���E�r���B0�Ȃ獶�r���X�V���A1�Ȃ�E�r���X�V����B���̑��̒l�͕s���B </param>
    public void Update_Equipped(Equipment.EquipmentType updateType, int whichArm = -1)
    {
        // �r�ȊO�̑������X�V����ꍇ�̏����B
        // �����}�l�[�W���[�����ݒ��p���Ă��鑕����m���Ă���̂ŁA������������擾���A�e�R���|�[�l���g�ɓK�p����B
        if (updateType != Equipment.EquipmentType.ARM_PARTS)
        {
            switch (updateType)
            {
                case Equipment.EquipmentType.HEAD_PARTS:
                    if (EquipmentDataBase.Instance.Equipped._headPartsID >= 0)
                        _headPartsTextArea.text = EquipmentDataBase.Instance.EquipmentData[EquipmentDataBase.Instance.Equipped._headPartsID]._myName;
                    else
                        _headPartsTextArea.text = "������";
                    break;
                case Equipment.EquipmentType.TORSO_PARTS:
                    if (EquipmentDataBase.Instance.Equipped._torsoPartsID >= 0)
                        _torsoPartsTextArea.text = EquipmentDataBase.Instance.EquipmentData[EquipmentDataBase.Instance.Equipped._torsoPartsID]._myName;
                    else
                        _torsoPartsTextArea.text = "������";
                    break;
                case Equipment.EquipmentType.FOOT_PARTS:
                    if (EquipmentDataBase.Instance.Equipped._footPartsID >= 0)
                        _footPartsTextArea.text = EquipmentDataBase.Instance.EquipmentData[EquipmentDataBase.Instance.Equipped._footPartsID]._myName;
                    else
                        _footPartsTextArea.text = "������";
                    break;
                default: Debug.LogError("�s���Ȓl�ł��B"); break;
            }
        }
        //�r�̑������X�V
        else
        {
            //���r�̏ꍇ
            if (whichArm == 0)
            {
                if (EquipmentDataBase.Instance.Equipped._armLeftPartsID >= 0)
                    _armLeftPartsTextArea.text = EquipmentDataBase.Instance.EquipmentData[EquipmentDataBase.Instance.Equipped._armLeftPartsID]._myName;
                else
                    _armLeftPartsTextArea.text = "������";
            }
            //�E�r�̏ꍇ
            else if (whichArm == 1)
            {
                if (EquipmentDataBase.Instance.Equipped._armRightPartsID >= 0)
                    _armRightPartsTextArea.text = EquipmentDataBase.Instance.EquipmentData[EquipmentDataBase.Instance.Equipped._armRightPartsID]._myName;
                else
                    _armRightPartsTextArea.text = "������";
            }
            else
            {
                Debug.LogError($"�s���Ȓl�ł��B{whichArm}");
            }
        }
    }
}
