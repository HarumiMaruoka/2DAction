using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドロップする"アイテム"のコンポーネント<br/>
/// 落とすアイテムのIDは敵から設定してください。<br/>
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class DropItem : MonoBehaviour, IDrops
{
    //====== フィールド / プロパティ =====//
    /// <summary> プレイヤーが取得するアイテムのID </summary>
    Item.ItemID _getID = 0;
    /// <summary> 発生時に掛ける力 </summary>
    const float _forcePower = 5f;

    //===== Unityメッセージ =====//
    void Start()
    {
        Initialized();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Constants.PLAYER_TAG_NAME)
        {
            try
            {
                // 取得時にメッセージを表示する。
                Debug.Log($"{ItemDataBase.Instance.ItemData[(int)_getID]._name}を取得しました!");
                // アイテムを増やす。
                ItemDataBase.Instance.MakeChanges_ItemNumberOfPossessions((int)_getID, 1);
                Destroy(gameObject);
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError($"{gameObject.name}のフィールド\"_getID=>{_getID}は範囲外です。");
            }
        }
    }

    //===== メソッド =====//
    /// <summary>
    /// 初期化処理
    /// </summary>
    void Initialized()
    {
        // 生まれた瞬間に、上に力を加える。
        var randomUpVector = new Vector2(Mathf.Cos(UnityEngine.Random.Range(0f, Mathf.PI)), 1f);
        GetComponent<Rigidbody2D>().AddForce(randomUpVector.normalized * _forcePower, ForceMode2D.Impulse);
    }

    public void SetID(int id)
    {
        _getID = (Item.ItemID)id;
    }
}
