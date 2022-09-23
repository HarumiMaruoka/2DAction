using System.IO;
using UnityEngine;

/// <summary> 
/// まだ実装していない。<br/>
/// プレイヤーが所持している装備を管理するクラス : <br/>
/// プレイヤーが所持している装備の情報を提供する。<br/>
/// プレイヤーが所持している装備の情報をjsonファイルに書き込んだり<br/>
/// jsonファイルから読み込んだりする機能を提供する。<br/>
/// </summary>
public class HaveEquipmentDataBase
{
    //===== このクラスで使用する型 =====//
    /// <summary> 
    /// 所持している装備を格納する構造体 :<br/>
    /// jsonファイルに保存する為に作成された型<br/>
    /// </summary>
    public struct HaveEquipped
    {
        /// <summary> 要素は装備のID。所持していなければ-1。 </summary>
        public int[] _equipmentsID;
    }

    //===== フィールド / プロパティ =====//
    /// <summary> 所持している装備を表すフィールド </summary>
    HaveEquipped _haveEquipmentID;
    /// <summary> 所持している装備のプロパティ </summary>
    public HaveEquipped HaveEquipmentID { get => _haveEquipmentID; }
    /// <summary> プレイヤーが所持できるパーツの最大数 </summary>
    const int _maxHaveVolume = 20;
    /// <summary> 所持している装備のデータを保存しているファイルへのパス </summary>
    const string _equipmentHaveJsonFilePath = "";

    // コンストラクタ
    public HaveEquipmentDataBase()
    {

    }

    //===== methods =====//
    /// <summary>
    /// このクラスの初期化処理
    /// </summary>
    void Init()
    {
        // 所持できる装備分の配列用のメモリを確保する。
        _haveEquipmentID._equipmentsID = new int[_maxHaveVolume];
        // 配列を初期化する。
        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length; i++)
        {
            _haveEquipmentID._equipmentsID[i] = -1;
        }
    }
    /// <summary> 
    /// "所持"している装備の情報をjsonファイルから読み取りメンバー変数に格納する。<br/>
    /// </summary>
    public void OnLoad_EquipmentHaveData_Json()
    {
        //Debug.Log("所持している装備データをロードします！");
        // 念のためファイルの存在チェック
        if (!File.Exists(_equipmentHaveJsonFilePath))
        {
            //ここにファイルが無い場合の処理を書く
            Debug.Log("所持している装備データを保存しているファイルが見つかりません。");

            //処理を抜ける
            return;
        }
        // ファイルが存在する場合の処理。
        // JSONオブジェクトを、デシリアライズ(C#形式に変換)し、値をメンバー変数にセットする。
        _haveEquipmentID = JsonUtility.FromJson<HaveEquipped>(File.ReadAllText(_equipmentHaveJsonFilePath));
    }
    /// <summary> 
    /// "所持"している装備のデータをjsonファイルに保存する。<br/>
    /// </summary>
    public void OnSave_EquipmentHaveData_Json()
    {
        Debug.Log("所持している装備データをセーブします！");
        // 所持している装備データを、JSON形式にシリアライズし、jsonファイルに保存
        File.WriteAllText(_equipmentHaveJsonFilePath, JsonUtility.ToJson(_haveEquipmentID, false));
    }
    /// <summary> 装備を取得する。 </summary>
    /// <param name="getEquipmentsID"> 取得する装備のID </param>
    public void GetEquipment(int getEquipmentsID)
    {
        // 空のスロットを見つけ、そこに保存する。
        for(int i=0;i< _haveEquipmentID._equipmentsID.Length; i++)
        {
            if(_haveEquipmentID._equipmentsID[i]== (int)EquipmentID.None)
            {
                _haveEquipmentID._equipmentsID[i] = getEquipmentsID;
                Debug.Log("取得に成功しました。");
                break;
            }
        }
        Debug.LogWarning("取得に失敗しました。");
    }
}