using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
	/// <summary>
	/// 壁の両端
	/// </summary>
	[SerializeField]
	private GameObject[] edge_ = new GameObject[2];

	/// <summary>
	/// 壁の端の位置
	/// </summary>
	public Vector3 edgePosition { get => edge_[0].transform.position;  }

	/// <summary>
	/// 端から端までのベクトル
	/// </summary>
	public Vector3 fromEdgeToEdge { get => edge_[1].transform.position - edge_[0].transform.position; }


	void Awake()
    {
        // リフレッシュレートに依存しない様に固定する
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
    }

}
