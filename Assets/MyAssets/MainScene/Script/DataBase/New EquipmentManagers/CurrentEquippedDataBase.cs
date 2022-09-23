using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary> 
/// 新しいクラス : まだ実装してない<br/>
/// 現在着用している装備を管理するクラス : <br/>
/// 現在着用している装備の情報を提供する。<br/>
/// 現在着用している装備をjsonから書き込み/読み込みする機能を提供する。<br/>
/// </summary>
public class CurrentEquippedDataBase
{
    //===== このクラスで使用する型 =====//
    /// <summary> 現在装着している装備を表す構造体 </summary>
    public struct MyEquipped
    {
        /// <summary> 頭に装着しているパーツ </summary>
        public EquipmentID _headPartsID;
        /// <summary> 胴に装備しているパーツ </summary>
        public EquipmentID _torsoPartsID;
        /// <summary> 右腕に装着しているパーツ </summary>
        public EquipmentID _armRightPartsID;
        /// <summary> 左腕に装着しているパーツ </summary>
        public EquipmentID _armLeftPartsID;
        /// <summary> 足に装着しているパーツ </summary>
        public EquipmentID _footPartsID;
    }

    //===== フィールド / プロパティ =====//
    /// <summary> 現在着用している装備を表すフィールド </summary>
    MyEquipped _equipped;
    /// <summary> 現在着用している装備のフィールド </summary>
    public MyEquipped Equipped { get => _equipped; }
    /// <summary> 着用しているパーツを保存しているファイルへのパス </summary>
    string _equippedJsonFilePath;

    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns>
    /// 初期化に成功した場合 true を返し、<br/>
    /// 初期化に失敗した場合 false を返す。<br/>
    /// </returns>
    bool Init()
    {
        // 着用している装備を保存しているファイルのパスを取得する。
        _equippedJsonFilePath = Path.Combine(Application.persistentDataPath, "EquippedFile.json");

        // 現在の着用している装備を初期化する。
        _equipped._headPartsID = EquipmentID.None;
        _equipped._torsoPartsID = EquipmentID.None;
        _equipped._armRightPartsID = EquipmentID.None;
        _equipped._armLeftPartsID = EquipmentID.None;
        _equipped._footPartsID = EquipmentID.None;

        return true;
    }
    /// <summary> "着用"している装備をjsonファイルから取得し、メンバー変数に格納する処理。 </summary>
    public void OnLoad_EquippedData_FromJson()
    {
        // 念のためファイルの存在チェック
        if (!File.Exists(_equippedJsonFilePath))
        {
            //ここにファイルが無い場合の処理を書く
            Debug.Log("現在装備している装備データを保存しているファイルが見つかりません。");

            //処理を抜ける
            return;
        }
        // JSONオブジェクトを、デシリアライズ(C#形式に変換)し、値をセット。
        _equipped = JsonUtility.FromJson<MyEquipped>(File.ReadAllText(_equippedJsonFilePath));
    }
    /// <summary> "着用"している装備のデータを、jsonファイルに保存する処理。 </summary>
    public void OnSave_EquippedData_FromJson()
    {
        Debug.Log("現在装備している装備データをセーブします。");
        // 現在装備している装備データを、JSON形式にシリアライズし、ファイルに保存する。
        File.WriteAllText(_equippedJsonFilePath, JsonUtility.ToJson(_equipped, false));
    }
    /// <summary>
    /// 着用している装備を変更する
    /// </summary>
    /// <param name="equipmentType"> 変更を加える箇所 </param>
    /// <param name="armType"> 腕の場合どちらか </param>
    /// <param name="changeID"> 変更するID </param>
    public void ChangeEquipped(Equipment.EquipmentType equipmentType, EquipmentID changeID = EquipmentID.None, int armType = Constants.NOT_ARM)
    {
        // 値が正しいかチェック
        if (changeID == EquipmentID.None)
        {
            Debug.LogWarning(
                $"装備箇所\"{equipmentType}\"に対して\"{EquipmentID.None}\"を代入します。\n" +
                $"つまり、装備箇所\"{equipmentType}\"を未装備にします。\n");
        }

        // 現在の装備を取得
        var changeValue = _equipped;
        // 変更を加える
        switch (equipmentType)
        {
            case Equipment.EquipmentType.HEAD_PARTS:
                changeValue._headPartsID = changeID;
                break;
            case Equipment.EquipmentType.TORSO_PARTS:
                changeValue._torsoPartsID = changeID;
                break;
            case Equipment.EquipmentType.ARM_PARTS:
                if (armType == Constants.LEFT_ARM)
                {
                    changeValue._armLeftPartsID = changeID;
                }
                else if (armType == Constants.RIGHT_ARM)
                {
                    changeValue._armRightPartsID = changeID;
                }
                else
                {
                    Debug.LogError("不正な値です！");
                }
                break;
            case Equipment.EquipmentType.FOOT_PARTS:
                changeValue._footPartsID = changeID;
                break;
            default: Debug.Log("不正な値です！"); break;
        }
        _equipped = changeValue;
    }
}