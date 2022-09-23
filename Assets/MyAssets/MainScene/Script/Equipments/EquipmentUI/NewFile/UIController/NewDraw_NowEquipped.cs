using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> 現在装備しているパーツを表示するコンポーネント </summary>
public class NewDraw_NowEquipped : UseEventSystemBehavior
{
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

    //===== Unityメッセージ =====//
    protected override void Start()
    {
        base.Start();
        Update_EquippedALL();
    }
    void OnEnable()
    {
        EquipmentUIUpdateManager.SwapEquipmentUpdate += Update_EquippedALL;
    }
    void OnDisable()
    {
        EquipmentUIUpdateManager.SwapEquipmentUpdate += Update_EquippedALL;
    }

    //===== メソッド =====//
    /// <summary> 着用している装備の表示を更新する。 </summary>
    void Update_EquippedALL()
    {
        Update_Equipped(Equipment.EquipmentType.HEAD_PARTS);                     // 頭
        Update_Equipped(Equipment.EquipmentType.TORSO_PARTS);                    // 胴
        Update_Equipped(Equipment.EquipmentType.ARM_PARTS, Constants.RIGHT_ARM); // 右腕
        Update_Equipped(Equipment.EquipmentType.ARM_PARTS, Constants.LEFT_ARM);  // 左腕
        Update_Equipped(Equipment.EquipmentType.FOOT_PARTS);                     // 足
    }

    /// <summary> 
    /// 装備の描画を更新<br/>
    /// 更新のタイミング : <br/>
    /// 着用している装備の交換が行われた時に実行する。
    /// </summary>
    /// <param name="updateType"> どこを更新するか </param>
    /// <param name="whichArm"> 腕の場合左腕か右腕か。</param>
    public void Update_Equipped(Equipment.EquipmentType updateType, int whichArm = Constants.NOT_ARM)
    {
        // 腕以外の装備を更新する場合の処理。
        // 装備マネージャーが現在着用している装備を知っているので、そこから情報を取得し、各コンポーネントに適用する。
        if (updateType != Equipment.EquipmentType.ARM_PARTS)
        {
            switch (updateType)
            {
                // 頭の場合
                case Equipment.EquipmentType.HEAD_PARTS:
                    if (EquipmentManager.Instance.CurrentEquippedData.Equipped._headPartsID != EquipmentID.None)
                        _headPartsTextArea.text =
                            EquipmentManager.Instance.NewEquipmentDataBase.
                            EquipmentData[(int)EquipmentManager.Instance.CurrentEquippedData.Equipped._headPartsID]._myName;
                    else
                        _headPartsTextArea.text = "未装備";
                    break;
                // 胴の場合
                case Equipment.EquipmentType.TORSO_PARTS:
                    if (EquipmentManager.Instance.CurrentEquippedData.Equipped._torsoPartsID != EquipmentID.None)
                        _torsoPartsTextArea.text =
                            EquipmentManager.Instance.NewEquipmentDataBase.
                            EquipmentData[(int)EquipmentManager.Instance.CurrentEquippedData.Equipped._torsoPartsID]._myName;
                    else
                        _torsoPartsTextArea.text = "未装備";
                    break;
                // 足の場合
                case Equipment.EquipmentType.FOOT_PARTS:
                    if (EquipmentManager.Instance.CurrentEquippedData.Equipped._footPartsID != EquipmentID.None)
                        _footPartsTextArea.text =
                            EquipmentManager.Instance.NewEquipmentDataBase.
                            EquipmentData[(int)EquipmentManager.Instance.CurrentEquippedData.Equipped._footPartsID]._myName;
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
                if (EquipmentManager.Instance.CurrentEquippedData.Equipped._armLeftPartsID != EquipmentID.None)
                    _armLeftPartsTextArea.text =
                        EquipmentManager.Instance.NewEquipmentDataBase.
                            EquipmentData[(int)EquipmentManager.Instance.CurrentEquippedData.Equipped._armLeftPartsID]._myName;
                else
                    _armLeftPartsTextArea.text = "未装備";
            }
            //右腕の場合
            else if (whichArm == Constants.RIGHT_ARM)
            {
                if (EquipmentManager.Instance.CurrentEquippedData.Equipped._armRightPartsID != EquipmentID.None)
                    _armRightPartsTextArea.text =
                        EquipmentManager.Instance.NewEquipmentDataBase.
                            EquipmentData[(int)EquipmentManager.Instance.CurrentEquippedData.Equipped._armRightPartsID]._myName;
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
