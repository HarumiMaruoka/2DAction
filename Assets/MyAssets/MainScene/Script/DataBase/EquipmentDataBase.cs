using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//using UnityEngine;

///// <summary> 
///// 全ての装備の情報と、
///// プレイヤーが所持している装備・着用している装備を、管理するクラス。
///// </summary>
//public class EquipmentDataBase : UseEventSystemBehavior
//{
//    //===== このクラスで使用する型 =====//
//    #region User defined type
//    /// <summary> 
//    /// 現在装着している装備を表す構造体<br/>
//    /// </summary>
//    public struct MyEquipped
//    {
//        /// <summary> 頭に装着しているパーツ </summary>
//        public int _headPartsID;
//        /// <summary> 胴に装備しているパーツ </summary>
//        public int _torsoPartsID;
//        /// <summary> 右腕に装着しているパーツ </summary>
//        public int _armRightPartsID;
//        /// <summary> 左腕に装着しているパーツ </summary>
//        public int _armLeftPartsID;
//        /// <summary> 足に装着しているパーツ </summary>
//        public int _footPartsID;
//    }
//    /// <summary> 
//    /// 所持している装備を格納する構造体 :<br/>
//    /// jsonファイルに保存する用
//    /// </summary>
//    public struct HaveEquipped
//    {
//        /// <summary> 要素は装備のID。所持していなければ-1。 </summary>
//        public int[] _equipmentsID;
//    }
//    #endregion

//    //<=========== フィールド / プロパティ ===========>//
//    #region Field and Property
//    /// <summary> 装備更新時に呼び出されるデリゲート変数。 </summary>
//    public System.Action ReplacedEquipment;
//    /// <summary> 全ての装備の情報を保存しておくフィールド </summary>
//    Equipment[] _equipmentData;
//    /// <summary> 全ての装備の情報が保存されたプロパティ </summary>
//    public Equipment[] EquipmentData { get => _equipmentData; }
//    /// <summary> 所持している装備を表すフィールド </summary>
//    HaveEquipped _haveEquipmentID;
//    /// <summary> 所持している装備のプロパティ </summary>
//    public HaveEquipped HaveEquipmentID { get => _haveEquipmentID; }
//    /// <summary> 現在着用している装備を表すフィールド </summary>
//    MyEquipped _equipped;
//    /// <summary> 現在着用している装備のフィールド </summary>
//    public MyEquipped Equipped { get => _equipped; }
//    #endregion
//    #region Inspector Variables
//    // インスペクタから設定すべき値
//    [Header("装備の基本情報が格納されたcsvファイルへのパス"), SerializeField] string _equipmentCsvFilePath;
//    [Header("確認用 : 所持している装備の情報が格納されたjsonファイルへのパス"), SerializeField] string _equipmentHaveJsonFilePath;
//    [Header("確認用 : 現在装備している装備の情報が格納されたjsonファイルへのパス"), SerializeField] string _equippedJsonFilePath;
//    [Header("プレイヤーが所持できる装備の最大数"), SerializeField] int _maxHaveVolume;

//    /// <summary> プレイヤーが所持できる装備の最大数 </summary>
//    public int MaxHaveValue { get => _maxHaveVolume; set => _maxHaveVolume = value; }

//    [SerializeField] GameObject _equipmentLostPanel;
//    GameObject _holdSelectedEquipment = default;
//    #endregion

//    //<======シングルトンパターン関連======>//
//    //インスタンス
//    private static EquipmentDataBase _instance;
//    //インスタンスのプロパティ
//    public static EquipmentDataBase Instance
//    {
//        get
//        {
//            if (_instance == null)
//            {
//                Debug.LogError("EquipmentDataBase._instanceはnullです。");
//            }
//            return _instance;
//        }
//    }
//    //プライベートなコンストラクタ
//    private EquipmentDataBase() { }

//    //===== Unityメッセージ =====//
//    void Awake()
//    {
//        //クラスを初期化
//        Initialize_EquipmentBase();
//    }
//    void Update()
//    {
//        //Kキー押下でセーブする
//        if (Input.GetKeyDown(KeyCode.K))
//        {
//            Debug.Log("装備関係をセーブする。");
//            OnSave_EquipmentHaveData_Json();
//            OnSave_EquippedData_Json();
//        }
//        //Lキー押下でロードする
//        if (Input.GetKeyDown(KeyCode.L))
//        {
//            Debug.Log("装備関係をロードする。");
//            OnLoad_EquipmentHaveData_Json();
//            OnLoad_EquippedData_Json();
//        }

