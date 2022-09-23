using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 装備のUIに関するコンポーネントを集めたクラス。<br/>
/// ここにアクセスすれば、"GetComponent()しなくても"<br/>
/// 他のUIとの連携を取れるようにすることが目的<br/>
/// </summary>
public class EquipmentUIComponentsManager : MonoBehaviour
{
    //===== インスペクタから設定すべき値群 =====//
    // 装備UIにアタッチしている全てのコンポーネントをここに記述してください。
    // 又、nullチェックをしている箇所にも記述してください。
    [Header("全ての装備のUIを制御するコンポーネント : 必ずアサインしてください")]

    [Tooltip("現在装備しているパーツを表示するコンポーネント"), SerializeField]
    Draw_NowEquipped _draw_NowEquipped = default;
    [Tooltip("選択されているパーツを装備することによる変化量を描画するコンポーネント"), SerializeField]
    DrawAlteration _drawAlteration = default;
    [Tooltip("装備画面に、プレイヤーステータスを描画するコンポーネント。"), SerializeField]
    DrawPlayerStatus_OnEquipmentWindow _drawPlayerStatus_OnEquipmentWindow = default;
    [Tooltip("所持している装備のボタンを管理するコンポーネント"), SerializeField]
    ManagerOfPossessedEquipment _managerOfPossessedEquipment = default;

    //===== プロパティ =====//
    Draw_NowEquipped Draw_NowEquippedReference => _draw_NowEquipped;
    DrawAlteration DrawAlterationReference => _drawAlteration;
    DrawPlayerStatus_OnEquipmentWindow DrawPlayerStatus_OnEquipmentWindowReference => _drawPlayerStatus_OnEquipmentWindow;
    ManagerOfPossessedEquipment ManagerOfPossessedEquipmentReference => _managerOfPossessedEquipment;

    //===== Unityメッセージ =====//
    void Start()
    {
        if (Init())
        {
            Debug.Log($"\"{gameObject.name}\"の初期化に成功しました。");
        }
        else
        {
            Debug.LogError(
                $"\"{gameObject.name}\"の初期化に失敗しました！\n" +
                $"正しくアサインされているかチェックしてください！");
        }
    }

    /// <summary>
    /// 初期化処理 : <br/>
    /// 全てのアサインされた値に対してnullチェックを行い成功の可否を返す。<br/>
    /// アサインすべき値が増えた時はここに追記してください。<br/>
    /// </summary>
    /// <returns></returns>
    private bool Init()
    {
        if (_draw_NowEquipped == null) return false;
        if (_drawAlteration == null) return false;
        if (_drawPlayerStatus_OnEquipmentWindow == null) return false;
        if (_managerOfPossessedEquipment == null) return false;

        return true;
    }
}
