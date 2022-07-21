using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary> 持っている装備を管理するクラス </summary>
public class EquipmentManager : MonoBehaviour
{
    //<======= このクラスで使用する型 =======>//
    /// <summary> 装備のID </summary>
    public enum EquipmentID
    {
        ID_0,
        ID_1,
        ID_2,
        ID_3,
        ID_4,
        ID_5,
        ID_6,
        ID_7,
        ID_8,
        ID_9,

        ID_10,
        ID_11,

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
    /// <summary> 所持している装備を格納する構造体 </summary>
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
    [Header("装備の基本情報が格納されたcsvファイルへのパス"), SerializeField] string _equipmentCsvFilePath;
    [Header("所持している装備の情報が格納されたjsonファイルへのパス"), SerializeField] string _equipmentHaveJsonFilePath;
    [Header("現在装備している装備の情報が格納されたjsonファイルへのパス"), SerializeField] string _equippedJsonFilePath;
    /// <summary> プレイヤーが所持できる装備の最大数 </summary>
    [Header("プレイヤーが所持できる装備の最大数"), SerializeField] int _maxHaveValue;


    //<======シングルトンパターン関連======>//
    private static EquipmentManager _instance;
    public static EquipmentManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PlayerManager._instanceはnullです。");
            }
            return _instance;
        }

    }
    private EquipmentManager() { }

    private void Awake()
    {
        //もしインスタンスが設定されていなかったら自身を代入する
        if (_instance == null)
        {
            _instance = this;
        }
        //もう既に存在する場合は、このオブジェクトを破棄する。
        else if (_instance != null)
        {
            Destroy(this.gameObject);
        }

        // 所持している装備を保存しているファイルのパスを取得し、ファイルを開く。
        _equipmentHaveJsonFilePath = Path.Combine(Application.persistentDataPath, "HaveEquipmentFile.json");
        //このオブジェクトは、シーンを跨いでもデストロイしない。
        DontDestroyOnLoad(gameObject);
        //リスト用のメモリを割り当てる。
        _haveEquipment._equipments = new List<Equipment>();
        //クラスを初期化
        Initialize_EquipmentBase();
    }
    void Start()
    {

    }

    void Update()
    {

    }

    //<======== このクラスの初期化関連 ========>//
    /// <summary> 装備基底クラスの初期化関数。(派生先で呼び出す。) </summary>
    /// <returns> 初期化に成功した場合true、失敗したらfalseを返す。 </returns>
    bool Initialize_EquipmentBase()
    {
        //csvから装備情報を取得
        OnLoad_EquipmentData_csv();
        return true;
    }

    //<======== ロード & セーブ関連 ========>//
    /// <summary> csvファイルから、全ての装備のデータを読み込む関数 </summary>
    /// <returns> 読み込んだ結果を返す。失敗した場合はnullを返す。 </returns>
    void OnLoad_EquipmentData_csv()
    {
        _equipmentData = new Equipment[(int)EquipmentID.ID_END];

        int index = 0;
        bool isFirstLine = true;//一行目かどうかを判断する値
        //CSVファイルからアイテムデータを読み込み、配列に保存する
        StreamReader sr = new StreamReader(@_equipmentCsvFilePath);//ファイルを開く
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
           Get_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           values[12],
           values[13]); break;
                //胴用装備を取得し保存
                case "Torso":
                    _equipmentData[index] = new TorsoParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.TORSO_PARTS,//Type
           values[2],//name
           Get_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           values[12],
           values[13]); break;
                //腕用装備を取得し保存
                case "Arm":
                    _equipmentData[index] = new ArmParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.ARM_PARTS,//Type
           values[2],//name
           Get_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           values[12],
           values[13],
           ArmParts.Get_AttackType(values[14])); break;
                //足用装備を取得し保存
                case "Foot":
                    _equipmentData[index] = new FootParts(
           (EquipmentID)int.Parse(values[0]),//ID
           Equipment.EquipmentType.FOOT_PARTS,//Type
           values[2],//name
           Get_EquipmentRarity(values[3]), //rarity
           float.Parse(values[4]),
           float.Parse(values[5]),
           float.Parse(values[6]),
           float.Parse(values[7]),
           float.Parse(values[8]),
           float.Parse(values[9]),
           float.Parse(values[10]),
           float.Parse(values[11]),
           values[12],
           values[13]); break;
                default: Debug.LogError("設定されていないEquipmentTypeです。"); break;
            }
            index++;
        }

    }
    /// <summary> 所持している装備を、jsonファイルからデータを読み込み、メンバー変数に格納する処理。 </summary>
    public void OnLoad_EquipmentHaveData_Json()
    {
        Debug.Log("所持している装備データをロードします！");
        // 念のためファイルの存在チェック
        if (!File.Exists(_equipmentHaveJsonFilePath))
         {
            //ここにファイルが無い場合の処理を書く
            Debug.Log("所持している装備データを保存しているファイルが見つかりません。");

            //処理を抜ける
            return;
        }
        // JSONオブジェクトを、デシリアライズ(C#形式に変換)し、値をセット。
        //_itemVolume = JsonUtility.FromJson<ItemNumberOfPossessions>(File.ReadAllText(filePath));
    }
    /// <summary> 所持している装備のデータを、jsonファイルに保存する処理。 </summary>
    public void OnSave_EquipmentHaveData_Json()
    {
        Debug.Log("所持している装備データをセーブします！");
        // アイテム所持数データを、JSON形式にシリアライズし、ファイルに保存
        //File.WriteAllText(filePath, JsonUtility.ToJson(_itemVolume, false));
    }
    /// <summary> 現在装備している装備をjsonファイルから取得し、メンバー変数に格納する処理。 </summary>
    public void OnLoad_EquippedData_Json()
    {

    }
    /// <summary> 現在装備している装備のデータを、jsonファイルに保存する処理。 </summary>
    public void OnSave_EquippedData_Json()
    {

    }

    Equipment.EquipmentRarity Get_EquipmentRarity(string str)
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

    //以下テスト用、実際に使えるモノと判断したら本番移行する。
    /// <summary> テスト用スクリプト。(今は)ボタンから呼び出す。特定の装備を取得する。 </summary>
    /// <param name="id"> 取得する装備のID </param>
    public void Get_Equipment(int id)
    {
        _haveEquipment._equipments.Add(_equipmentData[id]);
    }

    /// <summary> テスト用スクリプト。(今は)ボタンから呼び出す。特定の装備を失う。 </summary>
    /// <param name="id"> 減らす装備のID </param>
    public void Lost_Equipment(int id)
    {
        _haveEquipment._equipments.Remove(_equipmentData[id]);
    }
}
