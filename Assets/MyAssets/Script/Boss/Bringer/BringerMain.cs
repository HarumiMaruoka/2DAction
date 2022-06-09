using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerMain : BossBase
{
    //各パラメータ
    [Tooltip("弱攻撃後のクールタイム"), SerializeField] CoolTimeRandomValue _lightAttackCoolTime;
    [Tooltip("弱攻撃後のクールタイム"), SerializeField] CoolTimeRandomValue _heavyAttackCoolTime;
    [Tooltip("弱攻撃後のクールタイム"), SerializeField] CoolTimeRandomValue _longRangeAttackCoolTime;

    //自分の状態
    BossState _nowState;

    void Start()
    {
        InitBoss();
    }

    void Update()
    {
        base.BossUpdate();
    }
}
