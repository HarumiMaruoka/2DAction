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
        Nan = -1,
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
    public struct MyEquipped
    {
        public int _headPartsID;
        public int _torsoPartsID;
        public int _armRightPartsID;
        public int _armLeftPartsID;
        public int _footPartsID;
    }
    /// <summary> 所持している装備を格納する構造体 </summary>
    public struct HaveEquipped
    {
        /// <summary> 要素は装備のID。所持していなければ-1。 </summary>
        public int[] _equipmentsID;
    }

    //<=========== 必要な値 ===========>//
    /// <summary> 全ての装備の情報を一時保存しておく変数 </summary>
    Equipment[] _equipmentData;
    public Equipment[] EquipmentData { get=> _equipmentData; }
    /// <summary> 所持している装備の配列 </summary>
    HaveEquipped _haveEquipmentID;
    public HaveEquipped HaveEquipmentID { get => _haveEquipmentID; }
    /// <summary> 現在装備している装備 </summary>
    MyEquipped _equipped;
    public MyEquipped Equipped { get => _equipped; }

    //<======== アサインすべき値 ========>//

    //<===== インスペクタから設定すべき値 =====>//
    [Header("装備の基本情報が格納されたcsvファイルへのパス"), SerializeField] string _equipmentCsvFilePath;
    [Header("所持している装備の情報が格納されたjsonファイルへのパス"), SerializeField] string _equipmentHaveJsonFilePath;
    [Header("現在装備している装備の情報が格納されたjsonファイルへのパス"), SerializeField] string _equippedJsonFilePath;
    /// <summary> プレイヤーが所持できる装備の最大数 </summary>
    [Header("プレイヤーが所持できる装備の最大数"), SerializeField] int _maxHaveVolume;
    /// <summary> プレイヤーが所持できる装備の最大数 </summary>
    public int MaxHaveValue { get => _maxHaveVolume; set => _maxHaveVolume = value; }

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
        _equippedJsonFilePath = Path.Combine(Application.persistentDataPath, "EquippedFile.json");
        //このオブジェクトは、シーンを跨いでもデストロイしない。
        DontDestroyOnLoad(gameObject);
        //配列用のメモリを確保し、-1で初期化する。
        _haveEquipmentID._equipmentsID = new int[_maxHaveVolume];
        _equipmentData = new Equipment[(int)EquipmentID.ID_END];

        // テスト用コード : テキトーに所持していることにする。
        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length; i++)
        {
            _haveEquipmentID._equipmentsID[i] = i % _equipmentData.Length;
        }

        //クラスを初期化
        Initialize_EquipmentBase();

        //Debug.Log("装備関係をロードする。");
        //OnLoad_EquipmentHaveData_Json();
        //OnLoad_EquippedData_Json();
    }
    void Start()
    {

    }

    void Update()
    {
        //Kキー押下でセーブする
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("装備関係をセーブする。");
            OnSave_EquipmentHaveData_Json();
            OnSave_EquippedData_Json();
        }
        //Lキー押下でロードする
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("装備関係をロードする。");
            OnLoad_EquipmentHaveData_Json();
            OnLoad_EquippedData_Json();
        }
    }

    //<======== このクラスの初期化関連 ========>//
    /// <summary> 装備基底クラスの初期化関数。(派生先で呼び出す。) </summary>
    /// <returns> 初期化に成功した場合true、失敗したらfalseを返す。 </returns>
    bool Initialize_EquipmentBase()
    {
        //csvから装備情報を取得
        OnLoad_EquipmentData_csv();
        //現在の着用している装備を初期化
        _equipped._headPartsID = (int)EquipmentID.Nan;
        _equipped._torsoPartsID = (int)EquipmentID.Nan;
        _equipped._armRightPartsID = (int)EquipmentID.Nan;
        _equipped._armLeftPartsID = (int)EquipmentID.Nan;
        _equipped._footPartsID = (int)EquipmentID.Nan;
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
        _haveEquipmentID = JsonUtility.FromJson<HaveEquipped>(File.ReadAllText(_equipmentHaveJsonFilePath));
        foreach (var i in _haveEquipmentID._equipmentsID)
        {
            if (i == -1) { Debug.Log("この要素は空です。"); }
            else Debug.Log(_equipmentData[i]._myName);
        }
    }
    /// <summary> 所持している装備のデータを、jsonファイルに保存する処理。 </summary>
    public void OnSave_EquipmentHaveData_Json()
    {
        Debug.Log("所持している装備データをセーブします！");
        // 所持している装備データを、JSON形式にシリアライズし、ファイルに保存
        File.WriteAllText(_equipmentHaveJsonFilePath, JsonUtility.ToJson(_haveEquipmentID, false));
        foreach (var i in _haveEquipmentID._equipmentsID)
        {
            if (i == -1) { Debug.Log("この要素は空です。"); }
            else Debug.Log(_equipmentData[i]._myName);
        }
    }
    /// <summary> 所持している装備をjsonファイルから取得し、メンバー変数に格納する処理。 </summary>
    public void OnLoad_EquippedData_Json()
    {
        Debug.Log("現在装備している装備データをロードします！");
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
    /// <summary> 現在装備している装備のデータを、jsonファイルに保存する処理。 </summary>
    public void OnSave_EquippedData_Json()
    {
        Debug.Log("現在装備している装備データをセーブします！");
        // 現在装備している装備データを、JSON形式にシリアライズし、ファイルに保存
        File.WriteAllText(_equippedJsonFilePath, JsonUtility.ToJson(_equipped, false));
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
    public bool Get_Equipment(int id)
    {
        //装備の取得処理
        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length; i++)
        {
            if (i == -1)
            {
                _haveEquipmentID._equipmentsID[i] = id;
                return true;
            }
        }
        return false;
    }

    /// <summary> テスト用スクリプト。(今は)ボタンから呼び出す。特定の装備を失う。 </summary>
    /// <param name="id"> 減らす装備のID </param>
    public bool Lost_Equipment(int id)
    {
        //装備の喪失処理
        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length; i++)
        {
            if (i == id)
            {
                _haveEquipmentID._equipmentsID[i] = -1;
                return true;
            }
        }
        return false;
    }

    /// <summary> 現在装備している装備をConsoleに表示する。テスト用。 </summary>
    public void DrawDebugLog_Equipped()
    {
        Debug.Log(
            "現在着用している装備\n" +
            "頭パーツ : " + _equipped._headPartsID
            + "/" +
            "胴パーツ : " + _equipped._torsoPartsID
            + "/" +
            "右腕パーツ : " + _equipped._armRightPartsID
            + "/" +
            "左腕パーツ : " + _equipped._armLeftPartsID
            + "/" +
            "足パーツ : " + _equipped._footPartsID
            );
    }
}
