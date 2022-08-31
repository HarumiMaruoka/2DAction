using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary> 
/// 全ての装備の情報と、
/// プレイヤーが所持している装備・着用している装備を、管理するクラス。
/// </summary>
public class EquipmentDataBase : MonoBehaviour
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
    /// <summary> 現在装着している装備を表す構造体 </summary>
    public struct MyEquipped
    {
        /// <summary> 頭に装着しているパーツ </summary>
        public int _headPartsID;
        /// <summary> 胴に装備しているパーツ </summary>
        public int _torsoPartsID;
        /// <summary> 右腕に装着しているパーツ </summary>
        public int _armRightPartsID;
        /// <summary> 左腕に装着しているパーツ </summary>
        public int _armLeftPartsID;
        /// <summary> 足に装着しているパーツ </summary>
        public int _footPartsID;
    }
    /// <summary> 所持している装備を格納する構造体 </summary>
    public struct HaveEquipped
    {
        /// <summary> 要素は装備のID。所持していなければ-1。 </summary>
        public int[] _equipmentsID;
    }

    //<=========== メンバー変数 ===========>//
    /// <summary> 装備更新時に呼び出されるデリゲート変数。 </summary>
    public System.Action ReplacedEquipment;
    /// <summary> 全ての装備の情報を一時保存しておく変数 </summary>
    Equipment[] _equipmentData;
    public Equipment[] EquipmentData { get => _equipmentData; }
    /// <summary> 所持している装備の配列 </summary>
    HaveEquipped _haveEquipmentID;
    public HaveEquipped HaveEquipmentID { get => _haveEquipmentID; }
    /// <summary> 現在装備している装備 </summary>
    MyEquipped _equipped;
    public MyEquipped Equipped { get => _equipped; }
    [Header("現在着用している装備の表示を管理しているクラス"), SerializeField] Draw_NowEquipped _draw_NowEquipped;
    [Header("装備の上昇値の表示を管理しているクラス"), SerializeField] ManagerOfPossessedEquipment _managerOfPossessedEquipment;

    //<===== インスペクタから設定すべき値 =====>//
    [Header("装備の基本情報が格納されたcsvファイルへのパス"), SerializeField] string _equipmentCsvFilePath;
    [Header("確認用 : 所持している装備の情報が格納されたjsonファイルへのパス"), SerializeField] string _equipmentHaveJsonFilePath;
    [Header("確認用 : 現在装備している装備の情報が格納されたjsonファイルへのパス"), SerializeField] string _equippedJsonFilePath;
    [Header("プレイヤーが所持できる装備の最大数"), SerializeField] int _maxHaveVolume;

    /// <summary> イベントシステム </summary>
    [Header("イベントシステム"), SerializeField] EventSystem _eventSystem;
    GameObject _beforeSelectedGameObject;

    bool _isTextUpdate = true;
    public bool IsTextUpdate { get => _isTextUpdate; set => _isTextUpdate = value; }

    /// <summary> プレイヤーが所持できる装備の最大数 </summary>
    public int MaxHaveValue { get => _maxHaveVolume; set => _maxHaveVolume = value; }

    public const int LEFT_ARM = 0;
    public const int RIGHT_ARM = 1;

    //<======シングルトンパターン関連======>//
    //インスタンス
    private static EquipmentDataBase _instance;
    //インスタンスのプロパティ
    public static EquipmentDataBase Instance
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
    //プライベートなコンストラクタ
    private EquipmentDataBase() { }



    //<======= Unityメッセージ =======>//
    private void Awake()
    {
        //クラスを初期化
        Initialize_EquipmentBase();
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

        _isTextUpdate = _beforeSelectedGameObject != _eventSystem.currentSelectedGameObject;
        if (_isTextUpdate)
        {
            _isTextUpdate = false;
            //StartCoroutine(WaitOneFrame_UpdateText());
            ReplacedEquipment();
        }
        _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }


    //<======== privateメンバー関数 ========>//
    /// <summary> 装備基底クラスの初期化関数。(派生先で呼び出す。) </summary>
    /// <returns> 初期化に成功した場合true、失敗したらfalseを返す。 </returns>
    bool Initialize_EquipmentBase()
    {
        /*** シングルトン関係の処理 ***/
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
        DontDestroyOnLoad(gameObject);

        /***** 所持している装備を保存しているファイルのパスを取得し、ファイルを開く。 *****/
        _equipmentHaveJsonFilePath = Path.Combine(Application.persistentDataPath, "HaveEquipmentFile.json");
        _equippedJsonFilePath = Path.Combine(Application.persistentDataPath, "EquippedFile.json");

        /***** 配列用のメモリを確保する。 *****/
        _haveEquipmentID._equipmentsID = new int[_maxHaveVolume];
        _equipmentData = new Equipment[(int)EquipmentID.ID_END];

        // ***** テスト用コード ***** // : テキトーに所持していることにする。
        for (int i = 0; i < _haveEquipmentID._equipmentsID.Length; i++)
        {
            _haveEquipmentID._equipmentsID[i] = i % _equipmentData.Length;
        }

        /***** csvファイルからすべての装備情報を取得 *****/
        OnLoad_EquipmentData_csv();

        /***** 現在の着用している装備を初期化 *****/
        _equipped._headPartsID = (int)EquipmentID.Nan;
        _equipped._torsoPartsID = (int)EquipmentID.Nan;
        _equipped._armRightPartsID = (int)EquipmentID.Nan;
        _equipped._armLeftPartsID = (int)EquipmentID.Nan;
        _equipped._footPartsID = (int)EquipmentID.Nan;

        /*jsonファイルから所持している装備と着用している装備の*/
        //OnLoad_EquipmentHaveData_Json();
        //OnLoad_EquippedData_Json();

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
    /// <summary> 着用している装備のステータス上昇値をプレイヤーステータスに適用する。 : 全身 </summary>
    void ApplyEquipment_ALL()
    {
        //リセットする。
        PlayerStatusManager.Instance.Equipment_RisingValue = PlayerStatusManager.PlayerStatus._zero;
        //増加値を適用する。
        ApplyEquipment_SpecificParts(_equipped._headPartsID);//頭
        ApplyEquipment_SpecificParts(_equipped._torsoPartsID);//胴
        ApplyEquipment_SpecificParts(_equipped._armLeftPartsID);//左腕
        ApplyEquipment_SpecificParts(_equipped._armRightPartsID);//右腕
        ApplyEquipment_SpecificParts(_equipped._footPartsID);//足
    }
    /// <summary> 着用している装備のステータス上昇値をプレイヤーステータスに適用する。 </summary>
    /// <param name="equipmentID"> 適用する装備のID </param>
    void ApplyEquipment_SpecificParts(int equipmentID)
    {
        if (equipmentID >= 0)
        {
            PlayerStatusManager.Instance.Equipment_RisingValue += EquipmentData[equipmentID].ThisEquipment_StatusRisingValue;
        }
        else
        {
            Debug.LogWarning("未装備の箇所はありますか?そうでなければエラーです! : 装備マネージャーコンポーネントより");
        }
    }

    //<===== publicメンバー関数 =====>//
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
    /// <summary> string を enum に変換する </summary>
    /// <param name="str"></param>
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
    /// <summary> 所持している装備と、着用している装備を交換する。 </summary>
    /// <param name="fromNowEquipmentID"> これから装備する装備のID </param>
    /// <param name="fromNowEquipmentType"> これから装備する装備のType </param>
    /// <param name="armFlag"> どちらの腕装備するか判断する値、0なら左腕、1なら右腕。 </param>
    public void Swap_HaveToEquipped(int fromNowEquipmentID, Equipment.EquipmentType fromNowEquipmentType, EquipmentButton button, int armFlag = -1)
    {
        Debug.Log("これから着用する装備のID : " + fromNowEquipmentID);
        Debug.Log("これから着用する装備のType : " + fromNowEquipmentType);

        //ここに装備を交換するコードを書く

        int temporary = -1;
        //Typeを基に着用する
        //腕以外の場合
        if (fromNowEquipmentType != Equipment.EquipmentType.ARM_PARTS)
        {
            switch (fromNowEquipmentType)
            {
                //頭パーツの場合
                case Equipment.EquipmentType.HEAD_PARTS:
                    temporary = _equipped._headPartsID;
                    _equipped._headPartsID = fromNowEquipmentID;
                    break;

                //胴パーツの場合
                case Equipment.EquipmentType.TORSO_PARTS:
                    temporary = _equipped._torsoPartsID;
                    _equipped._torsoPartsID = fromNowEquipmentID;
                    break;

                //足パーツの場合
                case Equipment.EquipmentType.FOOT_PARTS:
                    temporary = _equipped._footPartsID;
                    _equipped._footPartsID = fromNowEquipmentID;
                    break;
            }
            //着脱した装備をインベントリに格納する
            if (temporary != -1) button.Set_Equipment(EquipmentData[temporary]);
            else button.Set_Equipment(null);
            //表示を更新する
            _draw_NowEquipped.Update_Equipped(fromNowEquipmentType);
            if (temporary != -1) _managerOfPossessedEquipment.Update_RiseValueText(EquipmentData[temporary]);
        }
        //腕の場合
        else
        {
            if (armFlag == 0)
            {
                temporary = _equipped._armLeftPartsID;
                _equipped._armLeftPartsID = fromNowEquipmentID;
            }
            else if (armFlag == 1)
            {
                temporary = _equipped._armRightPartsID;
                _equipped._armRightPartsID = fromNowEquipmentID;
            }
            else
            {
                Debug.LogError($"不正な値です{armFlag}");
            }
            //着脱した装備をインベントリに格納する
            if (temporary != -1) button.Set_Equipment(EquipmentData[temporary]);
            else button.Set_Equipment(null);
            //表示を更新する
            _draw_NowEquipped.Update_Equipped(fromNowEquipmentType, armFlag);
            if (temporary != -1) _managerOfPossessedEquipment.Update_RiseValueText(EquipmentData[temporary]);
        }
        ApplyEquipment_ALL(); 
        ReplacedEquipment();
        //以下要修正
        Debug.Log("着用した装備のID : " + fromNowEquipmentType);
        Debug.Log("着用した装備のID : " + fromNowEquipmentID);
    }


    //<===== 以下テスト用、実際に使えるモノと判断したら本番移行する。 =====>//
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

    /// <summary> 選択中の装備のステータス上昇値の差を取得する。 </summary>
    /// <returns> 現在の総合ステータスと、選択中のパーツを装備することによるステータスの差 </returns>
    public PlayerStatusManager.PlayerStatus Get_SelectedStatusDifference(Equipment selectedEquipment, bool armFlag)
    {
        //現在のステータスを取得する。
        PlayerStatusManager.PlayerStatus result = PlayerStatusManager.Instance.ConsequentialPlayerStatus;
        //選択中の装備の種類を取得する。
        Equipment.EquipmentType type = selectedEquipment._myType;

        return result;
    }

    IEnumerator WaitOneFrame_UpdateText()
    {
        yield return null;
        ReplacedEquipment();
    }
}