//        if (_beforeSelectedGameObject != _eventSystem.currentSelectedGameObject && ReplacedEquipment != null)
//        {
//            ReplacedEquipment();
//        }
//        _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
//    }


//    //<======== privateメンバー関数 ========>//
//    /// <summary> 装備基底クラスの初期化関数。(派生先で呼び出す。) </summary>
//    /// <returns> 初期化に成功した場合true、失敗したらfalseを返す。 </returns>
//    bool Initialize_EquipmentBase()
//    {
//        //===== シングルトン関係の処理 =====//
//        //もしインスタンスが設定されていなかったら自身を代入する
//        if (_instance == null)
//        {
//            _instance = this;
//        }
//        //もう既に存在する場合は、このオブジェクトを破棄する。
//        else if (_instance != null)
//        {
//            Destroy(this.gameObject);
//        }
//        DontDestroyOnLoad(gameObject);

//        // 所持している装備を保存しているファイルのパスを取得し、ファイルを開く。
//        _equipmentHaveJsonFilePath = Path.Combine(Application.persistentDataPath, "HaveEquipmentFile.json");
//        _equippedJsonFilePath = Path.Combine(Application.persistentDataPath, "EquippedFile.json");

//        // csvファイルからすべての装備情報を取得する。
//        OnLoad_EquipmentData_csv();

//        // 現在の着用している装備を初期化する。
//        _equipped._headPartsID = (int)EquipmentID.None;
//        _equipped._torsoPartsID = (int)EquipmentID.None;
//        _equipped._armRightPartsID = (int)EquipmentID.None;
//        _equipped._armLeftPartsID = (int)EquipmentID.None;
//        _equipped._footPartsID = (int)EquipmentID.None;

//        // 配列用のメモリを確保する。
//        _haveEquipmentID._equipmentsID = new int[_maxHaveVolume];
//        if (_equipmentData == null) _equipmentData = new Equipment[(int)EquipmentID.ID_END];

//        // 所持している装備を初期化する。
//        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length; i++)
//        {
//            _haveEquipmentID._equipmentsID[i] = -1;
//        }

//        // ***** テスト用コード ***** // : テキトーに所持していることにする。
//        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length - 1; i++)
//        {
//            _haveEquipmentID._equipmentsID[i] = i % _equipmentData.Length;
//        }
//        // Debug.LogError(_haveEquipmentID._equipmentsID[_haveEquipmentID._equipmentsID.Length - 1]);


//        // jsonファイルから所持している装備と着用している装備の情報を読み込む。
//        // OnLoad_EquipmentHaveData_Json();
//        // OnLoad_EquippedData_Json();

//        return true;
//    }
//    //<======== ロード & セーブ関連 ========>//
//    /// <summary> 
//    /// csvファイルから、全ての装備のデータを読み込みメンバー変数に保存する。<br/>
//    /// </summary>
//    /// <returns> 読み込みに成功したらtrue,失敗した場合はfalseを返す。 </returns>
//    void OnLoad_EquipmentData_csv()
//    {
//        if (_equipmentData == null) _equipmentData = new Equipment[(int)EquipmentID.ID_END];

