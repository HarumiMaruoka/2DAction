using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// アイテム管理者 : <br/>
/// 全てのアイテムの情報 <br/>
/// 現在所持しているアイテムの情報を持つ/提供する。<br/>
/// 他になにか提供するものが増えたりした場合ここに追記する。
/// </para>
/// このクラスはシングルトンで実装する。
/// </summary>
public class NewItemDataBase
{
    //===== シングルトン関連 =====//
    /// <summary> このクラスの唯一のインスタンス </summary>
    private static NewItemDataBase _instance = new NewItemDataBase();
    /// <summary> このクラス唯一のインスタンスのプロパティ </summary>
    public static NewItemDataBase Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError($"<color=yellow>エラー！ItemManagerのインスタンスがnullです！</color>");
            }
            return _instance;
        }
    }
    // コンストラクタ
    private NewItemDataBase() { }

    //===== フィールド =====//
    /// <summary> アイテムのデータベース </summary>
    private ItemDataBase _itemDataBase = new ItemDataBase();
    /// <summary> プレイヤーが所持しているアイテムの数 </summary>
    private PlayerHaveItemData _playerHaveItemData = new PlayerHaveItemData();

    //===== プロパティ =====//
    /// <summary> アイテムのデータベース </summary>
    public ItemDataBase AllItemDataBase => _itemDataBase;
    /// <summary> プレイヤーが所持しているアイテムの数 </summary>
    public PlayerHaveItemData PlayerHaveItemData => _playerHaveItemData;


    //===== publicメソッド =====//

    //===== privateメソッド =====//
}
