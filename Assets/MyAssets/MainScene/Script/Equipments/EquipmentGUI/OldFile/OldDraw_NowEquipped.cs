using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 現在装備しているパーツを表示するコンポーネント </summary>
public class OldDraw_NowEquipped : UseEventSystemBehavior
{
    const int REFT_ARM = 0;
    const int RIGHT_ARM = 1;
    //<==========インスペクタから設定すべき値==========>//
    [Header("体全体を表示する場所"), SerializeField] Image _bodyPartsImageArea;
    [Header("頭パーツの情報を表示する場所"), SerializeField] Image _headPartsImageArea;
    [Header("胴パーツの情報を表示する場所"), SerializeField] Image _torsoPartsImageArea;
    [Header("腕パーツの情報を表示する場所"), SerializeField] Image _armLeftPartsImageArea;
    [Header("腕パーツの情報を表示する場所"), SerializeField] Image _armRightPartsImageArea;
    [Header("足パーツの情報を表示する場所"), SerializeField] Image _footPartsImageArea;

    //今はアイコン用画像を用意するのが面倒くさいのでテキストで表示する。
    [Header("体全体を表示する場所(テキスト版) : テスト用"), SerializeField] Text _bodyPartsTextArea;
    [Header("頭パーツの情報を表示する場所のテキスト(テキスト版) : テスト用"), SerializeField] Text _headPartsTextArea;
    [Header("胴パーツの情報を表示する場所のテキスト(テキスト版) : テスト用"), SerializeField] Text _torsoPartsTextArea;
    [Header("左腕パーツの情報を表示する場所のテキスト(テキスト版) : テスト用"), SerializeField] Text _armLeftPartsTextArea;
    [Header("右腕パーツの情報を表示する場所のテキスト(テキスト版) : テスト用"), SerializeField] Text _armRightPartsTextArea;
    [Header("足パーツの情報を表示する場所のテキスト(テキスト版) : テスト用"), SerializeField] Text _footPartsTextArea;

    protected override void Start()
    {
        base.Start();
        Update_EquippedALL();
    }

    /// <summary> 着用している装備の表示を更新する。 </summary>
    void Update_EquippedALL()
    {
        Update_Equipped(Equipment.EquipmentType.HEAD_PARTS);//頭
        Update_Equipped(Equipment.EquipmentType.TORSO_PARTS);//胴
        Update_Equipped(Equipment.EquipmentType.ARM_PARTS, Constants.RIGHT_ARM);//右腕
        Update_Equipped(Equipment.EquipmentType.ARM_PARTS, Constants.LEFT_ARM);//左腕
        Update_Equipped(Equipment.EquipmentType.FOOT_PARTS);//足
    }

    /// <summary> 装備の描画を更新 </summary>
    /// <param name="updateType"> どこを更新するか </param>
    /// <param name="whichArm"> 腕の場合左腕か右腕か。0なら左腕を更新し、1なら右腕を更新する。その他の値は不正。 </param>
    public void Update_Equipped(Equipment.EquipmentType updateType, int whichArm = -1)
    {
        var currentEquipped = EquipmentManager.Instance.CurrentEquippedData.Equipped;
        var equipmentData = EquipmentManager.Instance.NewEquipmentDataBase.EquipmentData;
        // 腕以外の装備を更新する場合の処理。
        // 装備マネージャーが現在着用している装備を知っているので、そこから情報を取得し、各コンポーネントに適用する。
        if (updateType != Equipment.EquipmentType.ARM_PARTS)
        {
            switch (updateType)
            {
                case Equipment.EquipmentType.HEAD_PARTS:
                    if (currentEquipped._headPartsID >= 0)
                        _headPartsTextArea.text = 
                            equipmentData[(int)currentEquipped._headPartsID]._myName;
                    else
                        _headPartsTextArea.text = "未装備";
                    break;
                case Equipment.EquipmentType.TORSO_PARTS:
                    if (currentEquipped._torsoPartsID >= 0)
                        _torsoPartsTextArea.text = 
                            equipmentData[(int)currentEquipped._torsoPartsID]._myName;
                    else
                        _torsoPartsTextArea.text = "未装備";
                    break;
                case Equipment.EquipmentType.FOOT_PARTS:
                    if (currentEquipped._footPartsID >= 0)
                        _footPartsTextArea.text = 
                            equipmentData[(int)currentEquipped._footPartsID]._myName;
                    else
                        _footPartsTextArea.text = "未装備";
                    break;
                default: Debug.LogError("不正な値です。"); break;
            }
        }
        //腕の装備を更新
        else
        {
            //左腕の場合
            if (whichArm == Constants.LEFT_ARM)
            {
                if (currentEquipped._armLeftPartsID >= 0)
                    _armLeftPartsTextArea.text = 
                        equipmentData[(int)currentEquipped._armLeftPartsID]._myName;
                else
                    _armLeftPartsTextArea.text = "未装備";
            }
            //右腕の場合
            else if (whichArm == Constants.RIGHT_ARM)
            {
                if (currentEquipped._armRightPartsID >= 0)
                    _armRightPartsTextArea.text = 
                        equipmentData[(int)currentEquipped._armRightPartsID]._myName;
                else
                    _armRightPartsTextArea.text = "未装備";
            }
            else
            {
                Debug.LogError($"不正な値です。{whichArm}");
            }
        }
    }
}
