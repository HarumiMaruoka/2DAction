using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

/// <summary>
/// �Q�[���S�̂Ŏg�p�\�ȃN���X�B�����ɉ����L�q���Ă��Ȃ��̂ŕK�v�Ȃ���������Ȃ��B
/// </summary>
public class GameManager : MonoBehaviour
{    
    /// <summary> �������ɐ����������ǂ�����\���ϐ��B </summary>
    bool _isInitialized = false;

    [Header("�t�F�[�h�C��/�A�E�g�p�̃p�l��"), SerializeField] GameObject _fadePanel;
    [Header("�t�F�[�h�C���Ɋ|���鎞��"),SerializeField] float _timeToFadeIn;

    public void FadeIn(string nextSceneName)
    {
        //�t�F�[�h�C���p�p�l�����A�N�e�B�u�ɂ���B
        _fadePanel.SetActive(true);
        //DOTween�𗘗p���ăt�F�[�h�C������B
        _fadePanel.
            GetComponent<Image>().
            DOFade(1, _timeToFadeIn).//_timeToFadeIn�b�|���āA�p�l���̃A���t�@�l��1�ɂ���B
            SetDelay(0.5f).          //0.5�b�҂��Ď��s����B
            OnComplete               //�t�F�[�h�C��������������A�V�[����ǂݍ��ށB
            (
                () => SceneManager.LoadScene(nextSceneName)
            );
    }
    public void FadeOut()
    {

    }

    //<===== Unity���b�Z�[�W =====>//
    void Awake()
    {

    }
    void Start()
    {

    }
    void Update()
    {

    }

    //<===== privete�����o�[�֐� =====>//
    /// <summary> �������֐� </summary>
    /// <returns> �������ɐ��������� true ��Ԃ��B </returns>
    bool Initialized()
    {

        return true;
    }
}