//        int index = 0;
//        bool isFirstLine = true;//一行目かどうかを判断する値
//        //CSVファイルからアイテムデータを読み込み、配列に保存する
//        StreamReader sr = new StreamReader(@_equipmentCsvFilePath);//ファイルを開く
//        while (!sr.EndOfStream)// 末尾まで繰り返す
//        {
//            string[] values = sr.ReadLine().Split(',');//一行読み込み区切って保存する
//            //最初の行(ヘッダーの行)はスキップする
//            if (isFirstLine)
//            {
//                isFirstLine = false;
//                continue;
//            }
//            //種類別で生成し保存する
//            switch (values[1])
//            {
//                //頭用装備を取得し保存
//                case "Head":
//                    _equipmentData[index] = new HeadParts(
//           (EquipmentID)int.Parse(values[0]),//ID
//           Equipment.EquipmentType.HEAD_PARTS,//Type
//           values[2],//name
//           Conversion_EquipmentRarity(values[3]), //rarity
//           float.Parse(values[4]),
//           float.Parse(values[5]),
//           float.Parse(values[6]),
//           float.Parse(values[7]),
//           float.Parse(values[8]),
//           float.Parse(values[9]),
//           float.Parse(values[10]),
//           float.Parse(values[11]),
//           float.Parse(values[12]),
//           values[13],
//           values[14]); break;
//                //胴用装備を取得し保存
//                case "Torso":
//                    _equipmentData[index] = new TorsoParts(
//           (EquipmentID)int.Parse(values[0]),//ID
//           Equipment.EquipmentType.TORSO_PARTS,//Type
//           values[2],//name
//           Conversion_EquipmentRarity(values[3]), //rarity
//           float.Parse(values[4]),
//           float.Parse(values[5]),
//           float.Parse(values[6]),
//           float.Parse(values[7]),
//           float.Parse(values[8]),
//           float.Parse(values[9]),
//           float.Parse(values[10]),
//           float.Parse(values[11]),
//           float.Parse(values[12]),
//           values[13],
//           values[14]); break;
//                //腕用装備を取得し保存
//                case "Arm":
//                    _equipmentData[index] = new ArmParts(
//           (EquipmentID)int.Parse(values[0]),//ID
//           Equipment.EquipmentType.ARM_PARTS,//Type
//           values[2],//name
//           Conversion_EquipmentRarity(values[3]), //rarity
//           float.Parse(values[4]),
//           float.Parse(values[5]),
//           float.Parse(values[6]),
//           float.Parse(values[7]),
//           float.Parse(values[8]),
//           float.Parse(values[9]),
//           float.Parse(values[10]),
//           float.Parse(values[11]),
//           float.Parse(values[12]),
//           values[13],
//           values[14],
//           ArmParts.Get_AttackType(values[15]),
//           float.Parse(values[16])); break;
//                //足用装備を取得し保存
//                case "Foot":
//                    _equipmentData[index] = new FootParts(
//           (EquipmentID)int.Parse(values[0]),//ID
//           Equipment.EquipmentType.FOOT_PARTS,//Type
//           values[2],//name
//           Conversion_EquipmentRarity(values[3]), //rarity
//           float.Parse(values[4]),
//           float.Parse(values[5]),
//           float.Parse(values[6]),
//           float.Parse(values[7]),
//           float.Parse(values[8]),
//           float.Parse(values[9]),
//           float.Parse(values[10]),
//           float.Parse(values[11]),
//           float.Parse(values[12]),
//           values[13],
//           values[14]); break;
//                default: Debug.LogError("設定されていないEquipmentTypeです。"); break;
//            }
//            index++;
//        }

//    }
//    /// <summary> 着用している装備のステータス上昇値をプレイヤーステータスに適用する。: 全身の処理 </summary>
//    void ApplyEquipment_ALL()
//    {
//        //リセットする。
//        PlayerStatusManager.Instance.Equipment_RisingValue = PlayerStatusManager.PlayerStatus._zero;
//        //増加値を適用する。
//        ApplyEquipment_SpecificParts(_equipped._headPartsID);//頭
//        ApplyEquipment_SpecificParts(_equipped._torsoPartsID);//胴
//        ApplyEquipment_SpecificParts(_equipped._armLeftPartsID);//左腕
//        ApplyEquipment_SpecificParts(_equipped._armRightPartsID);//右腕
//        ApplyEquipment_SpecificParts(_equipped._footPartsID);//足
//    }
//    /// <summary> 着用している装備のステータス上昇値をプレイヤーステータスに適用する。 </summary>
//    /// <param name="equipmentID"> 適用する装備のID </param>
//    void ApplyEquipment_SpecificParts(int equipmentID)
//    {
//        if (equipmentID >= 0)
//        {
//            PlayerStatusManager.Instance.Equipment_RisingValue += EquipmentData[equipmentID].ThisEquipment_StatusRisingValue;
//        }
//        else
//        {
//            Debug.LogWarning("未装備の箇所はありますか?そうでなければエラーです! : 装備マネージャーコンポーネントより");
//        }
//    }

//    //<===== publicメンバー関数 =====>//
//    /// <summary> "所持"している装備を、jsonファイルからデータを読み込み、メンバー変数に格納する処理。 </summary>
//    public void OnLoad_EquipmentHaveData_Json()
//    {
//        //Debug.Log("所持している装備データをロードします！");
//        // 念のためファイルの存在チェック
//        if (!File.Exists(_equipmentHaveJsonFilePath))
//        {
//            //ここにファイルが無い場合の処理を書く
//            Debug.Log("所持している装備データを保存しているファイルが見つかりません。");

//            //処理を抜ける
//            return;
//        }
//        // ファイルが存在する場合の処理。
//        // JSONオブジェクトを、デシリアライズ(C#形式に変換)し、値をメンバー変数にセットする。
//        _haveEquipmentID = JsonUtility.FromJson<HaveEquipped>(File.ReadAllText(_equipmentHaveJsonFilePath));

