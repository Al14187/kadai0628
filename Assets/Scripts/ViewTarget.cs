using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewTarget : MonoBehaviour
{

	/// <summary>
	/// ターゲット
	/// </summary>
	[SerializeField]
	private GameObject target_;

	/// <summary>
	/// 更新処理
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


		if (Input.GetKeyDown(KeyCode.Z))
		{
			// ターゲットまでの向きの単位ベクトル
			var toTarget = (target_.transform.position - transform.position).normalized;
			// 自身の前方を指す単位ベクトル
			var fowerd = transform.forward;

			// 単位ベクトル同士の内積で cos を求める
			var dot = Vector3.Dot(fowerd, toTarget);
			// 内積がほぼ 1 の場合は同じ向きなので回転しない
			if (0.999f < dot) { return; }

			// cos から角度（ラジアン）を求める
			var radian = Mathf.Acos(dot);

			// 外積で回転軸を求める
			var cross = Vector3.Cross(fowerd, toTarget);			
			// 回転軸が上向きか下向きかで角度を反転させる
			radian *= (cross.y / Mathf.Abs(cross.y));

			// 角度を指定して回転させる
			transform.rotation *= Quaternion.Euler(0, Mathf.Rad2Deg * radian, 0);
		}
	}

}
