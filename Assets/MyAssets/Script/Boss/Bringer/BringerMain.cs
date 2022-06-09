using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BringerMain : BossBase
{
    //�e�p�����[�^
    [Tooltip("��U����̃N�[���^�C��"), SerializeField] CoolTimeRandomValue _lightAttackCoolTime;
    [Tooltip("��U����̃N�[���^�C��"), SerializeField] CoolTimeRandomValue _heavyAttackCoolTime;
    [Tooltip("��U����̃N�[���^�C��"), SerializeField] CoolTimeRandomValue _longRangeAttackCoolTime;

    //�����̏��
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