//        //***以下デバッグ用のコード。***
//        //所持している装備をコンソールに表示する。
//        foreach (var i in _haveEquipmentID._equipmentsID)
//        {
//            if (i == -1) { Debug.Log("この要素は空です。"); }
//            else Debug.Log(_equipmentData[i]._myName);
//        }
//    }
//    /// <summary> "所持"している装備のデータを、jsonファイルに保存する処理。 </summary>
//    public void OnSave_EquipmentHaveData_Json()
//    {
//        Debug.Log("所持している装備データをセーブします！");
//        // 所持している装備データを、JSON形式にシリアライズし、jsonファイルに保存
//        File.WriteAllText(_equipmentHaveJsonFilePath, JsonUtility.ToJson(_haveEquipmentID, false));
//    }
//    /// <summary> "着用"している装備をjsonファイルから取得し、メンバー変数に格納する処理。 </summary>
//    public void OnLoad_EquippedData_Json()
//    {
//        Debug.Log("現在装備している装備データをロードします！");
//        // 念のためファイルの存在チェック
//        if (!File.Exists(_equippedJsonFilePath))
//        {
//            //ここにファイルが無い場合の処理を書く
//            Debug.Log("現在装備している装備データを保存しているファイルが見つかりません。");

//            //処理を抜ける
//            return;
//        }
//        // JSONオブジェクトを、デシリアライズ(C#形式に変換)し、値をセット。
//        _equipped = JsonUtility.FromJson<MyEquipped>(File.ReadAllText(_equippedJsonFilePath));
//    }
//    /// <summary> "着用"している装備のデータを、jsonファイルに保存する処理。 </summary>
//    public void OnSave_EquippedData_Json()
//    {
//        Debug.Log("現在装備している装備データをセーブします！");
//        // 現在装備している装備データを、JSON形式にシリアライズし、ファイルに保存
//        File.WriteAllText(_equippedJsonFilePath, JsonUtility.ToJson(_equipped, false));
//    }
//    /// <summary> 所持している装備と、着用している装備を交換する。 </summary>
//    /// <param name="fromNowEquipmentID"> これから装備する装備のID </param>
//    /// <param name="fromNowEquipmentType"> これから装備する装備のType </param>
//    /// <param name="armFlag"> どちらの腕装備するか判断する値、0なら左腕、1なら右腕。 </param>
//    public void Swap_HaveToEquipped(int fromNowEquipmentID, Equipment.EquipmentType fromNowEquipmentType, EquipmentButton button, int armFlag = -1)
//    {
//        //Debug.Log("これから着用する装備のID : " + fromNowEquipmentID);
//        //Debug.Log("これから着用する装備のType : " + fromNowEquipmentType);

//        int temporary = -1;//仮の入れ物
//        //Typeを基に着用する
//        //腕以外の場合
//        if (fromNowEquipmentType != Equipment.EquipmentType.ARM_PARTS)
//        {
//            switch (fromNowEquipmentType)
//            {
//                //頭パーツの場合
//                case Equipment.EquipmentType.HEAD_PARTS:
//                    temporary = _equipped._headPartsID;
//                    _equipped._headPartsID = fromNowEquipmentID;
//                    break;

//                //胴パーツの場合
//                case Equipment.EquipmentType.TORSO_PARTS:
//                    temporary = _equipped._torsoPartsID;
//                    _equipped._torsoPartsID = fromNowEquipmentID;
//                    break;

//                //足パーツの場合
//                case Equipment.EquipmentType.FOOT_PARTS:
//                    temporary = _equipped._footPartsID;
//                    _equipped._footPartsID = fromNowEquipmentID;
//                    break;
//            }
//            //着脱した装備をインベントリに格納する
//            if (temporary != -1) button.Set_Equipment(EquipmentData[temporary]);
//            else button.Set_Equipment(null);
//        }
//        //腕の場合
//        else
//        {
//            if (armFlag == 0)
//            {
//                temporary = _equipped._armLeftPartsID;
//                _equipped._armLeftPartsID = fromNowEquipmentID;
//            }
//            else if (armFlag == 1)
//            {
//                temporary = _equipped._armRightPartsID;
//                _equipped._armRightPartsID = fromNowEquipmentID;
//            }
//            else
//            {
//                Debug.LogError($"不正な値です{armFlag}");
//            }
//            //着脱した装備をインベントリに格納する
//            if (temporary != -1) button.Set_Equipment(EquipmentData[temporary]);
//            else button.Set_Equipment(null);
//        }
//        ApplyEquipment_ALL();
//        ReplacedEquipment();
//    }

