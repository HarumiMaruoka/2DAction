using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 装備の所持数を管理する基底クラス </summary>
public abstract class EquipmentBase : MonoBehaviour
{
    //<======= このクラスで使用する型 =======>//
    /// <summary> 装備のID </summary>
    public enum EquipmentID
    {
        ID_0,
        ID_1,
        ID_2,
        ID_3,

        ID_END,
    }
    /// <summary> 現在装備している装備を表す構造体 </summary>
    struct MyEquipped
    {
        Equipment _head;
        Equipment _torso;
        Equipment _arm;
        Equipment _foot;
    }
    struct HaveEquipped
    {
        public List<Equipment> _equipments;
    }

    //<=========== 必要な値 ===========>//
    /// <summary> 全ての装備の情報を一時保存しておく変数 </summary>
    Equipment[] _equipmentData;
    /// <summary> 所持している装備のリスト </summary>
    HaveEquipped _haveEquipment;
    /// <summary> 現在装備している装備 </summary>
    MyEquipped _myEquipped;

    //<======== アサインすべき値 ========>//


    //<===== インスペクタから設定すべき値 =====>//


    /// <summary> 装備基底クラスの初期化関数。(派生先で呼び出す。) </summary>
    /// <returns> 初期化に成功した場合true、失敗したらfalseを返す。 </returns>
    protected bool Initialize_EquipmentBase()
    {

        return true;
    }

    /// <summary> csvファイルから、全ての装備のデータを読み込む関数 </summary>
    /// <returns> 読み込んだ結果を返す。失敗した場合はnullを返す。 </returns>
    void OnLoad_EquipmentData_csv()
    {
        _equipmentData = new Equipment[(int)EquipmentID.ID_END];

    }

    /// <summary> 所持している装備を、jsonファイルからデータを読み込み、メンバー変数に格納する処理。 </summary>
    public void OnLoad_EquipmentData_Json()
    {
        _haveEquipment._equipments = new List<Equipment>();
    }

    /// <summary> 所持している装備のデータを、jsonファイルに保存する処理。 </summary>
    public void OnSave_EquipmentData_Json()
    {

    }
}
