using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ドロップする"装備"のコンポーネント<br/>
/// 落とす装備のIDは敵からしてください。<br/>
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
public class DropEquipment : MonoBehaviour, IDrops
{
    //===== フィールド / プロパティ =====//
    /// <summary> プレイヤーが取得するアイテムのID </summary>
    EquipmentID _getID = 0;
    /// <summary> 発生時に掛ける力 </summary>
    const float _forcePower = 5f;


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
                Debug.Log($"{EquipmentDataBase.Instance.EquipmentData[(int)_getID]._myName}を取得しました!");
                // 装備を増やす。
                if (EquipmentDataBase.Instance.Get_Equipment((int)_getID))
                {
                    Debug.Log("装備の取得に成功しました。");
                }
                else
                {
                    Debug.Log("装備の取得に失敗しました。");
                }
                Destroy(gameObject);
            }
            catch (IndexOutOfRangeException e)
            {
                Debug.LogError($"IndexOutOfRangeException : {e.StackTrace}");
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
        _getID = (EquipmentID)id;
    }
}
