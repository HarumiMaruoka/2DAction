using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> 持っている装備を管理するクラス </summary>
public class HaveEquipmentManager : MonoBehaviour
{
    //<======シングルトンパターン関連======>//
    private static HaveEquipmentManager _instance;
    public static HaveEquipmentManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("PlayerManager._instanceはnullです。");
            }
            return _instance;
        }
    }
    private HaveEquipmentManager() { }


    //<=========== 必要な値 ===========>//
    /// <summary> 所持している装備の配列 </summary>
    GameObject[] _haveEquipments;

    //<======== アサインすべき値 ========>//


    //<===== インスペクタから設定すべき値 =====>//
    /// <summary> プレイヤーが所持できる装備の最大数 </summary>
    [Header("プレイヤーが所持できる装備の最大数"), SerializeField] int _maxHaveValue;

    private void Awake()
    {
        //もしインスタンスが設定されていなかったら自身を代入する
        if (_instance == null)
        {
            _instance = this;
        }
        //もう既に存在する場合は、このオブジェクトを破棄する。
        else if (_instance != null)
        {
            Destroy(this);
        }
        //このオブジェクトは、シーンを跨いでもデストロイしない。
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
