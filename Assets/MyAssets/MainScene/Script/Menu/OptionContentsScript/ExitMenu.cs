using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitMenu : MonoBehaviour
{
    //�Q�[�����I������
    public void GameExit()
    {
        //�G�f�B�^�[��ŏI������ꍇ�̏���(�r���h���ɂ͏���)
        UnityEditor.EditorApplication.isPlaying = false;
        //�r���h�����Q�[�����I������ꍇ�̏���(�G�f�B�^�[���s���̓R�����g�A�E�g����)
        //Application.Quit();
    }

    //�Q�[�����I�����Ȃ�
    public void CancelExit()
    {
        //���j���[��ʂɖ߂�
        gameObject.SetActive(false);
    }
}
