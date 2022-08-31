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
    protected bool Initialized_EventSystemBehavior()
    {
        Debug.Log("ggggggggggggggggggggggggggggggg");
        if (!(_eventSystem = GameObject.FindObjectOfType<EventSystem>())) return false;
        return true;
    }

    //<===== �֗��n =====>//
    /// <summary> 
    /// �I�����Ă����I�u�W�F�N�g�̕ω������m����B
    /// �g�p����ꍇ�AUpdate_UseEventSystemBehavior()��Update()�ŌĂ�ł��������B 
    /// </summary>
    /// <returns> �ω������m�����t���[���� true ��Ԃ��B </returns>
    protected bool IsChangeSelectedObject()
    {
        return _eventSystem.currentSelectedGameObject != _beforeSelectedGameObject;
    }
    /// <summary> 
    /// UseEventSystemBehavior�̃A�b�v�f�[�g�֐��B
    /// </summary>
    protected void Update_UseEventSystemBehavior()
    {
        //���t���[���őI�����Ă����I�u�W�F�N�g��ۑ��B
        _beforeSelectedGameObject = _eventSystem.currentSelectedGameObject;
    }
}
