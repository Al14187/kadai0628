using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    /// <summary>  
    /// プレイヤー  
    /// </summary>  
    [SerializeField] private Player player_ = null;

    /// <summary>  
    /// ワールド行列   
    /// </summary>  
    private Matrix4x4 worldMatrix_ = Matrix4x4.identity;

    /// <summary>  
    /// ターゲットとして設定する  
    /// </summary>  
    /// <param name="enable">true:設定する / false:解除する</param>  
    public void SetTarget(bool enable)
    {
        // マテリアルの色を変更する  
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.materials[0].color = enable ? Color.red : Color.white;
    }

	/// <summary>
	/// 開始処理
	/// </summary>
	public void Start()
    {
        worldMatrix_ = Matrix4x4.TRS(transform.position, Quaternion.identity, Vector3.one);
    }

    /// <summary>  
    /// 更新処理  
    /// </summary>  
    public void Update()
    {

        // 自分の向きの単位ベクトルを取得する

        var normalZ = new Vector3(0, 0, 1);
        var forward = worldMatrix_ * normalZ;

        // プレイヤーまでの向きを単位ベクトルで取得する  
        var EnemytoPlayerNormal = (player_.transform.position - transform.position).normalized;

        // プレイヤーの視野角のコサイン値  
        var dot = Vector3.Dot(EnemytoPlayerNormal, forward);

        // 視野角は 20 度  
        var inViewCos = Mathf.Cos(20.0f * Mathf.Deg2Rad);

        // 視野角に入っている場合は追尾を開始する  
        if (inViewCos <= dot)
        {
            // 外積で回転軸を求める  
            var cross = Vector3.Cross(forward, EnemytoPlayerNormal);

            // コサインからラジアンを求める  
            var radian = Mathf.Min(Mathf.Acos(dot), (10.0f * Mathf.Deg2Rad));
            // 回転軸が上向きか下向きかで角度を反転させる  
            radian *= (cross.y / Mathf.Abs(cross.y));

            // y軸回転の角度をラジアンから度数に変換
            var localMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, Mathf.Rad2Deg * radian, 0));

            // ローカル変換を現在のワールド行列に反映
            worldMatrix_ = worldMatrix_ * localMatrix;

            // ローカル座標系のｚ軸方向に0.3の移動ベクトルを定義
            var forwadVec = new Vector3(0, 0, 0.3f);

            // ローカル移動ベクトルforwadVecをワールド空間に変換
            var move = worldMatrix_ * forwadVec;

            // ワールド行列の列を更新する  
            var pos = worldMatrix_.GetColumn(3) + move;
            //ワールド行列の座標をposに更新
            worldMatrix_.SetColumn(3, pos);
        }

        // ワールド行列から座標、回転、拡大縮小を取得して設定する  
        transform.position = worldMatrix_.GetColumn(3);
        transform.rotation = worldMatrix_.rotation;
        transform.localScale = worldMatrix_.lossyScale;
    }
}
