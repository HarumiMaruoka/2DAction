using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�Q�[���̏I������
public class ExitMenu : MonoBehaviour
{
    //�Q�[�����I������
    public void GameExit()
    {
        //�G�f�B�^�[��ŏI������ꍇ�̏���(�r���h���ɂ̓G���[�𓊂���̂ŏ���)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_EDITOR_WIN
        //�r���h�����Q�[�����I������ꍇ�̏���(�G�f�B�^�[���s���̓R�����g�A�E�g����)
        Application.Quit();
#endif
    }
}
