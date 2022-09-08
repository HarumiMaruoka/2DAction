using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 攻撃処理
/// </summary>
public class NewPlayerAttack : MonoBehaviour
{
    //<===== メンバー変数 =====>//

    /// <summary> <para>左腕に装備された武器の攻撃処理。デリゲート変数。</para>
    /// <para>"左クリック時に一回だけ実行する。"</para> </summary>
    static public System.Action _playerLeftArmWeapon_Moment;
    /// <summary> <para>右腕に装備された武器の攻撃処理。デリゲート変数。</para>
    /// <para>"右クリック時に一回だけ実行する。"</para> </summary>
    static public System.Action _playerRightArmWeapon_Moment;

    /// <summary> <para>左腕に装備された武器の攻撃処理。デリゲート変数。</para>
    /// <para>"左クリック中ずっと実行する。"</para> </summary>
    static public System.Action _playerLeftArmWeapon_Consecutively;
    /// <summary> <para>右腕に装備された武器の攻撃処理。デリゲート変数。</para>
    /// <para>"右クリック中ずっと実行する。"</para> </summary>
    static public System.Action _playerRightArmWeapon_Consecutively;

    /// <summary> FireOne入力判定用 </summary>
    bool _isFireOneDown = false;
    /// <summary> FireTow入力判定用 </summary>
    bool _isFireTowDown = false;
    /// <summary> FireOne入力判定用 </summary>
    bool _isFireOneDownNow = false;
    /// <summary> FireTow入力判定用 </summary>
    bool _isFireTowDownNow = false;

    [Header("左クリックのボタンの名前"), SerializeField] string FireOneButtonName = "Fire1";
    [Header("右クリックのボタンの名前"), SerializeField] string FireTowButtonName = "Fire2";

    //<===== Unityメッセージ =====>//
    void Update()
    {
        Input_Attack();
        Update_Attack();
    }
    //<===== privateメンバー関数 =====>//
    /// <summary> 入力処理 </summary>
    void Input_Attack()
    {
        _isFireOneDown = Input.GetButtonDown(FireOneButtonName);
        _isFireTowDown = Input.GetButtonDown(FireTowButtonName);
        _isFireOneDownNow = Input.GetButton(FireOneButtonName);
        _isFireTowDownNow = Input.GetButton(FireTowButtonName);
    }
    /// <summary> 更新処理 </summary>
    void Update_Attack()
    {
        // 左クリック押下時の処理 デリゲート変数に登録された処理を実行する。
        if (_isFireOneDown && _playerLeftArmWeapon_Moment != null)
        {
            _playerLeftArmWeapon_Moment();
        }
        // 右クリック押下時の処理 デリゲート変数に登録された処理を実行する。
        if (_isFireTowDown && _playerRightArmWeapon_Moment != null)
        {
            _playerRightArmWeapon_Moment();
        }
        // 左クリック押下中の処理 デリゲート変数に登録された処理を実行する。
        if (_isFireOneDownNow && _playerLeftArmWeapon_Consecutively != null)
        {
            _playerLeftArmWeapon_Consecutively();
        }
        // 右クリック押下中の処理 デリゲート変数に登録された処理を実行する。
        if (_isFireTowDownNow && _playerRightArmWeapon_Consecutively != null)
        {
            _playerRightArmWeapon_Consecutively();
        }
    }
}
