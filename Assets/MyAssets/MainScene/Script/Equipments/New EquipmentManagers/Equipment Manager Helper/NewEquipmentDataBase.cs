using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary> 
/// 全ての装備の情報を管理するクラス。<br/>
/// 全ての装備の情報を提供する。<br/>
/// </summary>
public class NewEquipmentDataBase
{
    //===== フィールド / プロパティ =====//
    // エディター拡張を作る。
    // Resourceのフォルダに入れる。
    // テキストアセット化する。
    // パーシステントデータパス
    /// <summary> 装備情報を格納しているファイルへのパス </summary>
    const string _csvFilePath = @"C:\Users\vantan\Desktop\2DActionBasicInformation\EquipmentDataBase.csv";
    /// <summary> 全ての装備の情報を保存しておくフィールド </summary>
    Equipment[] _equipmentData;
    /// <summary> 全ての装備の情報が保存されたプロパティ </summary>
    public Equipment[] EquipmentData { get => _equipmentData; }

    //コンストラクタ
    public NewEquipmentDataBase()
    {
        if (!Init())
        {
            Debug.LogError($"初期化に失敗しました");
        }
    }

    //===== methods =====//
    /// <summary>
    /// 初期化処理
    /// </summary>
    /// <returns> 初期化に成功した場合true、失敗したらfalseを返す。 </returns>
    bool Init()
    {
        // csvファイルから全ての装備のデータを読み込む。
        OnLoad_EquipmentData_csv();

        return true;
    }

    /// <summary> 
    /// csvファイルから、全ての装備のデータを読み込みメンバー変数に保存する。<br/>
    /// </summary>
    /// <returns> 読み込みに成功したらtrue,失敗した場合はfalseを返す。 </returns>
    void OnLoad_EquipmentData_csv()
    {
        if (_equipmentData == null) _equipmentData = new Equipment[(int)EquipmentID.ID_END];

        int index = 0;
        bool isFirstLine = true;//一行目かどうかを判断する値
        //CSVファイルからアイテムデータを読み込み、配列に保存する
        StreamReader sr = new StreamReader(_csvFilePath);//ファイルを開く
        while (!sr.EndOfStream)// 末尾まで繰り返す
        {
            string[] values = sr.ReadLine().Split(',');//一行読み込み区切って保存する
            //最初の行(ヘッダーの行)はスキップする
            if (isFirstLine)
            {
                isFirstLine = false;
                continue;
            }
            //種類別で生成し保存する
            switch (values[1])
            {
                //頭用装備を取得し保存
                case "Head":
                    _equipmentData[index] = new HeadParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.HEAD_PARTS,//Type
           values[2],//name
           Conversion_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           float.Parse(values[12]),
           values[13],
           values[14]); break;
                //胴用装備を取得し保存
                case "Torso":
                    _equipmentData[index] = new TorsoParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.TORSO_PARTS,//Type
           values[2],//name
           Conversion_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           float.Parse(values[12]),
           values[13],
           values[14]); break;
                //腕用装備を取得し保存
                case "Arm":
                    _equipmentData[index] = new ArmParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.ARM_PARTS,//Type
           values[2],//name
           Conversion_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           float.Parse(values[12]),
           values[13],
           values[14],
           ArmParts.Get_AttackType(values[15]),
           float.Parse(values[16])); break;
                //足用装備を取得し保存
                case "Foot":
                    _equipmentData[index] = new FootParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.FOOT_PARTS,//Type
           values[2],//name
           Conversion_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           float.Parse(values[12]),
           values[13],
           values[14]); break;
                default: Debug.LogError("設定されていないEquipmentTypeです。"); break;
            }
            index++;
        }

    }
    /// <summary> レアリティを表す string を enum に変換する。 </summary>
    /// <param name="str"> 対象の文字列 </param>
    /// <returns></returns>
    Equipment.EquipmentRarity Conversion_EquipmentRarity(string str)
    {
        switch (str)
        {
            case "A": return Equipment.EquipmentRarity.A;
            case "B": return Equipment.EquipmentRarity.B;
            case "C": return Equipment.EquipmentRarity.C;
            case "D": return Equipment.EquipmentRarity.D;
            case "E": return Equipment.EquipmentRarity.E;
        }
        Debug.LogError("不正な値です。");
        return Equipment.EquipmentRarity.ERROR;
    }
}