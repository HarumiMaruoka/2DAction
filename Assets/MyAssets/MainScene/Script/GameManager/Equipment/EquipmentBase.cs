using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EquipmentBase : MonoBehaviour
{
    public enum EquipmentID
    {
        ID_0,
        ID_1,

        ID_END,
    }

    //<=========== 必要な値 ===========>//
    Equipment[] _equipmentData = new Equipment[(int)EquipmentID.ID_END];

    //<======== アサインすべき値 ========>//


    //<===== インスペクタから設定すべき値 =====>//


    /// <summary> 装備基底クラスの初期化関数。(派生先で呼び出す。) </summary>
    /// <returns> 初期化に成功した場合true、失敗したらfalseを返す。 </returns>
    protected bool Initialize_EquipmentBase()
    {

        return true;
    }

    /// <summary> csvファイルからデータを読み込む関数 </summary>
    /// <returns> 読み込んだ結果を返す。失敗した場合はnullを返す。 </returns>
    Equipment[] OnLoad_EquipmentData_csv()
    {
        Equipment[] result = new Equipment[(int)EquipmentID.ID_END];

        return result;
    }

    /// <summary> jsonファイルからデータを読み込み、メンバー変数に格納する処理。 </summary>
    public void OnLoad_EquipmentData_Json()
    {

    }

    /// <summary> 所持している装備のデータを、jsonファイルに保存する処理。 </summary>
    public void OnSave_EquipmentData_Json()
    {

    }
}
