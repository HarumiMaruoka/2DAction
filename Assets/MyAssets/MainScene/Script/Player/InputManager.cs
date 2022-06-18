using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    //入力保存用変数
    //横・縦の入力
    public float _inputHorizontal { get; private set; } = 0f;
    public float _inputVertical { get; private set; } = 0f;

    //Shiftキーの入力
    public bool _inputLeftShift { get; private set; } = false;

    //Spaceキーの入力
    public bool _inputJumpDown { get; private set; } = false;
    public bool _inputJump { get; private set; } = false;
    public bool _inputJumpUp { get; private set; } = false;

    //マウスの入力
    public bool _inputFire1 { get; private set; } = false;//左クリック
    public bool _inputFire1Up { get; private set; } = false;//左クリック離したとき
    public bool _inputFire2 { get; private set; } = false;//右クリック
    public bool _inputFire2Down { get; private set; } = false;//右クリック
    public bool _inputFire2Up { get; private set; } = false;//右クリック

    void Update()
    {
        ButtonSet();
    }

    void ButtonSet()
    {
        _inputHorizontal = Input.GetAxisRaw("Horizontal");
        _inputVertical = Input.GetAxisRaw("Vertical");

        _inputLeftShift = Input.GetButton("Dash");

        _inputJumpDown = Input.GetButtonDown("Jump");
        _inputJump = Input.GetButton("Jump");
        _inputJumpUp= Input.GetButtonUp("Jump");

        _inputFire1 = Input.GetButton("Fire1");
        _inputFire1Up = Input.GetButtonUp("Fire1");

        _inputFire2 = Input.GetButton("Fire2");
        _inputFire2Up = Input.GetButtonUp("Fire2");
        _inputFire2Down = Input.GetButtonDown("Fire2");
    }
}
