using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //���̃N���X�̓V���O���g���p�^�[�����g�p�������̂ł���B

    //�C���X�^���X�𐶐�
    private static GameManager _instance;

    //�C���X�^���X���J�v�Z����
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
    }

    private void Start()
    {
        //���̃X�N���v�g���A�^�b�`���ꂽ�I�u�W�F�N�g�́A�V�[�����ׂ��ł��f�X�g���C����Ȃ��悤�ɂ���B
        DontDestroyOnLoad(gameObject);

        //BGM�Ǘ��p�ϐ���������
        //_bgm = GetComponent<AudioSource>();
        //if (_bgm == null)
        //{
        //    Debug.LogError("�Q�[���}�l�[�W���[��AudioSource Component���A�^�b�`���Ă�������!");
        //}
    }


    //���Ƃ�BGMManager�ɏ���
    /// <summary> BGM��؂�ւ��� </summary>
    //public void ChangeBGM(AudioClip newAudio)
    //{
    //    _bgm.clip = newAudio;
    //    _bgm.Play();
    //}
}
