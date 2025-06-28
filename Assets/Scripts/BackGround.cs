using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
	void Awake()
    {
        // リフレッシュレートに依存しない様に固定する
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

}