//    /// <summary> レアリティを表す string を enum に変換する。 </summary>
//    /// <param name="str"> 対象の文字列 </param>
//    /// <returns></returns>
//    Equipment.EquipmentRarity Conversion_EquipmentRarity(string str)
//    {
//        switch (str)
//        {
//            case "A": return Equipment.EquipmentRarity.A;
//            case "B": return Equipment.EquipmentRarity.B;
//            case "C": return Equipment.EquipmentRarity.C;
//            case "D": return Equipment.EquipmentRarity.D;
//            case "E": return Equipment.EquipmentRarity.E;
//        }
//        Debug.LogError("不正な値です。");
//        return Equipment.EquipmentRarity.ERROR;
//    }


//    //===== 便利機能 =====//
//    /// <summary> 
//    /// 特定の装備を取得する。:<br/>
//    /// 引数に渡されたIDの装備を、"所持している装備"を表す変数に保存する。<br/>
//    /// インベントリに空きがなければ false を返す。<br/>
//    /// </summary>
//    /// <param name="id"> 取得する装備のID </param>
//    /// <returns> 
//    /// 取得に成功したらtrue<br/>
//    /// インベントリに空きがなく、取得に失敗した場合はfalseを返す。<br/>
//    /// </returns>
//    public bool Get_Equipment(int id)
//    {
//        //装備の取得処理
//        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length; i++)
//        {
//            if (_haveEquipmentID._equipmentsID[i] == -1 && id != -1)
//            {
//                _haveEquipmentID._equipmentsID[i] = id;
//                return true;
//            }
//        }
//        return false;
//    }
//    /// <summary> 
//    /// 特定の装備を失う。:<br/>
//    /// 引数に渡されたIDの装備を"所持している装備"を表す変数から削除する。<br/>
//    /// 削除に失敗した場合は false を返す。
//    /// </summary>
//    /// <param name="id"> 減らす装備のID </param>
//    /// <returns> 
//    /// 削除に成功したら true,失敗した場合は false を返す。<br/>
//    /// </returns>
//    public bool Lost_Equipment(int id)
//    {
//        //装備の喪失処理
//        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length; i++)
//        {
//            if (i == id)
//            {
//                _haveEquipmentID._equipmentsID[i] = -1;
//                return true;
//            }
//        }
//        return false;
//    }
//    /// <summary>
//    /// 選択中の装備を保持する。<br/>
//    /// 「装備を捨てる」ボタンが押された瞬間に実行する。<br/>
//    /// </summary>
//    public void HoldSelectedEquipment()
//    {
//        if (_beforeSelectedGameObject != null && _beforeSelectedGameObject.TryGetComponent(out EquipmentButton equipment))
//        {
//            _equipmentLostPanel.SetActive(true);
//            _holdSelectedEquipment = equipment.gameObject;
//        }
//        else
//        {
//            Debug.LogError("ホールドに失敗しました。");
//        }
//    }
//    /// <summary> _holdSelectedEquipmentに保持されている装備を失う処理 </summary>
//    public void SelectedEquipmentLost()
//    {
//        // _holdSelectedEquipmentをnullチェックし、_holdSelectedEquipmentにEquipmentButtonがアタッチされている場合の処理
//        if (_holdSelectedEquipment != null && _holdSelectedEquipment.TryGetComponent(out EquipmentButton equipment))
//        {
//            // 装備を捨てる。成功の可否を表示する。
//            if (Lost_Equipment((int)equipment._myEquipment._myID))
//            {
//                Debug.Log("装備を失いました。");
//                equipment.gameObject.SetActive(false);
//            }
//            else
//            {
//                Debug.Log("装備を失うのに失敗しました。");
//            }
//            ReleaseHold();
//        }
//        // チェックに通らなかった場合の処理
//        else
//        {
//            if (_holdSelectedEquipment != null)
//                Debug.Log($"{_holdSelectedEquipment.name}はEquipmentButtonを持っていません");
//            else
//                Debug.Log("何もホールドされていません。");
//        }
//    }
//    /// <summary> ホールドしているGameObjectを開放する。 </summary>
//    public void ReleaseHold()
//    {
//        _holdSelectedEquipment = null;
//    }
//}

/// <summary> 装備のID </summary>
public enum EquipmentID
{
    None = -1,
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