using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �v���C���[�̍U�����Ǘ�����N���X </summary>
public class NewPlayerAttack : MonoBehaviour
{
    /// <summary> Fire1�����������Ǝ��s����f���Q�[�g�ϐ��B </summary>
    static public System.Action On_Fire1Action_OnButton;
    /// <summary> Fire2�����������Ǝ��s����f���Q�[�g�ϐ��B </summary>
    static public System.Action On_Fire2Action_OnButton;

    /// <summary> Fire1�������Ɏ��s����f���Q�[�g�ϐ��B </summary>
    static public System.Action On_Fire1Action_OnButtonDown;
    /// <summary> Fire2�������Ɏ��s����f���Q�[�g�ϐ��B </summary>
    static public System.Action On_Fire2Action_OnButtonDown;

    void Start()
    {

    }

    void Update()
    {
        //�U������
        //�����������Ǝ��s����B
        if (Input.GetButton("Fire1"))
        {
            //On_Fire1Action();
        }
        if (Input.GetButton("Fire2"))
        {
            //On_Fire2Action();
        }
        //�������̂ݎ��s����B
        if (Input.GetButtonDown("Fire1"))
        {
            
        }
        if (Input.GetButtonDown("Fire2"))
        {

        }
    }
}
