using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionManager : MonoBehaviour
{
    //�I�v�V�����Ǘ��p�X�N���v�g
    //�I�v�V�����̃R���e���c
    [SerializeField] GameObject _exitPanel;
    [SerializeField] GameObject _toolPanel;

    /// <summary> �I���m�F�_�C�A���O��\������B </summary>
    public void OnExitButton()
    {
        if (_exitPanel == null)
        {
            Debug.LogError("Exit�p�l���̎擾�Ɏ��s���܂����B");
        }
        _exitPanel.SetActive(true);
    }

    /// <summary> �����ʂ�\������B </summary>
    public void OnToolButton()
    {
        if (_toolPanel == null)
        {
            Debug.LogError("Tool�p�l���̎擾�Ɏ��s���܂����B");
        }
        _toolPanel.SetActive(true);
    }
}
