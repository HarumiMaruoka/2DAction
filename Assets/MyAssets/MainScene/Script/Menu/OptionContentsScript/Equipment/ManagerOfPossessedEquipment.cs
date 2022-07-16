using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//所持している装備を管理するクラス
public class ManagerOfPossessedEquipment : MonoBehaviour
{
    //<=========== 必要な値 ===========>//
    /// <summary> 装備ボタンの配列 </summary>
    GameObject[] _equipmentButton;

    //<======== アサインすべき値 ========>//
    /// <summary> 装備ボタンのプレハブ </summary>
    [Header("装備ボタンのプレハブ"), SerializeField] GameObject _equipmentButtonPrefab;

    //<===== インスペクタから設定すべき値 =====>//
    /// <summary> プレイヤーが所持できる装備の最大数 </summary>
    [Header("プレイヤーが所持できる装備の最大数"), SerializeField] int _maxHaveValue;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
