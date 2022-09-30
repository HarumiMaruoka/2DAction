using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// <para>
/// プレイヤーが"所持"している"アイテム"のデータを管理するクラス。
/// </para>
/// プレイヤーが所持しているアイテムの所持数を提供する。<br/>
/// プレイヤーが所持しているアイテムの所持数をjsonファイルに書き込む機能、<br/>
/// jsonファイルから読み込みフィールドに保存する機能を提供する。<br/>
/// </summary>
public class PlayerHaveItemData
{
    // コンストラクタ
    public PlayerHaveItemData() 
    {
        // jsonファイルへのパスを取得する。
        _jsonFilePathName = Path.Combine(Application.persistentDataPath, "PlayerHaveItemData.json");
        Init();
    }
    //===== このクラスで使用する型 =====//
    /// <summary> 
    /// アイテムの所持数を管理する構造体 : <br/>
    /// jsonファイルへ書き込む為に作成した型 <br/>
    /// </summary>
    public struct Data
    {
        /// <summary> 
        /// アイテムの所持数を管理している値。<br/>
        /// インデックスにIDを指定する事で、
        /// そのIDのアイテムの所持数にアクセスできる。
        /// </summary>
        public int[] _itemVolume;
    }

    //===== フィールド =====//
    /// <summary>
    /// アイテムの所持数を記録しているファイルへのパス。
    /// </summary>
    private readonly string _jsonFilePathName;
    /// <summary>
    /// プレイヤーの"アイテムの所持数"を記録しているフィールド。
    /// </summary>
    private Data _haveItemData;

    //===== プロパティ =====//
    public Data HaveItemData => _haveItemData;

    //===== privateメソッド =====//
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Init()
    {
        
    }

    //===== publicメソッド =====//
    /// <summary>
    /// プレイヤーが所持している"アイテムの所持数"をjsonファイルに書き込む。
    /// </summary>
    public void OnLoadPlayerHaveItemData()
    {
        Debug.Log("<color=yellow>アイテム所持数をロードします！</color>");
        // 念のためファイルの存在チェック
        if (!File.Exists(_jsonFilePathName))
        {
            //ここにファイルが無い場合の処理を書く
            Debug.LogError("<color=yellow>アイテム所持数を保存しているファイルが見つかりません。</color>");

            //処理を抜ける
            return;
        }
        // JSONオブジェクトを、デシリアライズ(C#形式に変換)し、値をセット。
        _haveItemData = JsonUtility.FromJson<Data>(File.ReadAllText(_jsonFilePathName));
    }
    /// <summary>
    /// プレイヤーが所持している"アイテムの所持数"を
    /// jsonファイルから読み込みフィールドに保存する。
    /// </summary>
    public void OnSavePlayerHaveItemData()
    {
        Debug.Log("<color=yellow>アイテム所持数をセーブします！</color>");
        // アイテム所持数データを、JSON形式にシリアライズし、ファイルに保存
        File.WriteAllText(_jsonFilePathName, JsonUtility.ToJson(_haveItemData, false));
    }
    /// <summary>
    /// アイテムの取得、喪失のメソッド。
    /// 特定のアイテムの所持数を変更する。 
    /// </summary>
    /// <param name="itemID"> 
    /// 変更するアイテムのID
    /// </param>
    /// <param name="volume"> 
    /// 変更する数 <br/>
    /// 例 : <br/>
    /// 1ならそのIDのアイテムを一つ増やす <br/>
    /// -1ならそのIDのアイテムを一つ減らす。<br/>
    /// </param>
    public void MakeChangesHaveItemData(int itemID, int volume)
    {
        // インデックスが正しいか判定
        if (itemID < (int)Item.ItemID.ITEM_ID_00 || itemID >= (int)Item.ItemID.ITEM_ID_END)
        {
            // インデックスが範囲外なら処理を抜ける
            Debug.LogError("無効なアイテムIDです。");
            return;
        }
        _haveItemData._itemVolume[itemID] += volume;
    }
}
