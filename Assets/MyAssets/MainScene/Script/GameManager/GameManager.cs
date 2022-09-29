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
    //===== �t�B�[���h / �v���p�e�B =====//

    /// <summary> �������ɐ����������ǂ�����\���ϐ��B </summary>
    bool _isInitialized = false;

    [Header("�t�F�[�h�C�� / �A�E�g�֘A")]
    [Tooltip("�t�F�[�h�C��/�A�E�g�p�̃p�l��"), SerializeField]
    GameObject _fadePanel;
    [Tooltip("�t�F�[�h�C���Ɋ|���鎞��"), SerializeField]
    float _timeToFadeIn;

    // �|�[�Y�J�n���Ɏ��s����f���Q�[�g
    static public System.Action OnPause = default;
    // �|�[�Y�������Ɏ��s����f���Q�[�g
    static public System.Action OnResume = default;

    bool _isPause = false;


    //===== Unity���b�Z�[�W =====//
    void Awake()
    {

    }
    void Start()
    {

    }
    void Update()
    {

    }

    //===== privete���\�b�h =====//
    /// <summary> �������֐� </summary>
    /// <returns> �������ɐ��������� true ��Ԃ��B </returns>
    bool Initialized()
    {

        return true;
    }

    //===== public���\�b�h =====//
    // �t�F�[�h�C������
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
    // �t�F�[�h�A�E�g����
    public void FadeOut()
    {

    }

    //===== �{�^������Ăяo���z��ō쐬�������\�b�h =====//
    /// <summary>
    /// �|�[�Y�J�n����
    /// </summary>
    public void OnPauseStart()
    {
        if (!_isPause)
        {
            _isPause = true;
            OnPause();
        }
    }
    /// <summary>
    /// �|�[�Y�I������
    /// </summary>
    public void OnPauseEnd()
    {
        if (_isPause)
        {
            _isPause = false;
            OnResume();
        }
    }
    /// <summary>
    /// �V�[����ύX����B
    /// </summary>
    /// <param name="nextSceneName"> �ǂݍ��ރV�[���� </param>
    public void SceneChange(string nextSceneName)
    {
        SceneManager.LoadScene(nextSceneName);
    }
}
