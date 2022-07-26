using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> ���ݑ������Ă���p�[�c��\������N���X </summary>
public class Draw_NowEquipped : MonoBehaviour
{
    //<==========�C���X�y�N�^����ݒ肷�ׂ��l==========>//
    [Header("�̂�\������ꏊ"), SerializeField] Image _bodyPartsImageArea;
    [Header("���p�[�c�̏���\������ꏊ"), SerializeField] Image _headPartsImageArea;
    [Header("���p�[�c�̏���\������ꏊ"), SerializeField] Image _torsoPartsImageArea;
    [Header("�r�p�[�c�̏���\������ꏊ"), SerializeField] Image _armLeftPartsImageArea;
    [Header("�r�p�[�c�̏���\������ꏊ"), SerializeField] Image _armRightPartsImageArea;
    [Header("���p�[�c�̏���\������ꏊ"), SerializeField] Image _footPartsImageArea;

    [Header("�̂�\������ꏊ : �e�X�g�p"), SerializeField] Text _bodyPartsTextArea;
    [Header("���p�[�c�̏���\������ꏊ�̃e�L�X�g : �e�X�g�p"), SerializeField] Text _headPartsTextArea;
    [Header("���p�[�c�̏���\������ꏊ�̃e�L�X�g : �e�X�g�p"), SerializeField] Text _torsoPartsTextArea;
    [Header("���p�[�c�̏���\������ꏊ�̃e�L�X�g : �e�X�g�p"), SerializeField] Text _armLeftPartsTextArea;
    [Header("���p�[�c�̏���\������ꏊ�̃e�L�X�g : �e�X�g�p"), SerializeField] Text _armRightPartsTextArea;
    [Header("���p�[�c�̏���\������ꏊ�̃e�L�X�g : �e�X�g�p"), SerializeField] Text _footPartsTextArea;

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary> ���p���Ă��鑕���̕\�����X�V����B </summary>
    void Update_EquippedALL()
    {
        _headPartsTextArea.text     = EquipmentManager.Instance.EquipmentData[EquipmentManager.Instance.Equipped._headPartsID]     ._myName;
        _torsoPartsTextArea.text    = EquipmentManager.Instance.EquipmentData[EquipmentManager.Instance.Equipped._torsoPartsID]    ._myName;
        _armLeftPartsTextArea.text  = EquipmentManager.Instance.EquipmentData[EquipmentManager.Instance.Equipped._armLeftPartsID]  ._myName;
        _armRightPartsTextArea.text = EquipmentManager.Instance.EquipmentData[EquipmentManager.Instance.Equipped._armRightPartsID] ._myName;
        _footPartsTextArea.text     = EquipmentManager.Instance.EquipmentData[EquipmentManager.Instance.Equipped._footPartsID]     ._myName;
    }

    void Update_Equipped()
    {

    }
}
