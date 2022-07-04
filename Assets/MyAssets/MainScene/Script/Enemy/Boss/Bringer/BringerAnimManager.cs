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
    bool _isWalk = false;

    bool _isLightAttack = false;
    bool _isHeavyAttack = false;
    bool _isLongRangeAttack = false;

    bool _isDie = false;


    void Start()
    {
        _bringerMain = GetComponent<BringerMain>();
        _animator = GetComponent<Animator>();
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
        _isWalk = false;

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
            case BringerMain.BossState.APPROACH: _animator.SetFloat("WalkSpeed", 1f); _isWalk = true; break;
            case BringerMain.BossState.RECESSION: _animator.SetFloat("WalkSpeed", -1f); _isWalk = true; break;

            case BringerMain.BossState.LIGHT_ATTACK: _isLightAttack = true; break;
            case BringerMain.BossState.HEAVY_ATTACK: _isHeavyAttack = true; break;
            case BringerMain.BossState.LONG_RANGE_ATTACK: _isLongRangeAttack = true; break;

            case BringerMain.BossState.DIE: _isDie = true; break;

            default: Debug.LogError("Bringerに、このステートはありません。"); break;
        }
    }

    void SetAnim()
    {
        _animator.SetBool("isIdle", _isIdele);
        _animator.SetBool("isWalk", _isWalk);

        _animator.SetBool("isLightAttack", _isLightAttack);
        _animator.SetBool("isHeavyAttack", _isHeavyAttack);
        _animator.SetBool("isLongRangeAttack", _isLongRangeAttack);

        _animator.SetBool("isDie", _isDie);
    }
}
