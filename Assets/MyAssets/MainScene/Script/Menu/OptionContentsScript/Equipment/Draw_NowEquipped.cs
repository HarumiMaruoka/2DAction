using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 現在装備しているパーツを表示するクラス </summary>
public class Draw_NowEquipped : MonoBehaviour
{
    //<==========インスペクタから設定すべき値==========>//
    [Header("体を表示する場所"), SerializeField] Image _bodyPartsImageArea;
    [Header("頭パーツの情報を表示する場所"), SerializeField] Image _headPartsImageArea;
    [Header("胴パーツの情報を表示する場所"), SerializeField] Image _torsoPartsImageArea;
    [Header("腕パーツの情報を表示する場所"), SerializeField] Image _armLeftPartsImageArea;
    [Header("腕パーツの情報を表示する場所"), SerializeField] Image _armRightPartsImageArea;
    [Header("足パーツの情報を表示する場所"), SerializeField] Image _footPartsImageArea;

    [Header("体を表示する場所 : テスト用"), SerializeField] Text _bodyPartsTextArea;
    [Header("頭パーツの情報を表示する場所のテキスト : テスト用"), SerializeField] Text _headPartsTextArea;
    [Header("頭パーツの情報を表示する場所のテキスト : テスト用"), SerializeField] Text _torsoPartsTextArea;
    [Header("頭パーツの情報を表示する場所のテキスト : テスト用"), SerializeField] Text _armLeftPartsTextArea;
    [Header("頭パーツの情報を表示する場所のテキスト : テスト用"), SerializeField] Text _armRightPartsTextArea;
    [Header("頭パーツの情報を表示する場所のテキスト : テスト用"), SerializeField] Text _footPartsTextArea;

    void Start()
    {

    }

    void Update()
    {

    }

    /// <summary> 着用している装備の表示を更新する。 </summary>
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
