using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectDestroy : MonoBehaviour
{
    //アニメーションイベントで呼び出す
    public void ThisObjectDestroy()
    {
        Destroy(this.gameObject);
    }

}
