using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerAnimManager : MonoBehaviour
{
    //自身のコンポーネント
    BringerMain _bringerMain;
    Animator _animator;

    //各パラメータ
    bool _isIdele = false;
    bool _isApproach = false;
    bool _isRecession = false;

    bool _isLightAttack = false;
    bool _isHeavyAttack = false;
    bool _isLongRangeAttack = false;




    void Start()
    {
        _bringerMain = GetComponent<BringerMain>();
    }

    void Update()
    {
        SetAnimFalse();
        ChangeAnim();
        SetAnim();
    }

    void SetAnimFalse()
    {
        _isIdele = false;
        _isApproach = false;
        _isRecession = false;

        _isLightAttack = false;
        _isHeavyAttack = false;
        _isLongRangeAttack = false;
    }

    void ChangeAnim()
    {
        //アニメーションを設定する
        switch (_bringerMain._nowState)
        {
            case BringerMain.BossState.IDLE: _isIdele = true; break;
            case BringerMain.BossState.APPROACH: _isApproach = true; break;
            case BringerMain.BossState.RECESSION: _isRecession = true; break;

            case BringerMain.BossState.LIGHT_ATTACK: _isLightAttack = true; break;
            case BringerMain.BossState.HEAVY_ATTACK: _isHeavyAttack = true; break;
            case BringerMain.BossState.LONG_RANGE_ATTACK: _isLongRangeAttack = true; break;

            default: Debug.Log("Bringerに、このステートはありません。"); break;
        }
    }

    void SetAnim()
    {
        _animator.SetBool("isIdle", _isIdele);
        _animator.SetBool("isWalk", _isApproach);
        _animator.SetBool("isWalk", _isRecession);

        _animator.SetBool("isLightAttack", _isLightAttack);
        _animator.SetBool("isHeavyAttack", _isHeavyAttack);
        _animator.SetBool("isLongRangeAttack", _isLongRangeAttack);
    }
}
