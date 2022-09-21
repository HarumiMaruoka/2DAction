using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// �C�x���g�V�X�e���𗘗p����N���X���p�����ׂ��N���X�B
/// �C�x���g�V�X�e���𗘗p���邤���ŁA�l�X�ȕ֗��@�\��񋟂���B
/// </summary>
public class UseEventSystemBehavior : MonoBehaviour
{
    //<===== �����o�[�ϐ� =====>//
    /// <summary> �C�x���g�V�X�e�� </summary>
    protected EventSystem _eventSystem;
    /// <summary> �O�t���[���őI�����Ă����Q�[���I�u�W�F�N�g(Button) </summary>
    protected GameObject _beforeSelectedGameObject;

    //<===== �����������n =====>//
    /// <summary> UseEventSystemBehavior�N���X�̏��������� </summary>
    /// <returns> �����������̉ۂ�Ԃ��B���������� true �B </returns>
    protected bool Init()
    {
        if ((_eventSystem = GameObject.FindObjectOfType<EventSystem>()) == null)
        {
            Debug.Log("\"EventSystem\"�̎擾�Ɏ��s���܂����B");
            return false;
        }
        return true;
    }
}
