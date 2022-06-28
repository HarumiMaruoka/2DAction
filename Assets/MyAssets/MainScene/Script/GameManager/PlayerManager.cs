using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    //このクラスはシングルトンパターンを使用したものである。
    //インスタンスを生成
    private static GameManager _instance;
    //インスタンスを読み取り専用かつインスタンスがなければインスタンスを生成し保存する
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    /// <summary> アイテムの所持数 </summary>
    int[] _itemHaveVolume = new int[(int)Item.ItemID.ITEM_ID_END];
    public int[] ItemHaveVolume
    {
        get
        {
            return _itemHaveVolume;
        }
    }

    public void SetItemHaveVolume(int index,int value)
    {
        _itemHaveVolume[index] = value;
    }

    //装備情報の格納先

    void Start()
    {

    }

    void SetItem()
    {
        //アイテム情報をスプレッドシートから読み込み、配列に保存する
    }
}
