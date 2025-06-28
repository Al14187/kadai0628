using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FromCubeToWall : MonoBehaviour
{
	/// <summary>
	/// 壁
	/// </summary>
	[SerializeField]
	private Wall wall_;

	/// <summary>
	/// キューブからラインまでの最短距離を計算する
	/// </summary>
	void Update()
	{
		// 平行移動
		Vector3 move = new Vector3();
		if (Input.GetKey(KeyCode.UpArrow))
		{
			move.z = 0.5f;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			move.z = -0.5f;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			move.x = 0.5f;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			move.x = -0.5f;
		}
		transform.position += move;


		// 壁の端の位置
		var start = wall_.edgePosition;

		// 壁の端の位置からキューブまでのベクトル
		var dir = transform.position - start;

		// 壁の端から端までの向きの単位ベクトル
		var edgeToEdgeNormal = wall_.fromEdgeToEdge.normalized;

		// ベクトルの内積を計算し、射影距離を求める
		var dest = Vector3.Dot(dir, edgeToEdgeNormal);
		// 壁の端から端までの長さを超えないように制限する
		dest = Mathf.Clamp(dest, 0.0f, wall_.fromEdgeToEdge.magnitude);

		// 射影した際の距離を単位ベクトルに掛け合わせて、壁までの最短を計算する	
		var end = (edgeToEdgeNormal * dest) + start;

		// 線を描画する
		var line = GetComponent<LineRenderer>();
		line.SetPositions(new Vector3[] { transform.position, end });
	}
}
