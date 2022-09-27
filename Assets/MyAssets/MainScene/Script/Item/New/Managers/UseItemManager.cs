using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

/// <summary>
/// アイテム使用を管理するマネージャー。<br/>
/// ゲームオブジェクトで表現すべき全てのアイテムのプレハブを持つ。<br/>
/// 使用時はここからインスタンシエイトしてください。<br/>
/// </summary>
public class UseItemManager : MonoBehaviour
{
    //===== シングルトン関連 =====//
    private static UseItemManager _instance = default;
    public static UseItemManager Instance
    {
        get
        {
            if (_instance == null) Debug.LogError("エラー！修正してください！");
            return _instance;
        }
    }


    //===== フィールド / プロパティ =====//
    [Header("アサインすべき値群")]
    [Tooltip("ItemID:03 を表現するためのゲームオブジェクト"), SerializeField]
    public GameObject _itemID3 = default;
    [Tooltip("ItemID:04 を表現するためのゲームオブジェクト"), SerializeField]
    public GameObject _itemID4 = default;
    [Tooltip("ItemID:05 を表現するためのゲームオブジェクト"), SerializeField]
    public GameObject _itemID5 = default;
    [Tooltip("ItemID:06 を表現するためのゲームオブジェクト"), SerializeField]
    public GameObject _itemID6 = default;
    /// <summary> 攻撃力上昇量 : 監視する値 </summary>
    IntReactiveProperty _offensivePowerUpValue = new IntReactiveProperty(100);
    /// <summary> 敵の移動速度減退を表す変数。 </summary>
    public float _enemyMoveSpeedDownValue { get; set; } = 0f;

    //===== Unityメッセージ =====//
    void Awake()
    {
        _instance = this;
    }
    void Start()
    {
        Init();
    }

    //===== privateメソッド =====//
    /// <summary> 初期化処理 </summary>
    void Init()
    {
        // アサインすべき全ての値のnullチェックを行う。
        if (_itemID3 == null) Debug.LogError($"{gameObject.name}アサインしてください!");
        if (_itemID4 == null) Debug.LogError($"{gameObject.name}アサインしてください!");
        if (_itemID5 == null) Debug.LogError($"{gameObject.name}アサインしてください!");
        if (_itemID6 == null) Debug.LogError($"{gameObject.name}アサインしてください!");

        // プレイヤーの攻撃力の変化を検知し、変化時に実行するメソッドを登録する。
        _offensivePowerUpValue.Skip(1).
            Subscribe(_ => RunWhenChanged_OffensivePowerUpValue());
    }
    /// <summary>
    /// 攻撃力上昇値が変更された時に実行する。<br/>
    /// 攻撃力上昇値をプレイヤーステータスに適用する。<br/>
    /// </summary>
    void RunWhenChanged_OffensivePowerUpValue()
    {
        var risingValue = PlayerStatusManager.Instance.Equipment_RisingValue + PlayerStatusManager.Instance.BaseStatus;
        risingValue._shortRangeAttackPower *= (float)_offensivePowerUpValue.Value * 0.01f;
        risingValue._longRangeAttackPower *= (float)_offensivePowerUpValue.Value * 0.01f;
        PlayerStatusManager.Instance.Other_RisingValue = risingValue;
        Debug.Log($"現在のプレイヤーの遠距離攻撃力は\n" +
            $"{PlayerStatusManager.Instance.ConsequentialPlayerStatus._longRangeAttackPower}だぜ");
        Debug.Log($"現在のプレイヤーの近距離攻撃力は\n" +
            $"{PlayerStatusManager.Instance.ConsequentialPlayerStatus._shortRangeAttackPower}だぜ");
        Debug.Log($"_offensivePowerUpValueの値は現在{_offensivePowerUpValue}です");
    }

    //===== publicメソッド =====//
    /// <summary> 攻撃力を変化させる。 </summary>
    /// <param name="changeValue"> 変化量 </param>
    public void OffensivePowerUpValueChange(int changeValue)
    {
        _offensivePowerUpValue.Value += changeValue;
    }
    public void EnemyMoveSpeedDownValueChange(float changeValue)
    {
        _enemyMoveSpeedDownValue += changeValue;
    }
}
