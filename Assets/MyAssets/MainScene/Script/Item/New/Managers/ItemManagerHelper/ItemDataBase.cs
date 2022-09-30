using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AddressableAssets;

/// <summary>
/// 全てのアイテムデータを管理するクラス <br/>
/// </summary>
public class ItemDataBase
{
    // コンストラクタ
    public ItemDataBase()
    {
        Init();
    }
    //===== Constant =====//
    /// <summary>
    /// アイテムデータを保存したテキストファイルのAddressables Name <br/>
    /// </summary>
    private const string _itemCsvAddressablesName = "ItemData";

    //===== Field =====//
    /// <summary>
    /// 全てのアイテムデータ <br/>
    /// </summary>
    private Item[] _itemData = new Item[(int)Item.ItemID.ITEM_ID_END];

    //===== Property =====//
    /// <summary>
    /// 全てのアイテムのデータ
    /// </summary>
    public Item[] ItemData => _itemData;

    //===== private Method =====//
    /// <summary>
    /// 初期化処理
    /// </summary>
    private void Init()
    {
        OnLoadItemData();
        Debug.Log("<color=yellow>ItemDataBase 初期化終了</color>");
    }
    /// <summary>
    /// アイテムデータを読み込みフィールドに保存する。
    /// </summary>
    private void OnLoadItemData()
    {
        try
        {
            // Addressable Assets System から Asset をロードする。
            Addressables.LoadAssetAsync<TextAsset>(_itemCsvAddressablesName).Completed +=
                (data) =>
                {
                    // ファイルを開く
                    var sr = new StringReader(data.Result.text);
                    bool isFirstLine = true;
                    int index = 0;
                    // ファイルから読み込み、フィールドに保存する。
                    string value = "";
                    while ((value = sr.ReadLine()) != null)// 末尾まで繰り返す
                    {
                        //最初の行(ヘッダーの行)はスキップする
                        if (isFirstLine)
                        {
                            isFirstLine = false;
                            continue;
                        }

                        string[] values = value.Split(',');

                        //種類別で生成し保存する
                        switch (values[2])
                        {
                            case "HealItem": _itemData[index] = new HealItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.HEAL, int.Parse(values[3]), values[4]); break;
                            case "PowerUpItem": _itemData[index] = new PowerUpItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.POWER_UP, int.Parse(values[3]), values[4]); break;
                            case "MinusItem": _itemData[index] = new MinusItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.MINUS_ITEM, int.Parse(values[3]), values[4]); break;
                            case "KeyItem": _itemData[index] = new KeyItem((Item.ItemID)int.Parse(values[0]), values[1], Item.ItemType.KEY, int.Parse(values[3]), values[4]); break;
                            default: Debug.LogError("設定されていないItemTypeです。"); break;
                        }
                        index++;
                    }
                };
        }
        catch (Exception ex)
        {
            Debug.LogError(
                $"<color=yellow>エラー！{ex.StackTrace}\n" +
                $"修正してください！</color>");
        }
    }
}
