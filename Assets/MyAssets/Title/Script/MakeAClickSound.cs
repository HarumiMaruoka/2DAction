using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ボタンをクリックしたときに音を鳴らす為のコンポーネント
/// </summary>
public class MakeAClickSound : MonoBehaviour
{
    public void MakeASound()
    {
        GetComponent<AudioSource>().Play();
    }
}
