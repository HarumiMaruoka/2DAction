using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] string _mainSceneName;
    [SerializeField] string _titleSceneName;

    //Main�V�[����ǂݍ���
    public void LoadMainScene()
    {
        SceneManager.LoadScene(_mainSceneName);
    }

    public void LoadTitleScene()
    {
        SceneManager.LoadScene(_titleSceneName);
    }

    public void GameExit()
    {
        //UnityEditor��ŏI������Ƃ��̏����B(�r���h���̓R�����g�A�E�g����B)
        UnityEditor.EditorApplication.isPlaying = false;
        //���s�t�@�C�����ł̏I������
        //UnityEngine.Application.Quit();
    }
}
