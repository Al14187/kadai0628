using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	/// <summary>
	/// 左側の線 
	/// </summary>
	[SerializeField]
	private GameObject left_;

	/// <summary>
	/// 右側の線 
	/// </summary>
	[SerializeField]
	private GameObject right_;

	/// <summary>
	/// 視野角(角度) 
	/// </summary>
	[SerializeField]
	private float viewAngle_ = 30.0f;

	/// <summary>
	/// プレイヤーのワールド行列 
	/// </summary>
	private Matrix4x4 worldMatrix_ = Matrix4x4.identity;

	public Matrix4x4 worldMatrix { get => worldMatrix_; }

	/// <summary>
	/// 視野角(ラジアン)
	/// </summary>
	public float viewRadian { get => viewAngle_ * Mathf.Deg2Rad; }

	/// <summary>
	/// 更新処理
	/// </summary>
	void Update()
	{
		// 回転
		float rad = 0.0f;
		if (Input.GetKey(KeyCode.A))
		{
			rad = -2.0f;
		}
		else if (Input.GetKey(KeyCode.S))
		{
			rad = 2.0f;
		}
		// 回転行列
		Matrix4x4 rotMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, rad, 0));

		// 平行移動
		Vector3 vec = new Vector3();
		if (Input.GetKey(KeyCode.UpArrow))
		{
			vec.z = 0.5f;
		}
		if (Input.GetKey(KeyCode.DownArrow))
		{
			vec.z = -0.5f;
		}
		if (Input.GetKey(KeyCode.RightArrow))
		{
			vec.x = 0.5f;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			vec.x = -0.5f;
		}

		// 平行移動行列
		var transMatrix = Matrix4x4.Translate(vec);

		// ワールド行列に現在のフレームの行列を掛け合わせて更新する
		worldMatrix_ = worldMatrix_ * (transMatrix * rotMatrix);

		// ワールド行列から座標、回転、拡大縮小を取得して設定する
		transform.position = worldMatrix_.GetColumn(3);
		transform.rotation = worldMatrix_.rotation;
		transform.localScale = worldMatrix_.lossyScale;

	}

	/// <summary>
	/// 視野角の線を更新
	/// </summary>
	private void FixedUpdate()
	{
		// 左側
		if (left_)
		{
			var localMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, -viewAngle_, 0));
			left_.transform.rotation = (worldMatrix_ * localMatrix).rotation;
			left_.transform.position = transform.position;
		}

		// 右側
		if (right_)
		{
			var localMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, viewAngle_, 0));
			right_.transform.rotation = (worldMatrix_ * localMatrix).rotation;
			right_.transform.position = transform.position;
		}

	}
}
