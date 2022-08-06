using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> プレイヤーの攻撃を管理するクラス </summary>
public class NewPlayerAttack : MonoBehaviour
{
    /// <summary> Fire1押下中ずっと実行するデリゲート変数。 </summary>
    static public System.Action On_Fire1Action_OnButton;
    /// <summary> Fire2押下中ずっと実行するデリゲート変数。 </summary>
    static public System.Action On_Fire2Action_OnButton;

    /// <summary> Fire1押下時に実行するデリゲート変数。 </summary>
    static public System.Action On_Fire1Action_OnButtonDown;
    /// <summary> Fire2押下時に実行するデリゲート変数。 </summary>
    static public System.Action On_Fire2Action_OnButtonDown;

    void Start()
    {

    }

    void Update()
    {
        //攻撃処理
        //押下中ずっと実行する。
        if (Input.GetButton("Fire1"))
        {
            //On_Fire1Action();
        }
        if (Input.GetButton("Fire2"))
        {
            //On_Fire2Action();
        }
        //押下時のみ実行する。
        if (Input.GetButtonDown("Fire1"))
        {
            
        }
        if (Input.GetButtonDown("Fire2"))
        {

        }
    }
}
