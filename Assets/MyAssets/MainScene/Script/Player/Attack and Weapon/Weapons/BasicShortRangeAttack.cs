using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 基本的な近距離攻撃クラス。
/// </summary>
public class BasicShortRangeAttack : FireBehavior
{
    //<===== メンバー変数 =====>//
    [Header("近距離攻撃 : コンボ一撃目のプレハブ"), SerializeField]
    GameObject _shortRangeAttack_FirstComboPrefab = default;
    [Header("近距離攻撃 : コンボ二撃目のプレハブ"), SerializeField]
    GameObject _shortRangeAttack_SecondComboPrefab = default;
    [Header("近距離攻撃 : コンボ三撃目のプレハブ"), SerializeField]
    GameObject _shortRangeAttack_ThirdComboPrefab = default;

    /// <summary> 2コンボ目が撃てるかどうか </summary>
    bool _isSecondCombo = false;
    /// <summary> 3コンボ目が撃てるかどうか </summary>
    bool _isThirdCombo = false;
    /// <summary> コンボ間のインターバル </summary>
    [Header("各コンボ間のインターバル"), SerializeField] float _intervalBetweenCombos = 0.8f;

    //<===== Unityメッセージ =====>//
    void Start()
    {
        Initialized(Constants.ON_FIRE_PRESS_TYPE_MOMENT);

        //***テスト用処理***//
        //*右腕に装備する。*//
        SetEquip_RightArm();
        //******************//
    }
    void Update()
    {
        
    }

    //<===== overrides =====>//
    protected override bool Initialized(bool pressType)
    {
        return base.Initialized(pressType);
    }
    protected override void OnFire_ThisWeapon()
    {
        //3撃目を撃つ
        if (_isThirdCombo)
        {
            Instantiate(_shortRangeAttack_ThirdComboPrefab);
            _isThirdCombo = false;
        }
        //2撃目を撃つ
        else if (_isSecondCombo)
        {
            Instantiate(_shortRangeAttack_SecondComboPrefab);
            //3撃目準備完了
            StartCoroutine(ThirdShotReady());
            _isSecondCombo = false;
        }
        //1撃目を撃つ
        else
        {
            Instantiate(_shortRangeAttack_FirstComboPrefab);
            //2撃目準備完了
            StartCoroutine(SecondShotReady());
        }
    }

    //<===== コルーチン =====>//
    /// <summary> 一定時間、2コンボ目を撃てるようにする。 </summary>
    IEnumerator SecondShotReady()
    {
        _isSecondCombo = true;
        yield return new WaitForSeconds(_intervalBetweenCombos);
        _isSecondCombo = false;
    }
    /// <summary> 一定時間、3コンボ目を撃てるようにする。 </summary>
    IEnumerator ThirdShotReady()
    {
        _isThirdCombo = true;
        yield return new WaitForSeconds(_intervalBetweenCombos);
        _isThirdCombo = false;
    }
}
